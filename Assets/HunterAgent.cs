using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using System;

public class HunterAgent : Agent {
  [SerializeField] private Material winMaterial;
  [SerializeField] private Material loseMaterial;
  [SerializeField] private SpriteRenderer floorSpriteRenderer;
  [SerializeField] Transform[] floorLimits;
  [SerializeField] private GameObject[] preysInsidePlayground;
  [SerializeField] private float moveSpeed = 2.5f;
  [SerializeField] private GameObject prey;
  public float visionRadius = 2.5f;
  private float previousDistanceToPrey = -1f;
  
  private Vector3 lastPosition;
  private int maxPreyChase = 3;
  private Rigidbody2D rb;
  public override void Initialize() {
    rb = GetComponent<Rigidbody2D>();
    lastPosition = transform.position;
  }

  public override void OnEpisodeBegin() {
    // Reiniciar la posición del agente
    float randomX = UnityEngine.Random.Range(floorLimits[0].position.x, floorLimits[1].position.x);
    float randomY = UnityEngine.Random.Range(floorLimits[0].position.y, floorLimits[1].position.y);
    transform.position = new Vector3(randomX, randomY, 0f);
    // Reiniciar la velocidad del agente
    rb.velocity = Vector2.zero;
  }

  public override void OnActionReceived(ActionBuffers actions) {
    // Obtener las acciones del agente
    float moveX = actions.ContinuousActions[0];
    float moveY = actions.ContinuousActions[1];
    Vector2 move = new Vector2(moveX, moveY);
    // Mover el agente usando Rigidbody2D
    rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

    // Recompensa por acercarse a la presa
    // Collider2D[] preyColliders = Physics2D.OverlapCircleAll(transform.position, visionRadius, LayerMask.GetMask("Prey"));
    // float minDistanceToPrey = Mathf.Infinity; 
    // foreach (var preyCollider in preyColliders) {
    //   float distanceToPrey = Vector2.Distance(transform.position, preyCollider.transform.position);
    //   if (distanceToPrey < minDistanceToPrey) {
    //     minDistanceToPrey = distanceToPrey;
    //   }
    // }
    // if (minDistanceToPrey < previousDistanceToPrey) {
    //   SetReward(0.3f);
    // }
    // previousDistanceToPrey = minDistanceToPrey;
    
    if (Vector2.Distance(transform.position, prey.transform.position) < previousDistanceToPrey) {
      SetReward(0.3f);
    }
    previousDistanceToPrey = Vector2.Distance(transform.position, prey.transform.position);

    // Penalización por moverse en círculos o quedarse quieto
    if (Vector2.Distance(transform.position, lastPosition) < 1f) {
        SetReward(-1f);
    }
    lastPosition = transform.position;
  }
  public override void CollectObservations(VectorSensor sensor) {
    // 3 observaciones por cada presa detectada
    // + 3 observaciones por cada pared detectada
    // en principio se pueden detectar 3 presas y 3 paredes, y por tanto 21 observaciones
    // int preyCount = 0;
    
    // Add observations for each prey detected
    // Collider2D[] preyColliders = Physics2D.OverlapCircleAll(transform.position, visionRadius, LayerMask.GetMask("Prey"));
    // Debug.Log("Presas " + preyColliders.Length);
    // foreach (var preyCollider in preyColliders) {
    //   Vector2 directionToPrey = (preyCollider.transform.position - transform.position).normalized;
    //   float distanceToPrey = Vector2.Distance(transform.position, preyCollider.transform.position);
    //   sensor.AddObservation(directionToPrey);
    //   sensor.AddObservation(distanceToPrey);
    //   // preyCount++;
    // }
    Vector2 directionToPrey = (prey.transform.position - transform.position).normalized;
    float distanceToPrey = Vector2.Distance(transform.position, prey.transform.position);
    sensor.AddObservation(directionToPrey);
    sensor.AddObservation(distanceToPrey);
  }

  public override void Heuristic(in ActionBuffers actionsOut) {
    ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
    continuousActions[0] = Input.GetAxis("Horizontal");
    continuousActions[1] = Input.GetAxis("Vertical");
  }

  public void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.CompareTag("Prey")) {
      UnityEngine.Debug.Log("Ganaste");
      SetReward(10f);
      floorSpriteRenderer.material = winMaterial;
      floorSpriteRenderer.sortingOrder = -1;
      EndEpisode();
    } else if (collision.collider.CompareTag("Wall")) {
      UnityEngine.Debug.Log("Perdiste");
      SetReward(-2f);
      floorSpriteRenderer.material = loseMaterial; 
      floorSpriteRenderer.sortingOrder = -1;
    }
  }
  void OnDrawGizmos() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, visionRadius);
  }
}
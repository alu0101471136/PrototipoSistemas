using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SimpleAgent2D : Agent
{
    public float moveSpeed = 4f;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private SpriteRenderer floorMeshRenderer;

    public GameObject p1; // Esquina inferior izquierda del mapa
    public GameObject p2; // Esquina superior derecha del mapa
    public LayerMask obstacleLayer;

    private float survivalTime = 0f; // Tiempo que el agente ha sobrevivido
    private float survivalRewardThreshold = 4f; // Umbral para la recompensa por supervivencia (10 segundos, por ejemplo)

    Vector3 GenerateRandomPosition(Vector2 bottomLeft, Vector2 topRight)
    {
        float randomX = Random.Range(bottomLeft.x, topRight.x);
        float randomY = Random.Range(bottomLeft.y, topRight.y);

        return new Vector3(randomX, randomY, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observa la posición relativa del enemigo
        sensor.AddObservation(transform.position - targetTransform.transform.position);

        // Observa la distancia al enemigo
        sensor.AddObservation(Vector3.Distance(transform.position, targetTransform.transform.position));

        // Direcciones ortogonales
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,    // Arriba
            Vector2.down,  // Abajo
            Vector2.left,  // Izquierda
            Vector2.right  // Derecha
        };

        foreach (Vector2 direction in directions)
        {
            // Realiza un raycast en la dirección ortogonal
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.55f, obstacleLayer);

            sensor.AddObservation(hit.collider != null ? 1 : 0); // Agrega la observación: 1 si hay una pared, 0 si no hay pared
        }
    }

    private Vector3 previousPosition;
    private float stationaryTime;
    private float positionUpdateInterval = 0.5f; // Intervalo de actualización en segundos
    private float positionUpdateTimer = 0f;

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Obtiene la dirección de movimiento normalizada
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        float moveY = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        Vector2 movement = new Vector2(moveX, moveY).normalized;
        // Comprobar si el movimiento lleva a una colisión
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, 0.55f, obstacleLayer);
        if (hit.collider != null) // Penalización por caminar contra una pared
        {
            AddReward(-0.25f);
        }
        else // Aplicar movimiento si no hay colisión
        {
            transform.position += new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed; // Actualiza la posición del agente
        }

        // Verificar si está estacionario (actualización cada x tiempo)
        positionUpdateTimer += Time.deltaTime;
        if (positionUpdateTimer >= positionUpdateInterval)
        {
            positionUpdateTimer = 0f; // Reinicia el temporizador
            if (Vector3.Distance(transform.position, previousPosition) < 1.5f)
            {
                stationaryTime += positionUpdateInterval;
                if (stationaryTime > 0.5f) // Penaliza después de 1 segundo en el mismo lugar
                {
                    AddReward(-1.5f * stationaryTime); // Penalización por estar estacionario
                }
            }
            else
            {
                stationaryTime = 0f; // Reinicia el contador si se mueve
            }
            previousPosition = transform.position; // Actualiza la posición anterior
        }

        // Aumenta el tiempo de supervivencia
        // survivalTime += Time.deltaTime;

        AddReward(Time.deltaTime); // Recompensa por sobrevivir durante un tiempo
        
        //if (survivalTime >= survivalRewardThreshold) // Si el agente ha sobrevivido más de un umbral, le damos una recompensa
        //{  
        //    survivalTime = 0f; // Reseteamos el temporizador
        //}
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void Update()
    {
        // Ajusta las recompensas según la distancia // Calcula la distancia entre el agente y el enemigo
        Debug.Log($"Current Reward: {GetCumulativeReward()}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hunter") || collision.TryGetComponent<Enemigo>(out Enemigo ene))
        {
            SetReward(-10f); // Penalización alta por ser atrapado
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        // Reinicia la posición del agente y de los enemigos
        transform.position = GenerateRandomPosition(p1.transform.position, p2.transform.position);

        targetTransform.position = GenerateRandomPosition(p1.transform.position, p2.transform.position);

        // Asegura que el enemigo no comience demasiado cerca del agente
        while (Vector3.Distance(transform.position, targetTransform.position) < 3f)
        {
            targetTransform.position = GenerateRandomPosition(p1.transform.position, p2.transform.position);
        }

        // Reinicia el tiempo de supervivencia
        survivalTime = 0f;
    }
}

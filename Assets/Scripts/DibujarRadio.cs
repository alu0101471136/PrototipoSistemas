using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;

// No se usa, en principio buscaba representar una esfera en 2D para representar el radio de visión del cazador 
// pero no se logró hacerlo de forma correcta
public class DibujarRadio : MonoBehaviour {
  [SerializeField] private HunterAgent predator;
  private SpriteRenderer spriteRenderer;
  private float visionRadius = 5f;
  public void Start() {
    visionRadius = predator.visionRadius;
    float angle = 2 * Mathf.PI / 100;
    UnityEngine.Vector3 coordX = new UnityEngine.Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * visionRadius;
    UnityEngine.Vector3 coordY = new UnityEngine.Vector3(Mathf.Cos(angle / 25), Mathf.Sin(angle / 25), 0) * visionRadius;
    transform.localScale = new UnityEngine.Vector3(2 * coordX.x, 2 * coordY.y, 1);
    spriteRenderer = GetComponent<SpriteRenderer>();
    spriteRenderer.color = new Color(1, 0, 0, 0.3f);
  }

  public void Update() {
    transform.position = predator.transform.position;
  }
  void OnDrawGizmosSelected() {
      Gizmos.color = new Color(1, 0, 0, 0.3f); // Color rojo semitransparente
      DrawCircle(transform.position, visionRadius);
  }
  void DrawCircle(UnityEngine.Vector3 center, float radius) {
    int segments = 100;
    float angle = 0f;
    UnityEngine.Vector3 lastPoint = center + new UnityEngine.Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
    for (int i = 1; i <= segments; i++) {
      angle += 2 * Mathf.PI / segments;
      UnityEngine.Vector3 nextPoint = center + new UnityEngine.Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
      Gizmos.DrawLine(lastPoint, nextPoint);
      lastPoint = nextPoint;
    }
  }
}
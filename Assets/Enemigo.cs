using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform target; // El objetivo a seguir
    public float speed = 3f; // Velocidad de movimiento
    private void Update()
    {
        // Calcular la distancia al objetivo
        float distance = Vector2.Distance(transform.position, target.position);
        // Calcular la dirección hacia el objetivo
        Vector2 direction = (target.position - transform.position).normalized;
        // Mover al objeto hacia el objetivo
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}

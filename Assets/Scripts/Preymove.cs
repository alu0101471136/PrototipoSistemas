using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preymove : MonoBehaviour {
    [SerializeField] Transform[] floorLimits;
    [SerializeField] float speed = 5f;
    [SerializeField] private Transform Hunter;

    public void Start() {}
    public void Update() {
        Vector2 direction = transform.position - Hunter.position;
        transform.position += (Vector3)direction.normalized * speed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotate : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLight : MonoBehaviour
{
    public float rotationSpeed = 0.6f;

    void Update()
    {
        // Rotate the sun around its own axis.
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}

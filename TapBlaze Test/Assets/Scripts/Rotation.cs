using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5;
    
    void Update()
    {
        transform.Rotate (0,0,rotateSpeed * Time.deltaTime);
    }
}

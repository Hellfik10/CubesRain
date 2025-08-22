using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    public event Action CollisionDetected;

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.collider.gameObject.layer & _layerMask.value) != 0)
        {
            CollisionDetected.Invoke();
        }
    }
}

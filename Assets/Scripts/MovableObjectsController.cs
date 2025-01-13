using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectsController : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask placeLayer;

    [Header("Movable Objects")]
    [SerializeField] private List<MovableObject> movableObjectsList;

    private const float defaultConstZValue = -1f;

    private void Awake()
    {
        InitializeMovableObjects();
    }

    private void InitializeMovableObjects()
    {
        foreach (var movableObj in movableObjectsList)
        {
            movableObj.Initialize(placeLayer, defaultConstZValue);
        }
    }
}

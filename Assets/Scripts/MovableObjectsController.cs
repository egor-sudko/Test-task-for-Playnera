using UnityEngine;

public class MovableObjectsController : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask placeLayer;

    private const float defaultConstZValue = -1f;

    private void Awake()
    {
        InitializeMovableObjects();
    }

    private void InitializeMovableObjects()
    {
        MovableObject[] movableObjects = transform.GetComponentsInChildren<MovableObject>();

        foreach (var movableObj in movableObjects)
        {
            movableObj.Initialize(placeLayer, defaultConstZValue);
        }
    }
}

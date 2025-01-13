using UnityEngine;

public class MovableObjectsController : MonoBehaviour
{
    // All this data is better to be moved to the Data file with the game settings and loaded when loading the level.
    // You can use Zenject for initialization, etc.

    [Header("Layers")]
    [SerializeField] private LayerMask placeLayer;  // layer to check if an object can be placed in the checked location

    private const float defaultConstZValue = -1f; // default Z value so that objects are drawn on top of the background

    private void Awake()
    {
        InitializeMovableObjects();// initialize objects for movement
    }

    private void InitializeMovableObjects()
    {
        MovableObject[] movableObjects = transform.GetComponentsInChildren<MovableObject>(); // get child elements with the required script

        foreach (var movableObj in movableObjects)
        {
            movableObj.Initialize(placeLayer, defaultConstZValue);
        }
    }
}

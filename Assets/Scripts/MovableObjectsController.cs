using UnityEngine;

public class MovableObjectsController : MonoBehaviour
{
    // все эти данные лучше вынести в Data файл с настройками игры и подгружать при загрузке уровня. Можно использовать Zenject
    // для инициализации и т.п.
    [Header("Layers")]
    [SerializeField] private LayerMask placeLayer; // слой для проверки, можно ли разместить объект в проверяемом месте

    private const float defaultConstZValue = -1f; // значение Z по дефолту, чтобы объекты отрисовывались поверх фона 
    
    private void Awake()
    {
        InitializeMovableObjects();// инициализируем объекты для перемещения
    }

    private void InitializeMovableObjects()
    {
        MovableObject[] movableObjects = transform.GetComponentsInChildren<MovableObject>();// получаем дочерние элементы с нужным скриптом

        foreach (var movableObj in movableObjects)
        {
            movableObj.Initialize(placeLayer, defaultConstZValue);
        }
    }
}

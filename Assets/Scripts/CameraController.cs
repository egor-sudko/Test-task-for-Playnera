using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    // класс отвечает за размер и перемещение камеры

    [SerializeField] private SpriteRenderer backgroundSprite;// спрайт фона уровня
    [Space]
    [SerializeField] private LayerMask movableObjectsLayer;// слой объектов для перемещения 

    private Vector2 startPosition;
    private float borderX;

    private bool canMove;

    private Bounds bounds;// границы фона

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        bounds = backgroundSprite.bounds;

        // устанавалием размер камеры в зависимости от фона
        SetCameraSizeToBackgroundSprite();
        // находим границы фона, чтобы камера не выходила за его пределы
        SetCameraXBorderToBackgroundSprite();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            // смотрим, чтобы не был выбран объект для перемещения. Если выбран, то камеру не двигаем
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
           Mathf.Infinity, movableObjectsLayer);

            if (!hit) canMove = true;
            else canMove = false;
        }
        else if (Input.GetMouseButton(0) && canMove) // если можно перемещать камеру, то перемещаем
        {
            float posX = _camera.ScreenToWorldPoint(Input.mousePosition).x - startPosition.x;

            // через Clamp ставим границы камеры
            transform.position = new Vector3(Mathf.Clamp(transform.position.x - posX, -borderX, borderX),
                transform.position.y, transform.position.z);
        }
    }

    private void SetCameraSizeToBackgroundSprite()
    {
        _camera.orthographicSize = backgroundSprite.bounds.size.y / 2f;
    }

    private void SetCameraXBorderToBackgroundSprite()
    {
        float ratio = (float)Screen.width / Screen.height;
        float val = backgroundSprite.bounds.size.y / 2f - _camera.orthographicSize;
        borderX = (bounds.size.x / 2) - _camera.ViewportToWorldPoint(new Vector2(1, 1)).x + (val * ratio);
    }
}

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    // the class is responsible for the size and movement of the camera

    [SerializeField] private SpriteRenderer backgroundSprite;// level background sprite
    [Space]
    [SerializeField] private LayerMask movableObjectsLayer;// layer of objects to move

    private Vector2 startPosition;
    private float borderX;

    private bool canMove;

    private Bounds bounds;// background borders

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        bounds = backgroundSprite.bounds;

        // we set the camera size depending on the background
        SetCameraSizeToBackgroundSprite();
        
        // we find the boundaries of the background so that the camera does not go beyond it
        SetCameraXBorderToBackgroundSprite();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            //we make sure that the object for moving is not selected. If it is selected, then we do not move the camera
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
           Mathf.Infinity, movableObjectsLayer);

            if (!hit) canMove = true;
            else canMove = false;
        }
        else if (Input.GetMouseButton(0) && canMove) //if it is possible to move the camera, then we move it
        { 

            float posX = _camera.ScreenToWorldPoint(Input.mousePosition).x - startPosition.x;

            // Using Clamp we set the camera boundaries
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

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour
{
    private bool isDrag;
    private Vector3 offset;

    private bool isWaitCollision;

    private LayerMask placeLayer;
    private float defaultPositionZ;

    private Camera _camera;
    private Rigidbody2D _rb;

    public void Initialize(LayerMask placeLayer, float defaultPositionZ)
    {
        this.placeLayer = placeLayer;
        this.defaultPositionZ = defaultPositionZ;

        transform.position = new Vector3(transform.position.x, transform.position.y, defaultPositionZ);

        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();

        _rb.freezeRotation = true;
        _rb.isKinematic = true;
    }

    private void Update()
    {
        if (isDrag)
        {
            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition) + offset;

            float posZ = (transform.position.y / 100f) + defaultPositionZ;
            transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
        isDrag = true;
    }

    private void OnMouseUp()
    {
        isDrag = false;

        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
            Mathf.Infinity, placeLayer);

        if (!hit)
        {
            _rb.isKinematic = false;
            isWaitCollision = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isWaitCollision)
        {
            _rb.isKinematic = true;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            isWaitCollision = false;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour
{
    // class with logic of the object operation for moving

    private bool isDrag;
    private Vector3 offset;

    private bool isWaitCollision;

    private LayerMask placeLayer;
    private float defaultPositionZ;

    private Camera _camera;
    private Rigidbody2D _rb;

    public void Initialize(LayerMask placeLayer, float defaultPositionZ)// initialize the object
    {
        this.placeLayer = placeLayer;
        this.defaultPositionZ = defaultPositionZ;

        transform.position = new Vector3(transform.position.x, transform.position.y, defaultPositionZ);

        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();

        /* since the technical specifications stated that objects should fall with physics, a rigid body was added.
         * But I would rather use movement animation instead of physics, since it is used in the reference game.
         * There is also a check for nearby objects that you can fly up to and take a place, and not just fall down until you stand on something */

        _rb.freezeRotation = true;// remove rotations if you haven't done it through the inspector
        _rb.isKinematic = true;// we make the body kinematic for normal movement
    }

    private void Update()
    {
        if (isDrag)// if it is possible to move an object, then we move it
        {
            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition) + offset;

            // changing the Z position with reference to Y. Made to create the effect of "depth" on the 2D scene
            float posZ = (transform.position.y / 100f) + defaultPositionZ;
            transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
        }
    }

    // if you click on an object, we record the position and allow it to move
    private void OnMouseDown()
    {
        offset = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
        isDrag = true;
    }

    // remove your finger from the object - finish moving
    private void OnMouseUp()
    {
        isDrag = false;

        /* we launch a ray from the tap point to check what the object intersects with.
         * if the object is above the object on which we can place it, then we leave it in the same place */

        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
            Mathf.Infinity, placeLayer);

        // if not, then we let the object fly down under the influence of physics until it collides with an object that can be stood on
        if (!hit)
        {
            _rb.isKinematic = false;
            isWaitCollision = true;
        }
    }

    // we check the collision with the collider of the place where the object can be placed. since in the physics settings the
    // Movable layer interacts only with Place, then you can not be afraid that it will become somewhere else
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (isWaitCollision)
        {
            // we make it kinematic again and reduce the speeds
            _rb.isKinematic = true;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            isWaitCollision = false;
        }
    }
}

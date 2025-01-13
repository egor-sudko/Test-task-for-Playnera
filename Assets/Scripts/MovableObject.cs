using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour
{
    // класс с логикой работы объекта для перемещения

    private bool isDrag;
    private Vector3 offset;

    private bool isWaitCollision;

    private LayerMask placeLayer;
    private float defaultPositionZ;

    private Camera _camera;
    private Rigidbody2D _rb;

    public void Initialize(LayerMask placeLayer, float defaultPositionZ)// инициализируем объект
    {
        this.placeLayer = placeLayer;
        this.defaultPositionZ = defaultPositionZ;

        transform.position = new Vector3(transform.position.x, transform.position.y, defaultPositionZ);

        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();

        // т.к. в ТЗ указывалось, что нужно чтобы предметы падали с физикой, то добавили твердое тело.
        // но я бы лучше использовал анимацию перемещения вместо физики, т.к. в игре референсе используется именно она
        // + там идет проверка на ближайшие объекты, к которым можно подлететь и занять место, а не просто упасть вниз пока не станем на что-то

        _rb.freezeRotation = true;// убираем вращения, если не сделали это через инспектор
        _rb.isKinematic = true;// делаем тело кинематическим для нормального перемещения 
    }

    private void Update()
    {
        if (isDrag)// если можно перемещать объект, то перемещаем
        {
            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition) + offset;

            // изменение позиции Z с привязкой к Y. Сделано чтобы создать эффет "глубины" на 2D сцене
            // Чем выше Y, тем дальше по Z
            float posZ = (transform.position.y / 100f) + defaultPositionZ;
            transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
        }
    }

    private void OnMouseDown() // если нажали на объект, то записываем позицию и разрешаем двигать
    {
        offset = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
        isDrag = true;
    }

    private void OnMouseUp()// убрали палец с объекта - закончили перемещение
    {
        isDrag = false;

        // пускаем луч из точки тапа, чтобы проверить с чем объект пересекается.
        // если объект над объектом, на который можно поставить, то оставляем его на этом же месте

        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
            Mathf.Infinity, placeLayer);

        if (!hit)// если нет, то отпускаем объект лететь вниз под действием физики, пока он не столкнется с объектом на который можно стать
        {
            _rb.isKinematic = false;
            isWaitCollision = true;
        }
    }

    // проверяем столкновение с коллайдером места, на которое можно поставить объект.
    // т.к. в настройках физики слой Movable взаимодействует только с Place, то можно не бояться, что она станет куда-то еще
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (isWaitCollision)
        {
            _rb.isKinematic = true;// делаем вновь кинематическим и сбрасываем скорости
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            isWaitCollision = false;
        }
    }
}

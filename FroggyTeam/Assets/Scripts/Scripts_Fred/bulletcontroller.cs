using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


public class bulletcontroller : MonoBehaviour
{
    public UnityEvent BulletTargetCollision;
    public UnityEvent PressedSpace;
    public UnityEvent PressedR;
    [SerializeField]
    float turnRate = 0.1f;

    float currentTurn = 0f;
    private int turnDirection = 0;


    [SerializeField]
    float maxTurn = 5.0f;

    [SerializeField]
    float forwardSpeed = 5.0f;

    [SerializeField]
    CameraMovement bulletCamera;

    [SerializeField]
    GameObject bullet;

    public InputSystem_Actions inputs;
    public InputAction jump;
    public InputAction retry;
    private float rotation_input;
    private float forward_input = 0.0f;

    private Vector2 leftStick;

    private bool hasStarted;

    private Renderer bulletRenderer;

    private void Start()
    {
        hasStarted = false;
        bulletRenderer = bullet.GetComponent<Renderer>();
        bulletRenderer.material.color = GameManager.instance.bulletColor;
    }

    private void OnEnable()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();
        inputs.Player.Move.performed += OnMove;
        inputs.Player.Move.canceled += OnMoveCancelled;
        jump = inputs.Player.Jump;
        jump.performed += OnShoot;
        retry = inputs.Player.Retry;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        rotation_input = context.ReadValue<Vector2>().x;
        Debug.Log("Rotation Input: " + rotation_input);
        /*if(context.ReadValue<Vector2>().y > 0.0f)
        {
            forward_input = context.ReadValue<Vector2>().y;
        }*/
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        Debug.Log("Rotation Input: " + rotation_input);
        rotation_input = 0;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (!hasStarted)
        {
            StartCoroutine(WaitForCam());
            bulletCamera.StartCamMovement();
            PressedSpace.Invoke();
            hasStarted = true;
        }
    }

    private void OnRPressed(InputAction.CallbackContext context)
    {
        PressedR.Invoke();
    }

    private void FixedUpdate()
    {
        if (rotation_input == 0)
        {
            currentTurn = 0f;
        }
        else if (rotation_input <= 0) {
            if(currentTurn > 0f)
            {
                currentTurn = 0f;
            }
            currentTurn = Mathf.Max(currentTurn - (turnRate), (-maxTurn));
        }
        else
        {
            if (currentTurn < 0f)
            {
                currentTurn = 0f;
            }
            currentTurn = Mathf.Min(currentTurn + (turnRate), (maxTurn));
        }

        if (forward_input != 0.0f) transform.Rotate(Vector3.up, currentTurn * Time.deltaTime);
    }

    private void Update()
    {
        GetComponent<Rigidbody>().linearVelocity = (transform.forward * forward_input);
    }

    private IEnumerator WaitForCam()
    {
        yield return new WaitForSeconds(1);
        forward_input = forwardSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision);
    }

    private void CheckCollision(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Target":
                OnHitTarget(collision.gameObject);
                BulletTargetCollision.Invoke();
                break;
            case "Reflect":
                OnHitReflect(collision);
                break;
            default:
                OnHitEnvironment();
                break;
        }
    }

    private void OnHitEnvironment()
    {
        StopBullet();

        FindFirstObjectByType<GameManager>().OnLose();
    }

    public void StopBullet()
    {
        forward_input = 0;
        forwardSpeed = 0;
        rotation_input = 0;
        turnRate = 0;
    }

    private void OnHitTarget(GameObject collision)
    {
        collision.GetComponentInParent<TargetBehaviour>().OnTargetHit();
    }

    private void OnHitReflect(Collision collision)
    {
        float angle = Vector3.SignedAngle(-gameObject.transform.forward, collision.contacts[0].normal, Vector3.up);

        float angleFactor = Mathf.Sign(collision.contacts[0].normal.x) * Mathf.Sign(collision.contacts[0].normal.z);


        Vector3 direction = Quaternion.AngleAxis(angle * 2, Vector3.up) * -gameObject.transform.forward;

        transform.forward = direction;

        collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
    private void OnDisable()
    {
        inputs.Player.Move.performed -= OnMove;
        inputs.Player.Move.canceled -= OnMoveCancelled;
        jump.performed -= OnShoot;
        inputs.Player.Disable();
    }
}

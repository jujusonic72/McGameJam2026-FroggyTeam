using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class bulletcontroller : MonoBehaviour
{
    [SerializeField]
    float turnRate = 5.0f;

    [SerializeField]
    float forwardSpeed = 5.0f;

    [SerializeField]
    CameraMovement bulletCamera;

    private InputSystem_Actions _inputs;
    private float rotation_input;
    private float forward_input = 0.0f;

    private void OnEnable()
    {
        _inputs = new InputSystem_Actions();
        _inputs.Player.Move.performed += OnMove;
        _inputs.Player.Jump.performed += OnShoot;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        rotation_input = context.ReadValue<Vector2>().x;
        
        /*if(context.ReadValue<Vector2>().y > 0.0f)
        {
            forward_input = context.ReadValue<Vector2>().y;
        }*/
    }

    private void OnShoot(InputAction.CallbackContext context)
    {

    }

    private void FixedUpdate()
    {

        if(forward_input != 0.0f) transform.Rotate(Vector3.up, rotation_input * turnRate * Time.deltaTime);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rotation_input = turnRate;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotation_input = -turnRate;
        }
        else
        {
            rotation_input = 0.0f;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            forward_input = forwardSpeed;
            bulletCamera.canMove = true;
        }

        //Continue à slide rn comme une balle, W est basically juste pour tirer
        if (Input.GetKey(KeyCode.W))
        {
            forward_input = turnRate;
        }

        Vector3 move = new Vector3(0, 0, forward_input);

        GetComponent<Rigidbody>().linearVelocity = (transform.forward * forward_input);

        //transform.Translate(move * 5.0f * Time.deltaTime, Space.Self);
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
        float angle = Vector3.Angle(-gameObject.transform.forward, collision.contacts[0].normal);

        Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * -gameObject.transform.forward;

        //TODO: DID NOT FIGURE OUT HOW TO CALCULATE THE ANGLE, TO CHECK
        transform.forward = collision.contacts[0].normal;
        print(angle);
    }

}

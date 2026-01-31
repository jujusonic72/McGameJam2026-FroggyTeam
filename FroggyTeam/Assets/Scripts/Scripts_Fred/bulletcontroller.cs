using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class bulletcontroller : MonoBehaviour
{
    [SerializeField]
    float turnRate = 5.0f;

    [SerializeField]
    float forwardSpeed = 5.0f;

    [SerializeField]
    Camera bulletCamera;

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
        print("hit");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static int commandsIssued = 0;
    public static int CommandsIssued
    {
        get { return commandsIssued; }
        set { commandsIssued = value; }
    }

    private KeyCode previousKey = KeyCode.None;
    private KeyCode currentKey;

    public float movementSpeed = 8f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;

    public bool isGrounded;

    public CharacterController controller;
    public Vector3 velocityVector;
    public GameObject groundCheck;
    public LayerMask groundMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);

            if (isGrounded && velocityVector.y < 0)
                velocityVector.y = -2f;

            float xAxis = Input.GetAxis("Horizontal");
            float zAxis = Input.GetAxis("Vertical");

            Vector3 movementVector = transform.right * xAxis + transform.forward * zAxis;
            controller.Move(movementVector * movementSpeed * Time.deltaTime);

            velocityVector.y += gravity * Time.deltaTime;
            controller.Move(velocityVector * Time.deltaTime);

            currentKey = GetKeyCode();
            if (currentKey != previousKey)
                CommandsIssued++;
            previousKey = currentKey;
        }
    }

    KeyCode GetKeyCode()
    {
        foreach(KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
                return keyCode;
        }
        return KeyCode.None;
    }
}
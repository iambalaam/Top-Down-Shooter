using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // You cannot have const values that are also editable in unity
    [SerializeField] private bool useMouseInput = true;
    [SerializeField] [Range(0f, 1000f)] private float rotationSpeed = 1000f;
    [SerializeField] [Range(0f, 20f)] private float walkSpeed = 5f;
    [SerializeField] [Range(0f, 20f)] private float runSpeed = 10f;

    private CharacterController controller;
    private Camera cam;
    private Quaternion targetRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 keyboardInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (useMouseInput)
        {
            UpdateView(GetRelativeMousePosition());
            UpdatePosition(keyboardInput);
        }
        else
        {
            UpdateView(keyboardInput);
            UpdatePosition(keyboardInput);
        }


    }

    Vector3 GetRelativeMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        // Copied from tutorial, I do not understand this coordinate change.
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            cam.transform.position.y - transform.position.y
            ));
        Vector3 mousePlayerPos = new Vector3(mouseWorldPos.x - transform.position.x, 0, mouseWorldPos.z - transform.position.z);
        return mousePlayerPos;
    }

    void UpdateView(Vector3 direction)
    {
        if (direction != null && direction != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(direction);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(
                transform.eulerAngles.y,
                targetRotation.eulerAngles.y,
                rotationSpeed * Time.deltaTime
                );
        }
    }
    void UpdatePosition(Vector3 velocity)
    {
        if (velocity != null && velocity != Vector3.zero)
        {
            float speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
            Vector3 motion = velocity.normalized * speed;
            controller.Move(motion * Time.deltaTime);
        };
    }
}

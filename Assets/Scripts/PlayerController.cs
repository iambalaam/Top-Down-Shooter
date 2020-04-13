using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // You cannot have const values that are also editable in unity
    [SerializeField] [Range(0f,1000f)] private float rotationSpeed = 1000f;
    [SerializeField] [Range(0f,  20f)] private float walkSpeed = 5f;
    [SerializeField] [Range(0f,  20f)] private float runSpeed = 10f;

    private CharacterController controller;
    private Quaternion targetRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input != Vector3.zero)
        {
            // Set rotation immediately
            // transform.rotation = Quaternion.LookRotation(input);

            // Tween to new roatation
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(
                transform.eulerAngles.y,
                targetRotation.eulerAngles.y,
                rotationSpeed * Time.deltaTime
                );

            // Move character
            float speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
            Vector3 motion = input.normalized * speed;
            controller.Move(motion * Time.deltaTime);
        };

    }
}

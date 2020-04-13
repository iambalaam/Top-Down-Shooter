using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    private GameObject player;
    [SerializeField] [Range(0f, 10f)] private float cameraSpeed = 5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 cameraTarget = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraTarget, cameraSpeed * Time.deltaTime);
    }
}

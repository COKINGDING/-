using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCam : MonoBehaviour
{


    private Transform camTr;
    private Transform playerTr;
    private GameObject player;

    private Vector3 velocity = Vector3.zero; 
    public float smoothTime = 0.4f;

    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;

    [Range(1.0f, 10.0f)]
    public float height = 2.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        camTr = GetComponent<Transform>();
        playerTr = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {

        Vector3 playerPos = playerTr.position - playerTr.forward * distance + playerTr.up * height;
        camTr.position = Vector3.SmoothDamp(camTr.position, playerPos, ref velocity, smoothTime);

        camTr.LookAt(playerTr.position + playerTr.up * 2);

        // Vector3.SmoothDamp(Vector3 current, Vector3 target, ref Vector3 Velocity, float smoothTime, float maxSpeed, float deltaTime);
    }
}

using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject playerFollow;
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    private float movementX = 0.0f;
    private float movementZ = 0.0f;
    private float _rotationVelocity;
    private RaycastHit hit;
    private Rigidbody rb;
    private bool rightMovement = false;
    private bool leftMovement = false;

    void Start()
    {
        playerFollow = GameObject.Find("PlayerFollow");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (UnityEngine.Input.GetKey("w"))
        {
            transform.position += Vector3.forward * Time.deltaTime * movementSpeed; 
        }
        else if (UnityEngine.Input.GetKey("s"))
        {
            transform.position -= Vector3.forward * Time.deltaTime * movementSpeed;
        }

        if (UnityEngine.Input.GetKey("a"))
        {
            transform.position -= Vector3.right * Time.deltaTime * movementSpeed;
        }
        else if (UnityEngine.Input.GetKey("d"))
            transform.position += Vector3.right * Time.deltaTime * movementSpeed;

        playerFollow.transform.position = transform.position;

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Respawn")
        {
            Debug.Log("osu johonki");
        }
    }
}

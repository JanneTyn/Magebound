using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject playerFollow;
    public float movementSpeed = 1.0f;
    void Start()
    {
        playerFollow = GameObject.Find("PlayerFollow");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
            transform.position += Vector3.forward * Time.deltaTime * movementSpeed;
        if (Input.GetKeyDown("space"))
            transform.position += Vector3.up * Time.deltaTime * movementSpeed; 
        if (Input.GetKey("s"))
            transform.position += Vector3.back * Time.deltaTime * movementSpeed;
        if (Input.GetKey("a"))
            transform.position += Vector3.left * Time.deltaTime * movementSpeed;
        if (Input.GetKey("d"))
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

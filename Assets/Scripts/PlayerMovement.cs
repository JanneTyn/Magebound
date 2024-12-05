using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour, IStatusVariables
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject playerFollow;

    public float movementSpeed = 1.0f;
    public float speed 
    { 
        get => movementSpeed; 
        set => movementSpeed = value; 
    }

    public float rotationSpeed = 1.0f;
    private float movementX = 0.0f;
    private float movementZ = 0.0f;
    private float _rotationVelocity;
    private RaycastHit hit;
    private Rigidbody rb;
    private bool rightMovement = false;
    private bool leftMovement = false;
    private Vector3 newLoc;
    private Dash dash;
    Animator anim;


    void Start()
    {
        playerFollow = GameObject.Find("PlayerFollow");
        rb = GetComponent<Rigidbody>();
        dash = GetComponent<Dash>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<CharacterStats_PlayerStats>().playerDead)
        {
            if (!dash.dashIsActive)
            {
                anim.SetBool("Walking", false);
                Vector3 newLoc = Vector3.zero;

                // Collect input directions
                if (UnityEngine.Input.GetKey("w"))
                {
                    newLoc += Vector3.forward;
                    anim.SetBool("Walking", true);
                }
                if (UnityEngine.Input.GetKey("s"))
                {
                    newLoc -= Vector3.forward;
                    anim.SetBool("Walking", true);
                }
                if (UnityEngine.Input.GetKey("a"))
                {
                    newLoc -= Vector3.right;
                    anim.SetBool("Walking", true);
                }
                if (UnityEngine.Input.GetKey("d"))
                {
                    newLoc += Vector3.right;
                    anim.SetBool("Walking", true);
                }

                // Normalize movement vector to ensure consistent speed in all directions
                if (newLoc != Vector3.zero)
                {
                    newLoc.Normalize();
                    transform.position += newLoc * Time.deltaTime * speed;
                    transform.LookAt(transform.position + newLoc);
                }
            }
            playerFollow.transform.position = transform.position;
        }
    }

    private void FixedUpdate()
    {

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

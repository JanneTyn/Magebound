using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public int speed = 10;
    public int experienceAmmount = 20;
    public int pointAmmount = 420;
    private GameObject player;
    

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.GetComponent<LevelManager>() != null)
        {
            player.GetComponent<LevelManager>().GainExperience(experienceAmmount);
            player.GetComponent<CharacterStats_PlayerStats>().GainPoints(pointAmmount);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Other is null");
        }
    }
}

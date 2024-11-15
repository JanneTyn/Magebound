using UnityEngine;

public class SpellProjectile : SpellBaseEffect
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 projectileDir;
    public float projectileSpeed = 6;
    public float travelDistance = 50;
    private bool directionSet;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (directionSet)
        {          
            var step = projectileSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, projectileDir, step);
            Debug.Log("projectileDir:" + projectileDir);
        }

        if (transform.position == projectileDir) { Destroy(gameObject); }
    }


    public void SetProjectileDirection(Vector3 dir, Vector3 playerLoc)
    {
        projectileDir = dir;
        transform.LookAt(projectileDir);
        directionSet = true;
    }
}

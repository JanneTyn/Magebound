using UnityEngine;

public class EnemyHandOverlapCheck : MonoBehaviour
{
    public LayerMask m_LayerMask;

    void FixedUpdate()
    {
        MyCollisions();
    }

    void MyCollisions()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        /*int i = 0;

        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name + i);

            //Increase the number of Colliders in the array
            i++;
        }


        //Check if player has been hit
        for (int j = 0; j < hitColliders.Length; j++)
        {
            Debug.Log(hitColliders[j].name);

            if (hitColliders[j].name == "Player")
            {
                Debug.Log("Player hit");
            }
        }*/
    }
}

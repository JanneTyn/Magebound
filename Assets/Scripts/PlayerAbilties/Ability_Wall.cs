using UnityEngine;

public class Ability_Wall : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundLayer;
    private float distance;
    private Vector3 midpoint;
    private Vector3 direction;
    private Quaternion rotation;

    public void ActivateAbility(int elementID)
    {
        //Mouse Position
        Vector3 mousePosition = Input.mousePosition;

        //Camera ray
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundLayer))
        {
            distance = Vector3.Distance(transform.position, hitInfo.point);

            midpoint = (transform.position + hitInfo.point) / 2;

            direction = hitInfo.point - transform.position;

            rotation = Quaternion.LookRotation(direction);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 90, 0f);

            //midpoint += direction.normalized * 1f;
        }

        switch(elementID)
        {
            case 1:

                var fireWall = Instantiate(walls[0], midpoint, rotation).GetComponent<SpellEffect_WorldEffect_Wall>();
                fireWall.length = (distance - 1f) / 2;
                fireWall.size = 5;

                break;

            case 2:

                var iceWall = Instantiate(walls[1], midpoint, rotation).GetComponent<SpellEffect_WorldEffect_Wall>();
                iceWall.length = (distance - 1f) / 2;
                iceWall.size = 5;

                break;
            
            case 3:

                var electricWall = Instantiate(walls[2], midpoint, rotation).GetComponent<SpellEffect_WorldEffect_Wall>();
                electricWall.length = (distance - 1f) / 2;
                electricWall.size = 5;

                break;
        }
    }
}

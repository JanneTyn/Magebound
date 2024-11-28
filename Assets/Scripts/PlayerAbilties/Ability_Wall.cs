using UnityEngine;

public class Ability_Wall : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxWallSize = 10f;
    [SerializeField] private float minWallSize = 3f;
    private float distance;
    private Vector3 midpoint;
    private Vector3 spawnPosition;
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

            spawnPosition = hitInfo.point;

            direction = hitInfo.point - transform.position;

            rotation = Quaternion.LookRotation(direction);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);

            //midpoint += direction.normalized * 1f;
        }

        switch(elementID)
        {
            case 1:

                var fireWall = Instantiate(walls[0], spawnPosition, rotation).GetComponent<SpellEffect_WorldEffect_Wall>();
                fireWall.length = Mathf.Clamp((distance - 1f) / 2, minWallSize, maxWallSize);
                fireWall.size = 5;

                break;

            case 2:

                var iceWall = Instantiate(walls[1], midpoint, rotation).GetComponent<SpellEffect_WorldEffect_Wall>();
                iceWall.length = Mathf.Clamp((distance - 1f) / 2, minWallSize, maxWallSize);
                iceWall.size = 5;

                break;
            
            case 3:

                var electricWall = Instantiate(walls[2], midpoint, rotation).GetComponent<SpellEffect_WorldEffect_Wall>();
                electricWall.length = Mathf.Clamp((distance - 1f) / 2, minWallSize, maxWallSize);
                electricWall.size = 5;

                break;
        }
    }
}

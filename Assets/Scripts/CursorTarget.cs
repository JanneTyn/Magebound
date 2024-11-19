using Unity.VisualScripting;
using UnityEngine;

public class CursorTarget : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private SpellBaseEffect spellEffect;
    private GameObject player;
    private Vector3 attackTarget;
    private Camera cam;
    private Ray ray;
    private Ray rayAttack;
    private RaycastHit hit;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        cam = GetComponent<Camera>();
        spellEffect = GameObject.Find("SpellBaseEffect").GetComponent<SpellBaseEffect>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
        if (Input.GetMouseButtonDown(0)) AttackPrepare();
    }

    void AttackPrepare()
    {
        if (!CheckForTarget())
        {
            Debug.Log("Valid target not found");
            return;
        }

        if (hit.collider.CompareTag("Ground"))
        {
            Debug.Log("Ground target hit");
            Vector3 fixedPoint = hit.point;
            spellEffect.InitializeProjectile(player.transform.position, fixedPoint);
        }
    }

    bool CheckForTarget()
    {
        rayAttack = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    

}

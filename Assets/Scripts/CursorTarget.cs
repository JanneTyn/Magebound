using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CursorTarget : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public float rotationLockTime = 0.3f;
    private SpellBaseEffect spellEffect;
    private GameObject player;
    private Vector3 attackTarget;
    private Camera cam;
    private Ray ray;
    private Ray rayAttack;
    private RaycastHit[] hits;
    private Vector3 fixedPoint;

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
        if (Input.GetMouseButtonDown(0)) AttackPrepare(false);
        else if (Input.GetMouseButtonDown(1)) AttackPrepare(true);
    }

    void AttackPrepare(bool isDash)
    {
        if (!CheckForTarget())
        {
            Debug.Log("Ground target not found");
            return;
        }
        else
        {
            Debug.Log("Ground target hit");
            if (!isDash) 
            {
                spellEffect.InitializeProjectile(player.transform.position, fixedPoint, player.GetComponent<CharacterStats_PlayerStats>().GetCurrentElement()); 
                StartCoroutine(RotationLockOnTarget(fixedPoint));
            }
            else
            {
                fixedPoint.y = player.transform.position.y;
                player.GetComponent<Dash>().InitializeDash(player.transform.position, fixedPoint);
            }

        }
    }

    IEnumerator RotationLockOnTarget(Vector3 targetLoc)
    {
        var t = 0f;
        targetLoc.y = player.transform.position.y;
        player.transform.LookAt(targetLoc);
        Quaternion rotation = player.transform.rotation;
        while (t < rotationLockTime)
        {
            t += Time.deltaTime;
            player.transform.rotation = rotation;

            yield return null;
        }

    }

    bool CheckForTarget()
    {
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100.0f);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.CompareTag("Ground"))
            {
                fixedPoint = hit.point;
                return true;
            }
        }

        return false;

    }

    

}

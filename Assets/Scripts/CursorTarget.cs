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
    private SpellExplosion spellExplosion;
    private SpellShard spellShard;
    private GameObject player;
    private Vector3 attackTarget;
    private Camera cam;
    private Ray ray;
    private Ray rayAttack;
    private RaycastHit[] hits;
    private Vector3 fixedPoint;

    private void Start()
    {
        cam = GetComponent<Camera>();
        spellEffect = GameObject.Find("SpellBaseEffect").GetComponent<SpellBaseEffect>();
        spellExplosion = GameObject.Find("SpellBaseEffect").GetComponent<SpellExplosion>();
        spellShard = GameObject.Find("SpellBaseEffect").GetComponent<SpellShard>();
        player = GameObject.Find("Player");

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            hotSpot = new Vector2(150, 150);
            cursorMode = CursorMode.Auto;
        }
        else
        {
            hotSpot = new Vector2(68, 68);
            cursorMode = CursorMode.ForceSoftware;
        }
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void Update()
    {
        /*ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
        if (Input.GetMouseButtonDown(0)) AttackPrepare(0);
        else if (Input.GetMouseButtonDown(1)) AttackPrepare(1);
        else if (Input.GetKeyDown(KeyCode.E)) AttackPrepare(2); */
    }

    public void AttackPrepare(int attackID)
    {
        if (!CheckForTarget())
        {
            Debug.Log("Ground target not found");
            return;
        }
        else
        {
            Debug.Log("Ground target hit");
            if (attackID == 1) { InitializeAttack(1); }
            else
            {
                StartCoroutine(player.GetComponent<PlayerAnimations>().InitializeAttackAnimation(fixedPoint, attackID));
            }
        }
    }

    IEnumerator RotationLockOnTarget(Vector3 targetLoc, int attackID)
    {
        // Adjust the target location to the player's level
        AdjustTargetLocation(ref targetLoc);

        // Initiate attack animation
        StartAttackAnimation(attackID);

        // Duration of the attack
        float attackTime = 2f;
        Debug.Log("attacktime: " + attackTime);

        // Perform rotation and attack initialization during the attack duration
        yield return RotateAndAttack(targetLoc, attackID, attackTime);

        // Reset animation state
        ResetAttackAnimation();
    }

    void AdjustTargetLocation(ref Vector3 targetLoc)
    {
        targetLoc.y = player.transform.position.y;
        player.transform.LookAt(targetLoc);
    }

    void StartAttackAnimation(int attackID)
    {
        var animator = player.GetComponentInChildren<Animator>();
        if (attackID == 0)
            animator.SetBool("BasicAttackShot", true);
        else
            animator.SetBool("SkillAttack", true);
    }

    IEnumerator RotateAndAttack(Vector3 targetLoc, int attackID, float attackTime)
    {
        float elapsedTime = 0f;
        bool attackInitialized = false;

        while (elapsedTime < attackTime)
        {
            elapsedTime += Time.deltaTime;

            // Continuously rotate towards the target
            player.transform.LookAt(targetLoc);

            // Initialize the attack at the delay
            if (elapsedTime > 0.5f && !attackInitialized)
            {
                attackInitialized = true;
                InitializeAttack(attackID);
            }

            yield return null;
        }
    }

    void ResetAttackAnimation()
    {
        var animator = player.GetComponentInChildren<Animator>();
        animator.SetBool("BasicAttackShot", false);
        animator.SetBool("SkillAttack", false);
    }

    void InitializeAttack(int attackID)
    {
        switch (attackID)
        {
            case 0: //projectile
                spellEffect.InitializeProjectile(player.transform.position, fixedPoint, player.GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());          
                break;
            case 1: //Dash
                fixedPoint.y = player.transform.position.y;
                player.GetComponent<Dash>().InitializeDash(player.transform.position, fixedPoint, player.GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());
                break;
            case 2: //explosion
                spellExplosion.InitializeExplosion(player.transform.position, fixedPoint, player.GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());
                break;
            case 3: //shard
                fixedPoint.y = 0.4f;
                spellShard.InitializeShard(player.transform.position, fixedPoint, player.GetComponent<CharacterStats_PlayerStats>().GetCurrentElement());
                break;
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
                Debug.Log("HIT.POINT: " + hit.point);
                return true;
            }
        }
        return false;
    }
}

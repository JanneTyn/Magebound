using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StatusManager : MonoBehaviour, IStatusVariables
{
    public float speed { get; set; }

    private Coroutine burnCoroutine;
    private Coroutine freezeCoroutine;
    private Coroutine chillCoroutine;
    private Coroutine stunCoroutine;

    private bool isBurning;
    private bool isChilled;
    public void Activate(int statusID, float duration)
    {
        switch (statusID)
        {
            //Burning
            case 01:
                if(burnCoroutine == null)
                {
                    burnCoroutine = StartCoroutine(ApplyBurn(duration));
                }

                if (burnCoroutine != null && !isBurning)
                {
                    StopCoroutine(burnCoroutine);
                    burnCoroutine = StartCoroutine(ApplyBurn(duration));
                }

                break;

            //Freeze
            case 02:
                if (freezeCoroutine == null)
                {
                    freezeCoroutine = StartCoroutine(ApplyFreeze(duration));
                }
                break;

            //Chill
            case 03:
                if (chillCoroutine == null)
                {
                    chillCoroutine = StartCoroutine(ApplyChill(duration));
                }

                if (chillCoroutine != null && !isChilled)
                {
                    StopCoroutine(chillCoroutine);
                    chillCoroutine = StartCoroutine(ApplyChill(duration));
                }

                break;
            //Stun
            case 04:
                if (stunCoroutine == null)
                {
                    stunCoroutine = StartCoroutine(ApplyStun(duration));
                }
                break;

            default:
                Debug.Log("Invalid Status");
                break;
        }
    }

    public void Activate(int statusID, float duration, float damage)
    {
        switch (statusID)
        {
            //Burning
            case 01:
                if (burnCoroutine == null)
                {
                    burnCoroutine = StartCoroutine(ApplyBurn(duration));
                }

                if (burnCoroutine != null && !isBurning)
                {
                    StopCoroutine(burnCoroutine);
                    burnCoroutine = StartCoroutine(ApplyBurn(duration));
                }
                break;
            //Freeze
            case 02:
                if (freezeCoroutine == null)
                {
                    freezeCoroutine = StartCoroutine(ApplyFreeze(duration));
                }

                break;
            //Chill
            case 03:
                if (chillCoroutine == null)
                {
                    chillCoroutine = StartCoroutine(ApplyChill(duration));
                }

                if (chillCoroutine != null && !isChilled)
                {
                    StopCoroutine(chillCoroutine);
                    chillCoroutine = StartCoroutine(ApplyChill(duration));
                }
                break;
            //Stun
            case 04:
                if (stunCoroutine == null)
                {
                    stunCoroutine = StartCoroutine(ApplyStun(duration));
                }
                break;

            default:
                Debug.Log("Invalid Status");
                break;
        }
    }

    private IEnumerator ApplyBurn(float duration)
    {
        float elapsedTime = 0;

        if (!isBurning)
        {
            isBurning = true;
        }

        while (elapsedTime < duration)
        {

            yield return new WaitForSeconds(1f);

            this.gameObject.GetComponent<DamageSystem>().CalculateDamage(10, 1);

            elapsedTime += 1f;

            if (isBurning)
            {
                isBurning = false;
            }
        }
    }

    private IEnumerator ApplyBurn(float duration, float damage)
    {
        float elapsedTime = 0;

        if (!isBurning)
        {
            isBurning = true;
        }

        while (elapsedTime < duration)
        {

            yield return new WaitForSeconds(1f);

            this.gameObject.GetComponent<DamageSystem>().CalculateDamage(damage, 1);

            elapsedTime += 1f;

            if (isBurning)
            {
                isBurning = false;
            }
        }
    }



    private IEnumerator ApplyFreeze(float duration)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Freeze<PlayerMovement>(duration));
            StartCoroutine(Freeze<PlayerAbilitiesInput>(duration));
        }
        else if (this.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Freeze<EnemyAI>(duration));
        }

        yield return null;
    }

    private IEnumerator ApplyChill(float duration)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Chill<PlayerMovement>(duration));
        }
        else if (this.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Chill<NavMeshAgentWrapper>(duration));

        }
        yield return null;
    }

    private IEnumerator Chill<T>(float duration) where T : Behaviour, IStatusVariables
    {
        T component = this.GetComponent<T>();
        float originalSpeed = component.speed;

        if (component != null)
        {
            component.speed = originalSpeed / 10;

            yield return new WaitForSeconds(duration);

            component.speed = originalSpeed;
        }
    }

    private IEnumerator ApplyStun(float duration)
    {
        if (this.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Stun<PlayerMovement>(duration));
            StartCoroutine(Stun<PlayerAbilitiesInput>(duration));
        }
        else if (this.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Stun<EnemyAI>(duration));
        }
        yield return null;

    }



    private IEnumerator Freeze<T>(float duration) where T : Behaviour
    {
        T component = this.GetComponent<T>();
        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();

        if (component != null)
        {
            if(navAgent != null)
            {
                navAgent.isStopped = true;
                navAgent.velocity = Vector3.zero;
            }

            component.enabled = false;

            yield return new WaitForSeconds(duration);

            component.enabled = true;

            if (navAgent != null)
            {
                navAgent.isStopped = false;
            }
        }
    }
    private IEnumerator Stun<T>(float duration) where T : Behaviour
    {
        T component = this.GetComponent<T>();
        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
        
        float elapsedTime = 0;


        if (component != null)
        {
            while (elapsedTime < duration)
            {
                if (navAgent != null)
                {
                    navAgent.isStopped = true;
                    navAgent.velocity = Vector3.zero;
                }

                component.enabled = false;

                yield return new WaitForSeconds(duration / 10);
                elapsedTime += duration / 10;

                component.enabled = true;

                if (navAgent != null)
                {
                    navAgent.isStopped = false;
                }

                yield return new WaitForSeconds(duration / 5);

                elapsedTime += duration / 5;
            }

            component.enabled = true;

            if (navAgent != null)
            {
                navAgent.isStopped = false;
            }
        }
    }

}

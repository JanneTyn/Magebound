using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StatusManager : MonoBehaviour
{
    public void Activate(int statusID, float duration)
    {
        switch (statusID)
        {
            //Burning
            case 01:
                StartCoroutine(ApplyBurn(duration));
                break;
            //Freeze
            case 02:
                ApplyFreeze(duration);
                break;
            //Chill
            case 03:
                ApplyChill(duration);
                break;
            //Stun
            case 04:
                ApplyStun(duration);
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
                StartCoroutine(ApplyBurn(duration, damage));
                break;
            //Freeze
            case 02:
                ApplyFreeze(duration);
                break;
            //Chill
            case 03:
                ApplyChill(duration);
                break;
            //Stun
            case 04:
                ApplyStun(duration);
                break;

            default:
                Debug.Log("Invalid Status");
                break;
        }
    }

    private IEnumerator ApplyBurn(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            this.gameObject.GetComponent<DamageSystem>().CalculateDamage(10, 1);
            
            yield return null;
        }
    }

    private IEnumerator ApplyBurn(float duration, float damage)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            this.gameObject.GetComponent<DamageSystem>().CalculateDamage(damage, 1);

            yield return null;
        }
    }

    private void ApplyFreeze(float duration)
    {

    }

    private IEnumerator ApplyChill(float duration)
    {
        float elapsedTime = 0;
        if (this.gameObject.CompareTag("Player"))
        {
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                this.gameObject.GetComponent<PlayerMovement>().movementSpeed /= 2;

                yield return null;
            }

            this.gameObject.GetComponent<PlayerMovement>().movementSpeed *= 2;
        }
        else if (this.gameObject.CompareTag("Enemy"))
        {
            while ( elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                this.gameObject.GetComponent<NavMeshAgent>().speed /= 2;

                yield return null;
            }

            this.gameObject.GetComponent<NavMeshAgent>().speed *= 2;
        }

    }
    private void ApplyStun(float duration)
    {

    }

    
}

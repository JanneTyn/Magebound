using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public void Activate(int statusID)
    {
        switch (statusID)
        {
            //Burning
            case 01:
                ApplyBurn();
                break;
            //Freeze
            case 02:
                ApplyFreeze();
                break;
            //Chill
            case 03:
                ApplyChill();
                break;
            //Stun
            case 04:
                ApplyStun();
                break;

            default:
                Debug.Log("Invalid Status");
                break;
        }
    }

    private void ApplyBurn()
    {

    }
    private void ApplyFreeze()
    {

    }
    private void ApplyChill()
    {

    }
    private void ApplyStun()
    {

    }

}

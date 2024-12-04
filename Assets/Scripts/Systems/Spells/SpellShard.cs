using UnityEngine;

public class SpellShard : MonoBehaviour
{

    public GameObject CrystalShardFirePrefab;
    public GameObject CrystalShardIcePrefab;
    public GameObject CrystalShardThunderPrefab;


    public void InitializeShard(Vector3 playerLocation, Vector3 targetLocation, int element)
    {
        switch (element)
        {
            case 1:
                GameObject shard = Instantiate(CrystalShardFirePrefab, targetLocation, Quaternion.identity);
                break;
            case 2:
                GameObject iceshard = Instantiate(CrystalShardIcePrefab, targetLocation, Quaternion.identity);
                break;
            case 3:
                GameObject thundershard = Instantiate(CrystalShardThunderPrefab, targetLocation, Quaternion.identity);
                break;
        }
    }
}

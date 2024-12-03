using UnityEngine;

public class SpellShard : MonoBehaviour
{

    public GameObject CrystalShardFirePrefab;
    public GameObject CrystalShardIcePrefab;
    public GameObject CrystalShardThunderPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeShard(Vector3 playerLocation, Vector3 targetLocation, int element)
    {
        switch (element)
        {
            case 1:
                GameObject shard = Instantiate(CrystalShardFirePrefab, targetLocation, Quaternion.identity);
                break;
            case 2:
                GameObject iceshard = Instantiate(CrystalShardIcePrefab, playerLocation, Quaternion.identity);
                iceshard.GetComponent<SpellEffect_Explosion_Ice>().SetProjectileDirection(targetLocation, playerLocation);
                break;
            case 3:
                GameObject thundershard = Instantiate(CrystalShardThunderPrefab, playerLocation, Quaternion.identity);
                thundershard.GetComponent<SpellEffect_Explosion_Thunder>().SetProjectileDirection(targetLocation, playerLocation);
                break;
        }
    }
}

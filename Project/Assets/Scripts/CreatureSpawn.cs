using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawn : MonoBehaviour
{
    [Header("Animals instances")]
    [SerializeField] private GameObject _rabbitInstance;

    [SerializeField] private GameObject _wolfInstance;
    
    [SerializeField] private GameObject _deerGroupInstance;

    [Header("Animals count in scene")]
    [SerializeField] private int _rabbitCount;

    [SerializeField] private int _wolfCount;

    [SerializeField] private int _deerGroupCount;

    private void Awake()
    {
        SpawnCreature(_rabbitInstance, _rabbitCount);
        SpawnCreature(_wolfInstance, _wolfCount);
        SpawnCreature(_deerGroupInstance, _deerGroupCount);
    }

    private void SpawnCreature(GameObject creature, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = Random.insideUnitSphere * 12;

            Instantiate(creature, spawnPosition, Quaternion.identity);
        }
    }
}

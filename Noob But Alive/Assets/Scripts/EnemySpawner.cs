using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1f, 2f);
    }

    void Spawn()
    {
        Vector3 pos = new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f));
        GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
        go.GetComponent<EnemyChase>().target = player;
    }
}

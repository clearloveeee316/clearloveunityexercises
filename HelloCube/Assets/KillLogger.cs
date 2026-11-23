using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogger : MonoBehaviour
{
    int killCount = 0;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("场景里缺少 GameManager，KillLogger 无法订阅事件。");
            return;
        }

        GameManager.Instance.OnEnemyDied += LogDeath;      // 订阅
        GameManager.Instance.OnEnemyDied += CountKill;     // 第二个订阅者——多播！
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyDied -= LogDeath;
            GameManager.Instance.OnEnemyDied -= CountKill;
        }
    }

    void LogDeath(string name) => Debug.Log("UI播报：敌人 " + name + " 阵亡，+10分");
    void CountKill(string name) { killCount++; Debug.Log("击杀计数：" + killCount); }
}
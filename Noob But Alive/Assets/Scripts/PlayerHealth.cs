using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 100;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log($"当前血量：{hp}"); 
        if (hp <= 0)
        {
            Debug.Log("你死了");
            Time.timeScale = 0f;
        }
    }
}

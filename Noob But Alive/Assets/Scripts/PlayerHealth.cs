using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 100;
    public int maxHp = 100;

    public delegate void HealthChangedHandler(int currentHp, int maxHp);
    public event HealthChangedHandler OnHealthChanged;

    public delegate void PlayerDiedHandler();
    public event PlayerDiedHandler OnPlayerDied;
    public void TakeDamage(int dmg)
    {
        if (hp <= 0)
            return;
        hp -= dmg;
        if (hp < 0)
            hp = 0;
        OnHealthChanged?.Invoke(hp, maxHp);
        Debug.Log($"当前血量：{hp}"); 
        if (hp <= 0)
        {
            Debug.Log("你死了");
            Time.timeScale = 0f;
            OnPlayerDied?.Invoke();
        }
    }
}

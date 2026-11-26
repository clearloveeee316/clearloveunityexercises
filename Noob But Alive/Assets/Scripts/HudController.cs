using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudController : MonoBehaviour
{
    public PlayerHealth player;
    public TextMeshProUGUI hpText;

    void Start()
    {
        if (player == null || hpText == null)
        {
            Debug.LogError("HudController 有槽没拖！");
            return;
        }

        player.OnHealthChanged += UpdateHud;
        UpdateHud(player.hp, player.maxHp);
    }

    void UpdateHud(int currentHp, int maxHp)
    {
        hpText.text = $"HP: {currentHp}/{maxHp}";
    }

    void OnDestroy()
    {
        player.OnHealthChanged -= UpdateHud;
    }
}

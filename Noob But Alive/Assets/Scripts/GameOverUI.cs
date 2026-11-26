using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public PlayerHealth player;      // 数据源：谁来报死
    public GameObject gameOverPanel; // 结果：谁被点亮
    public TextMeshProUGUI statsText;
    public GameStats stats;          // 新增：全局计数单例

    void Start()
    {
        if (player == null || gameOverPanel == null || statsText == null || stats == null)
        {
            Debug.LogError("GameOverUI 有槽没拖！");
            return;
        }

        player.OnPlayerDied += ShowGameOver;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        statsText.text = $"存活 {Time.timeSinceLevelLoad:F1} 秒\n消灭 {stats.kills}";
    }

    void OnDestroy()
    {
        player.OnPlayerDied -= ShowGameOver;
    }
    void Update()
    {
        if (!gameOverPanel.activeSelf) return;   // 活着的时候不管 R
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;                 // ← 这行是命根子
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCount;
    [SerializeField] private TextMeshProUGUI enemyCount;

    private int CoinCount = 0;
    private int EnemyCount = 0;
    public bool isGameOver = false;

    public void coinCountUP()
    {
        CoinCount++;
        coinCount.text = "Coin : " + (int)CoinCount;
    }

    public void enemyCountUP()
    {
        EnemyCount++;
        enemyCount.text = "Kill Enemy : " + (int)EnemyCount;
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Text coin;
    [SerializeField] Text score;
    [SerializeField] Text bestScore;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Best", 0) < PlayerPrefs.GetInt("Score", 0))
        {
            PlayerPrefs.SetInt("Best", PlayerPrefs.GetInt("Score", 0));
        }

        coin.text = System.Convert.ToString(PlayerPrefs.GetInt("Coins", 0));
        bestScore.text = "Best Score: " + System.Convert.ToString(PlayerPrefs.GetInt("Best", 0));
        score.text = "You Score: " + System.Convert.ToString(PlayerPrefs.GetInt("Score", 0));
    }
}

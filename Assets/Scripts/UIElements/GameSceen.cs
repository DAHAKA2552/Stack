using UnityEngine;
using UnityEngine.UI;

public class GameSceen : MonoBehaviour
{
    [SerializeField] Text coin;
    [SerializeField] Text score;

    private void OnEnable()
    {
        coin.text = System.Convert.ToString(PlayerPrefs.GetInt("Coins", 0));
        score.text = System.Convert.ToString(PlayerPrefs.GetInt("Score", 0));
    }

    private void Update()
    {
        score.text = System.Convert.ToString(PlayerPrefs.GetInt("Score", 0));
    }
}

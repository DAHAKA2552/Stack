using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] Text coin;
    [SerializeField] Button score;
    [SerializeField] Button custom;
    [SerializeField] Button sounds;

    private void OnEnable()
    {
        coin.text = System.Convert.ToString(PlayerPrefs.GetInt("Coins", 0));

    }
}

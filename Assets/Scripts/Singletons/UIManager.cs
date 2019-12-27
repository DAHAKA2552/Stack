using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Fields
    [SerializeField] private Component[] screens;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Canvas backGround;

    private bool isOnGUI;

    private Vector3 cameraStartPosiiton;
    #endregion



    #region Unity
    void Awake()
    {
        InitializationComponents(screens);
        cameraStartPosiiton = Camera.main.transform.position;
    }

    void Start()
    {
        StartGameScreen();
        canvas.worldCamera = Camera.main;
        backGround.worldCamera = Camera.main;
        
    }
    #endregion



    #region Private Method
    void InitializationComponents(Component[] pref)
    {
        for (int i = 0; i < pref.Length; i++)
        {
            if (pref[i] != null)
            {
                screens[i] = CreateComponents(pref[i]);
            }
        }
    }

    Component CreateComponents(Component pref)
    {
        var obj = Instantiate(pref, canvas.transform);
        obj.name = pref.name;
        return obj;
    }

    void ShowSceen(int j)
    {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].gameObject.SetActive(false);
        }
        screens[j].gameObject.SetActive(true);
    }
    #endregion



    #region Public Method
    public void StartGameScreen()
    {
        ShowSceen(0);
        Debug.Log("Start");
        Camera.main.transform.position = cameraStartPosiiton;
    }

    public void GameScreen()
    {
        ShowSceen(1);
        Debug.Log("Game");
    }

    public void GameOverScreen()
    {
        ShowSceen(2);
        Debug.Log("GameOver");
    }
    #endregion
}

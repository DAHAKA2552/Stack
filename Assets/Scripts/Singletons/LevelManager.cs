using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    #region Fields
    [SerializeField] private GameObject playerPref;
    [SerializeField] private GameObject platformsPref;

    GameObject lastBlock;
    GameObject player;
    GameObject leftPart;
    GameObject platforms;
    GameObject lastChangePlatform;

    Vector3 startPosition;
    Vector3 endPosition;
    Stack<GameObject> stack = new Stack<GameObject>();

    private float speed;
    private bool isMoving;
    private bool isGameOver;

    private int score;
    private int perfectCount;

    #endregion


    #region Unity
    void Start()
    {
        CreateStartPlatform();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (true) 
            {
                if(isGameOver)
                {
                    DestroyPlatform();

                    GameManager.Instance.NewGame();
                    UIManager.Instance.StartGameScreen();
                }
                else
                {
                    if (isMoving)
                    {
                        PlayerController.Instance.StopMoving();

                        if (ComparisonPosition())
                        {
                            perfectCount++;
                            if(perfectCount % 4 == 0)
                            {
                                perfectCount = 0;
                                if(stack.Count !=0)
                                {
                                    GameObject lastTransform = stack.Pop();
                                    player.transform.localScale = lastTransform.transform.localScale;
                                    player.transform.position = new Vector3(lastTransform.transform.position.x, player.transform.position.y, lastTransform.transform.position.z);
                                    lastBlock = player;
                                }
                            }
                            ChechScore();
                            NewPlatform();
                        }
                        else
                        {
                            perfectCount = 0;
                            stack.Push(lastBlock);

                            leftPart = Instantiate(player, platforms.transform);
                            leftPart.name = "leftPart";
                            CreatPart(leftPart);
                            
                            if (!isGameOver)
                            {
                                NewPlatform();
                                ChechScore();
                            }
                            else
                            {
                                GameOver();
                                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - score / 10,
                                                                             Camera.main.transform.position.y - score / 50,
                                                                             Camera.main.transform.position.z - score / 10);
                            }
                        }
                    }
                    else
                    {
                        UIManager.Instance.GameScreen();
                        PlayerPrefs.SetInt("Score", 0);
                        NewPlatform(); 
                    }
                }
            }
        }
    }
    #endregion


    #region Private Function
    void ChechScore()
    {
        PlayerPrefs.SetInt("Score", ++score);

        if (score > 4)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.MoveCamera());
        }
        if (score % 10 == 0)
        {
            int coin = PlayerPrefs.GetInt("Coins", 0);
            PlayerPrefs.SetInt("Coins", ++coin);
            speed += 0.2f;
        }
    }

    void NewPlatform()
    {
        player = Instantiate(player, platforms.transform);
        player.name = "Block";
        RandomPosition();
        player.transform.position = startPosition;

        PlayerController.Instance.StartCoroutine(PlayerController.Instance.MovePlatform(startPosition, endPosition, player, speed));
        isMoving = true;
    }

    void RandomPosition()
    {
        float randomVector = UnityEngine.Random.Range(0, 100);
        if(randomVector <= 25)
        {
            startPosition = new Vector3(1.25f, player.transform.position.y + player.transform.localScale.y, player.transform.position.z);
            endPosition = new Vector3(-1.25f, player.transform.position.y + player.transform.localScale.y, player.transform.position.z);
        }
        if(randomVector > 25 && randomVector <= 50)
        {
            startPosition = new Vector3(-1.25f, player.transform.position.y + player.transform.localScale.y, player.transform.position.z);
            endPosition = new Vector3(1.25f, player.transform.position.y + player.transform.localScale.y, player.transform.position.z);
        }
        if(randomVector > 50 && randomVector <= 75)
        {
            startPosition = new Vector3(player.transform.position.x, player.transform.position.y + player.transform.localScale.y, 1.25f);
            endPosition = new Vector3(player.transform.position.x, player.transform.position.y + player.transform.localScale.y, -1.25f);
        }
        if(randomVector > 75)
        {
            startPosition = new Vector3(player.transform.position.x, player.transform.position.y + player.transform.localScale.y, -1.25f);
            endPosition = new Vector3(player.transform.position.x, player.transform.position.y + player.transform.localScale.y, 1.25f);
        }
    }

    bool ComparisonPosition()
    {
        float offset = 0.02f;

        float playerOffsetX = player.transform.position.x - lastBlock.transform.position.x;
        float playerOffsetZ = player.transform.position.z - lastBlock.transform.position.z;

        if (Math.Abs(playerOffsetX) <= offset && Math.Abs(playerOffsetZ) <= offset)
        {
            player.transform.position = lastBlock.transform.position + new Vector3(0.0f, 0.25f, 0.0f);
            lastBlock = player;
            return true;
        }
        else
        {
            return false;
        }
    }

    void CreatPart(GameObject part)
    {
        Vector3 offsetPosition = player.transform.position - lastBlock.transform.position;
        float offset = 0.001f;

        if (player.transform.localScale.x - Math.Abs(offsetPosition.x) < 0 || player.transform.localScale.z - Math.Abs(offsetPosition.z) < 0)
        {
            isGameOver = true;
            Destroy(player);
            part.transform.eulerAngles = Vector3.zero;
            part.AddComponent<Rigidbody>();
        }
        else
        {
            player.transform.localScale = new Vector3(player.transform.localScale.x - Math.Abs(offsetPosition.x),
                                                      player.transform.localScale.y,
                                                      player.transform.localScale.z - Math.Abs(offsetPosition.z));

            if (offsetPosition.x != 0)
            {
                if (offsetPosition.x < 0)
                {
                    player.transform.position = new Vector3(player.transform.position.x + Mathf.Abs(offsetPosition.x) / 2,
                                                            player.transform.position.y,
                                                            player.transform.position.z);

                    part.transform.localScale = new Vector3(lastBlock.transform.localScale.x - player.transform.localScale.x,
                              player.transform.localScale.y,
                              player.transform.localScale.z);
                    part.transform.position = new Vector3(player.transform.position.x - ((player.transform.localScale.x + part.transform.localScale.x) / 2.0f) - offset,
                                                  player.transform.position.y,
                                                  player.transform.position.z);
                }
                if (offsetPosition.x > 0)
                {
                    player.transform.position = new Vector3(player.transform.position.x - Mathf.Abs(offsetPosition.x) / 2,
                                                            player.transform.position.y,
                                                            player.transform.position.z);

                    part.transform.localScale = new Vector3(lastBlock.transform.localScale.x - player.transform.localScale.x,
                              player.transform.localScale.y,
                              player.transform.localScale.z);
                    part.transform.position = new Vector3(player.transform.position.x + ((player.transform.localScale.x + part.transform.localScale.x) / 2.0f) + offset,
                                                  player.transform.position.y,
                                                  player.transform.position.z);
                }
            }
            if (offsetPosition.z != 0)
            {
                if (offsetPosition.z < 0)
                {
                    player.transform.position = new Vector3(player.transform.position.x,
                                                            player.transform.position.y,
                                                            player.transform.position.z + Mathf.Abs(offsetPosition.z) / 2);

                    part.transform.localScale = new Vector3(player.transform.localScale.x,
                              player.transform.localScale.y,
                              lastBlock.transform.localScale.z - player.transform.localScale.z);
                    part.transform.position = new Vector3(player.transform.position.x,
                                                  player.transform.position.y,
                                                  player.transform.position.z - ((player.transform.localScale.z + part.transform.localScale.z) / 2.0f) - offset);
                }
                if (offsetPosition.z > 0)
                {
                    player.transform.position = new Vector3(player.transform.position.x,
                                                            player.transform.position.y,
                                                            player.transform.position.z - Mathf.Abs(offsetPosition.z) / 2);

                    part.transform.localScale = new Vector3(player.transform.localScale.x,
                              player.transform.localScale.y,
                              lastBlock.transform.localScale.z - player.transform.localScale.z);
                    part.transform.position = new Vector3(player.transform.position.x,
                                                  player.transform.position.y,
                                                  player.transform.position.z + ((player.transform.localScale.z + part.transform.localScale.z) / 2.0f) + offset);
                }
            }
            lastBlock = player;
            part.transform.eulerAngles = Vector3.zero;
            part.AddComponent<Rigidbody>();
        }
    }

    void GameOver()
    {
        UIManager.Instance.GameOverScreen();
    }

    void DestroyPlatform()
    {
        Destroy(platforms);
    }

    #endregion 


    public void CreateStartPlatform()
    {
        platforms = Instantiate(platformsPref, transform);
        platforms.name = platforms.name;

        player = Instantiate(playerPref, platforms.transform);
        player.name = "Start";

        isMoving = false;
        isGameOver = false;
        speed = 1;
        score = 0;
        perfectCount = 0;

        lastBlock = player;
    }
}

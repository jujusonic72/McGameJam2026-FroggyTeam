using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<TargetBehaviour> targets = new List<TargetBehaviour>();

    [SerializeField]
    private BlackFadeBehaviour fade;

    [SerializeField]
    private string nextLevel;

    [SerializeField]
    public static GameObject canvaMenu;

    [SerializeField]
    private GameObject loseScreen;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private TMP_Text DiceRollText;

    [SerializeField]
    private TMP_Text PrizeWonText;

    [SerializeField]
    private bulletcontroller bulletcontroller;

    private bool hasWon;
    private bool rolled = false;

    private int diceRoll;

    private bool _hasFinishedReset = false;

    public static GameManager instance;

    public Color bulletColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != this && instance != null) 
        {
            Debug.Log("Destroying duplicate GameManager");
            Destroy(this);
        }
        else if (instance == null || instance == this)
        {
            Debug.Log("Either Setting the first game manager or making sure it is subscribed to SceneLoaded");
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnLoad;
        }

        
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) || SceneManager.GetActiveScene().name.Contains("Gym"))
        //{
        //    targets = FindObjectsByType<TargetBehaviour>(FindObjectsSortMode.None).ToList();

        //    foreach (var target in targets)
        //    {
        //        print(target.gameObject.name);
        //    }
        //    DiceRollText.text = "Rolling Dice...";
        //    PrizeWonText.text = "";
        //    rolled = false;

        //    _hasFinishedReset = true;
        //}

    }
    private void OnLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (fade == null)
        {
            fade = GameObject.Find("Canvas").GetComponentInChildren<BlackFadeBehaviour>();
        }
        if (winScreen == null)
        {
            winScreen = GameObject.Find("Canvas").transform.Find("WinScreen").gameObject;
            winScreen.transform.Find("Continue").GetComponent<Button>().onClick.AddListener(OnPressNext);
        }
        if(loseScreen == null)
        {
            loseScreen = GameObject.Find("Canvas").transform.Find("LoseScreen").gameObject;
            loseScreen.transform.Find("Retru").GetComponent<Button>().onClick.AddListener(OnPressRetry);
        }

        if (bulletcontroller == null)
        {
            bulletcontroller = GameObject.Find("Bullet").GetComponent<bulletcontroller>();
        }
        targets = FindObjectsByType<TargetBehaviour>(FindObjectsSortMode.None).ToList();

        foreach (var target in targets)
        {
            print(target.gameObject.name);
        }
        hasWon = false;
        if (DiceRollText != null) DiceRollText.text = "Rolling Dice...";
        else Debug.Log("DiceRollText is null");

        if (PrizeWonText != null) PrizeWonText.text = "";
        else Debug.Log("PrizeWonText is null");

        rolled = false;

        _hasFinishedReset = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(targets.Count <= 0 && !hasWon)
        if(targets.Count <= 0 && _hasFinishedReset)
        {
            OnWin();
            hasWon=true;
        }

        if(Input.GetKeyDown(KeyCode.R)) OnPressRetry();
    }

    void OnWin()
    {
        winScreen.SetActive(true);
        if (!rolled)
        {
            StartCoroutine(RollDice());
        }
        bulletcontroller.StopBullet();
    }

    public void OnLose()
    {
        loseScreen.SetActive(true);
    }

    public void OnPressRetry()
    {
        StartCoroutine(fade.LevelEndFade(SceneManager.GetActiveScene().name));
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void OnPressNext()
    {
        StartCoroutine(fade.LevelEndFade(nextLevel));
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        _hasFinishedReset = false;
        SceneManager.LoadScene(nextLevel);
    }

    public Color GetBulletColor()
    {
        return bulletColor;
    }

    IEnumerator RollDice()
    {
        yield return new WaitForSeconds(3.0f);
        diceRoll = Random.Range(1, 7);
        DiceRollText.text = "You Rolled " + diceRoll;
        yield return new WaitForSeconds(1.0f);
        rolled = true;

        yield return new WaitForSeconds(1.0f);

        switch (diceRoll)
        {
            case 1:
                PrizeWonText.text = "You won Item 1";
                bulletColor = Color.red;
                Debug.Log("New Color: " + bulletColor);
                break;

            case 2:
                PrizeWonText.text = "You won Item 2";
                bulletColor = Color.cyan;
                Debug.Log("New color: " + bulletColor);
                break;

            case 3:
                PrizeWonText.text = "You won Item 3";
                bulletColor = Color.green;
                Debug.Log("New color: " + bulletColor);
                break;

            case 4:
                PrizeWonText.text = "You won Item 4";
                bulletColor = Color.blue;
                Debug.Log("New color: " + bulletColor);
                break;

            case 5:
                PrizeWonText.text = "You won Item 5";
                bulletColor = Color.magenta;
                Debug.Log("New color: " + bulletColor);
                break;

            case 6:
                PrizeWonText.text = "You won Item 6";
                bulletColor = Color.yellow;
                Debug.Log("New color: " + bulletColor);
                break;

            default:
                PrizeWonText.text = "Roll Error";
                break;

        }
    }
}
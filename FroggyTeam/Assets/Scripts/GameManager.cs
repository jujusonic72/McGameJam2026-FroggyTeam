using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
//using UnityEngine.Windows;

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
    private GameObject pauseScreen;

    [SerializeField]
    private TMP_Text DiceRollText;

    [SerializeField]
    private TMP_Text PrizeWonText;

    public bulletcontroller bulletcontroller;


    private bool hasWon;
    private bool hasLost;
    private bool rolled = false;

    private int diceRoll;

    private bool _hasFinishedReset = false;

    public static GameManager instance;

    public Color bulletColor;

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        
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
    private void OnEnable()
    {
    }
    public IEnumerator InitLevel()
    {
        Debug.Log("Initializing Level in GameManager");
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Enabling GameManager");
        if (instance != this && instance != null)
        {
            Debug.Log("Destroying duplicate GameManager");
            Destroy(this.gameObject);
            yield break;
        }
        else if (instance == null || instance == this)
        {
            Debug.Log("Either Setting the first game manager or making sure it is subscribed to SceneLoaded" + _hasFinishedReset);
            instance = this;
            DontDestroyOnLoad(instance);
            if(_hasFinishedReset)
            {
                yield break;
            }
        }
        StartCoroutine(fade.LevelStartFade());
        bulletcontroller = GameObject.Find("Bullet").GetComponent<bulletcontroller>();
        bulletcontroller.BulletTargetCollision.AddListener(CheckTargets);
        bulletcontroller.jump.performed += MenuControls;
        bulletcontroller.retry.performed += OnRetry;
        bulletcontroller.inputs.Player.Pause.performed += OnPressPause;
        targets = FindObjectsByType<TargetBehaviour>(FindObjectsSortMode.None).ToList();
        foreach (var target in targets)
        {
            print(target.gameObject.name);
        }
        hasWon = false;
        hasLost = false;
        if (DiceRollText != null) DiceRollText.text = "Rolling Dice...";
        else Debug.Log("DiceRollText is null");

        if (PrizeWonText != null) PrizeWonText.text = "";
        else Debug.Log("PrizeWonText is null");

        rolled = false;

        _hasFinishedReset = true;
    }
    private void OnLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("GameManager detected scene load: " + scene.name);
        StartCoroutine(InitLevel());
    }
    private void MenuControls(InputAction.CallbackContext context)
    {
        if (hasWon)
        {
            OnPressNext();
        }
        else if (hasLost)
        {
            OnPressRetry();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Gamepad.current.startButton.IsPressed())
        {
            OnPauseResume();
        }

    }

    private void OnRetry(InputAction.CallbackContext context)
    {
        OnPressRetry();
    }

    private void OnPressPause(InputAction.CallbackContext context)
    {
        OnPauseResume();
    }

    private void CheckTargets()
    {
        if (targets.Count <= 0 && !hasWon)
        {
            OnWin();
            hasWon = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        

        
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
        hasLost = true;
    }

    void OnPauseResume()
    {
        if (isPaused)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }

    }

    public void OnPressRetry()
    {
        bulletcontroller.jump.performed -= MenuControls;
        bulletcontroller.retry.performed -= OnRetry;
        
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        _hasFinishedReset = false;
        StartCoroutine(fade.LevelEndFade(SceneManager.GetActiveScene().name));
    }

    public void OnPressNext()
    {
        bulletcontroller.jump.performed -= MenuControls;
        bulletcontroller.retry.performed -= OnRetry;
        
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
                break;

            case 2:
                PrizeWonText.text = "You won Item 2";
                bulletColor = Color.cyan;
                break;

            case 3:
                PrizeWonText.text = "You won Item 3";
                bulletColor = Color.green;
                break;

            case 4:
                PrizeWonText.text = "You won Item 4";
                bulletColor = Color.blue;
                break;

            case 5:
                PrizeWonText.text = "You won Item 5";
                bulletColor = Color.magenta;
                break;

            case 6:
                PrizeWonText.text = "You won Item 6";
                bulletColor = Color.yellow;
                break;

            default:
                PrizeWonText.text = "Roll Error";
                break;

        }
    }
}
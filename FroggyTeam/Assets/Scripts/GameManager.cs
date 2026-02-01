using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
    private GameObject skinSelection;

    [SerializeField]
    private GameObject pauseScreen;

    [SerializeField]
    private TMP_Text DiceRollText;

    [SerializeField]
    private TMP_Text PrizeWonText;

    [SerializeField]
    private List<GameObject> skinPanels;

    [SerializeField]
    private List<SkinObject> skins;

    [SerializeField]
    private Sprite lockedSkinIcon;

    public bulletcontroller bulletcontroller;

    [SerializeField]
    private int currentSkinIndex = 0;


    private bool hasWon;
    private bool hasLost;
    private bool rolled = false;

    private int diceRoll;

    private bool _hasFinishedReset = false;

    public static GameManager instance;

    public GameObject bulletRender;

    public GameObject CurrentBullet;

    private bool isPaused = false;

    [SerializeField]
    private AudioClip bgMusic;
    private bool isSkinSelectOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInChildren<SoundPlayer>().PlaySound(bgMusic, true);



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
        bulletcontroller.inputs.Player.SkinSelection.performed += context =>
        {
            if (!isSkinSelectOpen && (hasWon || hasLost || isPaused))
            {
                skinSelection.SetActive(true);
                skinSelection.GetComponentInChildren<UnityEngine.UI.Button>().Select();
                isSkinSelectOpen = true;
            }
        };
        RefreshBulletSkins();
        SelectSkin(currentSkinIndex);
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
        if (hasWon && rolled && !isSkinSelectOpen)
        {
            OnPressNext();
        }
        else if (hasLost && !isSkinSelectOpen)
        {
            OnPressRetry();
        }
        else if (isSkinSelectOpen)
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
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
        Debug.Log("Checking Targets" + targets.Count);
        if (targets.Count <= 0 && !hasWon && _hasFinishedReset)
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

    public void SelectSkin(int skinIndex)
    {
        currentSkinIndex = skinIndex;
        if (skins[skinIndex].isUnlocked)
        {
            //bulletColor = skins[skinIndex].skinMaterial.color;
            bulletRender = GameObject.Find("Bullet").transform.Find("BulletRender").gameObject;
            if (CurrentBullet != null)
            {
                Destroy(CurrentBullet);
            }
            CurrentBullet = Instantiate(skins[skinIndex].skinMesh, bulletRender.transform.position, bulletRender.transform.rotation, bulletRender.transform.parent);
            CurrentBullet.transform.localScale = skins[skinIndex].skinScale * Vector3.one;

            //bulletRender.GetComponent<Renderer>().material = skins[0].skinMaterial;
            skinSelection.SetActive(false);
            if (hasWon && _hasFinishedReset)
            {
                winScreen.SetActive(true);
                winScreen.GetComponentInChildren<UnityEngine.UI.Button>().Select();
            }
            else if (hasLost && _hasFinishedReset)
            {
                loseScreen.SetActive(true);
                loseScreen.GetComponentInChildren<UnityEngine.UI.Button>().Select();
            }
            else if (isPaused && _hasFinishedReset)
            {
                pauseScreen.SetActive(true);
                pauseScreen.GetComponentInChildren<UnityEngine.UI.Button>().Select();
            }
            isSkinSelectOpen = false;
        }
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
        StartCoroutine(fade.LevelEndFade(SceneManager.GetActiveScene().buildIndex));
    }

    public void OnPressNext()
    {
        bulletcontroller.jump.performed -= MenuControls;
        bulletcontroller.retry.performed -= OnRetry;
        print(SceneManager.GetActiveScene().buildIndex);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        print(SceneManager.GetSceneByBuildIndex(nextSceneIndex).name);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        _hasFinishedReset = false;
        StartCoroutine(fade.LevelEndFade(nextSceneIndex));
    }
    public void RefreshBulletSkins()
    {
        foreach (GameObject panel in skinPanels)
        {
            int index = skinPanels.IndexOf(panel);
            if (skins[index].isUnlocked)
            {
                panel.transform.GetComponentInChildren<UnityEngine.UI.Image>().sprite = skins[index].skinIcon;
                panel.GetComponentInChildren<TextMeshProUGUI>().text = skins[index].skinName;
            }
            else
            {
                panel.transform.GetComponentInChildren<UnityEngine.UI.Image>().sprite = lockedSkinIcon;
                panel.GetComponentInChildren<TextMeshProUGUI>().text = "Locked";
            }
        }
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
                PrizeWonText.text = "You won the Classic Bullet Skin";
                skins[0].isUnlocked = true;
                break;

            case 2:
                PrizeWonText.text = "You won the Bazooka Bullet Skin";
                skins[1].isUnlocked = true;
                break;

            case 3:
                PrizeWonText.text = "You won the Horse Bullet Skin";
                skins[2].isUnlocked = true;
                break;

            case 4:
                PrizeWonText.text = "You won the Nerf Bullet Skin";
                skins[3].isUnlocked = true;
                break;

            case 5:
                PrizeWonText.text = "You won the Roblox Bullet Skin";
                skins[4].isUnlocked = true;
                break;

            case 6:
                PrizeWonText.text = "You won Shotgun Bullet Skin";
                skins[5].isUnlocked = true;
                break;

            default:
                PrizeWonText.text = "Roll Error";
                break;

        }
        RefreshBulletSkins();
    }
}
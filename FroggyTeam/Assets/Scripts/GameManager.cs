using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<TargetBehaviour> targets = new List<TargetBehaviour>();

    [SerializeField]
    private string nextLevel;

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

    private bool rolled = false;

    private int diceRoll;

    public static Color bulletColor { set; get; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targets = FindObjectsByType<TargetBehaviour>(FindObjectsSortMode.None).ToList();

        foreach(var target in targets)
        {
            print(target.gameObject.name);
        }
        DiceRollText.text = "Rolling Dice...";
        PrizeWonText.text = "";
        rolled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(targets.Count <= 0)
        {
            OnWin();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPressNext()
    {
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
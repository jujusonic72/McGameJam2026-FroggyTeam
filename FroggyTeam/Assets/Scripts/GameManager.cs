using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<TargetBehaviour> targets = new List<TargetBehaviour>();

    [SerializeField]
    private BlackFadeBehaviour fade;

    [SerializeField]
    private string nextLevel;

    [SerializeField]
    private GameObject loseScreen;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private bulletcontroller bulletcontroller;

    private bool hasWon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targets = FindObjectsByType<TargetBehaviour>(FindObjectsSortMode.None).ToList();
        foreach(var target in targets)
        {
            print(target.gameObject.name);
        }
        hasWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(targets.Count <= 0 && !hasWon)
        {
            OnWin();
            hasWon=true;
        }

        if(Input.GetKeyDown(KeyCode.R)) OnPressRetry();
    }

    void OnWin()
    {
        winScreen.SetActive(true);
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
    }
}

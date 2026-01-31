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
    private string nextLevel;

    [SerializeField]
    private GameObject loseScreen;

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private bulletcontroller bulletcontroller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targets = FindObjectsByType<TargetBehaviour>(FindObjectsSortMode.None).ToList();
        foreach(var target in targets)
        {
            print(target.gameObject.name);
        }
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
}

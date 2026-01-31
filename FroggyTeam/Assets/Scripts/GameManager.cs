using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<TargetBehaviour> targets = new List<TargetBehaviour>();

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
    }

    void OnWin()
    {
        print("Win");
    }
}

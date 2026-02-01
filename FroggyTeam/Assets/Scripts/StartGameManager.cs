using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] GMs = GameObject.FindGameObjectsWithTag("GameManager");
        foreach (var item in GMs)
        {
            item.GetComponent<GameManager>().StartCoroutine("InitLevel");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

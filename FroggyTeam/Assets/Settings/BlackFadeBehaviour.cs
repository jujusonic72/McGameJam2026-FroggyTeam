using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackFadeBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image bg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Starting to unfade");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LevelEndFade(int nextBuildIndex)
    {
        print("Fading");
        while(bg.color.a < 1)
        {
            float oldAlpha = bg.color.a;
            bg.color = new Color(0, 0, 0, oldAlpha + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Done Fading");
        SceneManager.LoadScene(nextBuildIndex);
    }

    public IEnumerator LevelStartFade()
    {
        Debug.Log("unFading");
        while (bg.color.a > 0)
        {
            float oldAlpha = bg.color.a;
            bg.color = new Color(0, 0, 0, oldAlpha - Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Done unFading");
    }
}

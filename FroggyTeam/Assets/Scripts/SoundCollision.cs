using UnityEngine;

public class SoundCollision : MonoBehaviour
{
    public AudioClip soundClip;
    GameManager gameManager;
    SoundPlayer soundPlayer;
    public bool isBullet = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundPlayer = gameManager.transform.Find("SoundManager").GetComponent<SoundPlayer>();
        if (soundClip == null)
        {
            soundClip = gameManager.skins[gameManager.currentSkinIndex].skinSoundEffect;
            isBullet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Tag: " + collision.gameObject.tag);
        if(!isBullet && collision.gameObject.tag == "Player")
        {
            // Debug.Log("Sonnnnnn");
            soundPlayer.PlaySound(soundClip, false, true, 0.5f);
        }
        else if (isBullet)
        {
            soundPlayer.PlaySound(soundClip, false, true, 0.5f);
        }
        
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class BallesSon : MonoBehaviour
{
    GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    SoundPlayer sPlayer;
    AudioClip clip;
    private void Start()
    {
       sPlayer = GM.gameObject.transform.Find("SoundManager").GetComponent<SoundPlayer>();
       clip = GM.skins[GM.currentSkinIndex].skinSoundEffect;
       sPlayer.PlaySound(clip, false, false, 0.5f);
    }
     
    
}

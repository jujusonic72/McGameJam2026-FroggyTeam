using UnityEngine;

[CreateAssetMenu(fileName = "SkinObject", menuName = "Scriptable Objects/SkinObject")]
public class SkinObject : ScriptableObject
{
    public string skinName;
    public Sprite skinIcon;
    public int skinIndex;
    public float skinScale = 0.5f;
    public GameObject skinMesh;
    public Material skinMaterial;
    public bool isUnlocked = false;
    public AudioClip skinSoundEffect;
}

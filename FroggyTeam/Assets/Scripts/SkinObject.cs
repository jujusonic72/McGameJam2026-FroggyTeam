using UnityEngine;

[CreateAssetMenu(fileName = "SkinObject", menuName = "Scriptable Objects/SkinObject")]
public class SkinObject : ScriptableObject
{
    public string skinName;
    public Sprite skinIcon;
    public int skinIndex;
    public GameObject skinPrefab;
    public bool isUnlocked = false;
}

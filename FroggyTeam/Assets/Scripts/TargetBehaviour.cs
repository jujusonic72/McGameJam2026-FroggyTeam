using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    public void OnTargetHit()
    {
        FindFirstObjectByType<GameManager>().targets.Remove(this);
        Destroy(gameObject);
    }
}

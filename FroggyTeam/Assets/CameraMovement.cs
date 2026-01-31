using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraAnchor;

    public bool canMove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.position = cameraAnchor.transform.position;
            transform.rotation = cameraAnchor.transform.rotation;
        }
    }
    
    public void StartCamMovement()
    {
        StartCoroutine(GetCamToSpot());
    }

    private IEnumerator GetCamToSpot()
    {
        while(Vector3.Distance(transform.position, cameraAnchor.transform.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, cameraAnchor.transform.position, 1f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraAnchor.transform.rotation, 1f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        canMove = true;
    }
}

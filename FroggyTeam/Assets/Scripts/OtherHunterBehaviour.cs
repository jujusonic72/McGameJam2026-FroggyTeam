using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class OtherHunterBehaviour : MonoBehaviour
{
    public List<Vector3> patrolPoints;
    public float speed = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveToPoint(0));
    }
    IEnumerator MoveToPoint(int index)
    {
        Vector3 targetPoint = patrolPoints[index];
        while (Vector3.Distance(transform.position, targetPoint) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
            yield return null;
        }
        int nextIndex = (index + 1) % patrolPoints.Count;
        StartCoroutine(MoveToPoint(nextIndex));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

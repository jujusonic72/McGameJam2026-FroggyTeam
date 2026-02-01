using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HunterBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool isWalking;

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject navigationPointsPlace;

    [SerializeField]
    private List<Vector3> positions = new List<Vector3>();

    private int posIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInChildren<Animator>().SetBool("IsWalking", isWalking);

        Transform[] trans = navigationPointsPlace.GetComponentsInChildren<Transform>();
        foreach (Transform t in trans)
        {
            positions.Add(t.position);
            print(t.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            if (Vector3.Distance(transform.position, positions[posIndex]) < 0.1f)
            {
                posIndex = (posIndex + 1) % positions.Count;
                return;
            }

            transform.forward = (positions[posIndex] - transform.position).normalized;
            transform.Translate(transform.forward * speed * Time.deltaTime);

            
        }
    }
}

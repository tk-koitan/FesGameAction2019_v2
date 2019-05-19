using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour
{
    [SerializeField]
    private Transform playerTrfm;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = playerTrfm.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTrfm.position;
    }
}

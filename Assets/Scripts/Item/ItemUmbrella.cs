using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class ItemUmbrella : MonoBehaviour
{
    public GameObject umbrella;
    public Vector3 offset = new Vector3(0, -0.27f, 0);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject tmpObj = Instantiate(umbrella, collision.transform.position + offset, Quaternion.Euler(0, 0, 180));
            tmpObj.GetComponent<JoyconUmbrella>().target = collision.transform;
            collision.GetComponent<PlayerRB>().ik.SetActive(true);
            Transform solverTarget = collision.GetComponent<PlayerRB>().handPos;
            Transform tuka = tmpObj.GetComponent<JoyconUmbrella>().tuka;
            solverTarget.position = tuka.position;
            solverTarget.SetParent(tuka);

            Destroy(gameObject);
        }
    }
}

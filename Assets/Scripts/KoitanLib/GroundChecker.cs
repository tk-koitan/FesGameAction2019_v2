using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public List<Mover> riders = new List<Mover>();
    public List<Mover> rmRiders = new List<Mover>();
    public bool rideOn;
    public PlayerRB playerRB;

    private void Update()
    {
        foreach(Mover mover in riders)
        {
            //Debug.DrawLine(transform.position, mover.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject groundObj = collision.gameObject;
        if(groundObj.layer == (int)LayerName.MovingPlatform)
        {
            riders.Add(groundObj.GetComponent<Mover>());
            rideOn = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject groundObj = collision.gameObject;
        Mover tmpM = groundObj.GetComponent<Mover>();
        if (groundObj.layer == (int)LayerName.MovingPlatform)
        {
            if(!tmpM.ridingPlayers.Contains(playerRB))
            {
                tmpM.ridingPlayers.Add(playerRB);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject groundObj = collision.gameObject;
        if (groundObj.layer == (int)LayerName.MovingPlatform)
        {
            rmRiders.Add(groundObj.GetComponent<Mover>());
            //riders.Remove(groundObj.GetComponent<Mover>());
        }
    }
}

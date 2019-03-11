using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStressTest : MonoBehaviour
{
    public GameObject playerPrefab;
    public int rayReTimes = 4;
    public float rayLength = 10;
    public Vector3 direction = new Vector3(0,1,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(playerPrefab);
        }
        */
    }

    private void OnDrawGizmos()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 10;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, direction, rayLength);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(worldPos, direction * rayLength);
        if (hit)
        {
            int cnt = rayReTimes;
            while(hit.point == (Vector2)worldPos && cnt>0)
            {
                cnt--;
                Gizmos.color = Color.red;
                Gizmos.DrawRay(hit.point, Quaternion.Euler(0,0,90)*hit.normal);
                worldPos -= direction * rayLength;
                hit = Physics2D.Raycast(worldPos, direction, rayLength);
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(worldPos, direction * rayLength);
            }
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hit.point, hit.normal);
        }

        /*
        Collider2D rap = Physics2D.OverlapBox(worldPos, new Vector2(1, 1), 0);
        if(rap)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(worldPos, new Vector3(1, 1, 1));
            return;
        }
        RaycastHit2D hit = Physics2D.BoxCast(worldPos, new Vector2(1, 1),0,direction,rayLength);
        if (hit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(hit.point, new Vector3(1, 1, 1));
            Gizmos.DrawRay(hit.point, hit.normal);
        }
        */
    }
}

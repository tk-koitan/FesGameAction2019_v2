using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;

    PlayerRB player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").
                    GetComponent<PlayerRB>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionInput.GetButtonDown(ButtonCode.Cancel))
        {
            if (GetIsGround() == false)
            {
                return;
            }
            pauseUI.SetActive(!pauseUI.activeSelf);

            if (pauseUI.activeSelf)
            {
                player.GetComponent<PlayerRB>().enabled = false;
                Time.timeScale = 0f;
            }
            else
            {
                player.GetComponent<PlayerRB>().enabled = true;
                Time.timeScale = 1f;
            }
        }
    }

    private bool GetIsGround()
    {
        return player.GetComponent<PlayerRB>().isGround;
    }
}

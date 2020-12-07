using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    private PhotonView PV;
    GameObject PauseMenuCanvas;
    Transform PauseMenu;
    bool pauseMenuEnable = false;
 

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PauseMenuCanvas = GameObject.Find("MenuCanvas");
        Transform[] child = PauseMenuCanvas.GetComponentsInChildren<Transform>(true);
        foreach (Transform c in child)
        {
            if (c.name == "PauseMenu")
                PauseMenu = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!PV.IsMine)
        //{
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuEnable == false)
            {
                Time.timeScale = 0;
                pauseMenuEnable = true;
                PauseMenuCanvas.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
               

            }
            else
            {
                ResumeGame();
            }
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenuEnable = false;
        PauseMenuCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
   
}

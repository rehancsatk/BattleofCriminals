using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject Controls;
    
    public void OnClickControls()
    {
       // SceneManager.LoadScene("Controls");
        Controls.SetActive(true);
    }
    public void OnClickBack()
    {
        // SceneManager.LoadScene("Controls");
        Controls.SetActive(false);
    }
  
}

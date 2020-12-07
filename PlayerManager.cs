using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public static int health = 100;
    public GameObject player;
    public Slider healthbar;
    void Start()
    {
       // InvokeRepeating("ReduceHealth", 1, 1);
    }

    void ReduceHealth()
    {
        health = health - 20;
        healthbar.value = health;
        if(health <=0)
        {
            player.GetComponent<Animator>().SetTrigger("Dead");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

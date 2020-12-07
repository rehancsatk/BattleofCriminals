using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Audio : MonoBehaviour
{
    private AudioSource audioclip;

    [SerializeField]
    private AudioClip[] Health_Down_Clips;

    [SerializeField]
    private AudioClip die_Clip,Bombblast;

    [SerializeField]
    private AudioClip[] Footsteps_Clips;
    void Awake()
    {
        audioclip = GetComponent<AudioSource>();
    }

   
    public void hit_Sound()
    {
        audioclip.clip = Health_Down_Clips[Random.Range(0, Health_Down_Clips.Length)];
        audioclip.Play();

    }
    public void DiedSound()
    {
        audioclip.clip = die_Clip;
        audioclip.Play();
    }
    public void bombblast()
    {
        audioclip.clip = Bombblast;
        audioclip.Play();
    }
    public void Footstep()
    {
        audioclip.clip = Footsteps_Clips[Random.Range(0, Footsteps_Clips.Length)];
        audioclip.Play();
    }
}

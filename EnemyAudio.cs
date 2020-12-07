using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioclip;

    [SerializeField]
    private AudioClip[] attack_clips;

    [SerializeField]
    private AudioClip scream_Clip, die_Clip;
    void Awake()
    {
        audioclip = GetComponent<AudioSource>();
    }

    public void Play_ScreamSound()
    {
        audioclip.clip = scream_Clip;
        audioclip.Play();
    }
    public void Play_AttackSound()
    {
        audioclip.clip = attack_clips[Random.Range(0, attack_clips.Length)];
        audioclip.Play();

    }
    public void Play_DiedSound()
    {
        audioclip.clip = die_Clip;
        audioclip.Play();
    }
}

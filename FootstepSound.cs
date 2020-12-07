using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(AudioSource))]
//[System.Serializable]
//public class TextureType
//{
//    public string name;
//    public Texture[] textures;
//    public AudioClip[] footstepSounds;
//}

public class FootstepSound : MonoBehaviour
{
    private Player_Audio player_audio;
    public void Start()
    {
        player_audio = GetComponentInChildren<Player_Audio>();
    }
    public void PlayFootstepSound()
    {
        player_audio.Footstep();
    }

}


//    private AudioSource m_Audio;
//    public AudioClip[] _GrassSounds;
//    public AudioClip[] _MudSounds;
//    public AudioClip[] _GravelSounds;
//    public CharacterController _CharController;
//    private Rigidbody _RigBod;
//    private bool playSteps;

//    void Awake()
//    {
//        m_Audio = gameObject.AddComponent<AudioSource>();
//        _RigBod = _CharController.GetComponent<Rigidbody>();
//    }

//    void Start()
//    {
//       PlayFootstepSound();
//        playSteps = true;
//    }

//    void Update()
//    {
//        if (!_CharController.isGrounded && playSteps)
//        {
//            playSteps = false;
//           PlayFootstepSound();
//        }
//        else if (_CharController.isGrounded && !playSteps && _CharController.velocity.magnitude > 0.2f)
//        {
//          PlayFootstepSound();
//        }
//    }


//   public void PlayFootstepSound()
//    {
//        int curSplatIndex = TerrainSurface.GetMainTexture(transform.position);

//        switch (curSplatIndex)
//        {
//            case 0:
//               // yield return new WaitForSeconds(0.4f);
//                m_Audio.PlayOneShot(_GrassSounds[Random.Range(0, _GrassSounds.Length )]);
//                break;
//            case 1:
//                m_Audio.PlayOneShot(_GravelSounds[Random.Range(0, _GravelSounds.Length )]);
//                break;
//            case 2:
//                m_Audio.PlayOneShot(_MudSounds[Random.Range(0, _MudSounds.Length - 1)]);
//                break;
//        }
//    }
//}

//    public TextureType[] textureTypes;

//    public AudioSource audioS;

//    SoundSystem sc;

//     Use this for initialization
//    void Start()
//    {
//        GameObject check = GameObject.FindGameObjectWithTag("Sound Controller");

//        if (check != null)
//        {
//            sc = check.GetComponent<SoundSystem>();
//        }
//    }

//    void PlayFootstepSound()
//    {
//        RaycastHit hit;
//        Vector3 start = transform.position + transform.up;
//        Vector3 dir = Vector3.down;

//        if (Physics.Raycast(start, dir, out hit, 1.3f))
//        {
//            if (hit.collider.GetComponent<MeshRenderer>())
//            {
//                PlayMeshSound(hit.collider.GetComponent<MeshRenderer>());
//            }
//            else if (hit.collider.GetComponent<Terrain>())
//            {
//                PlayTerrainSound(hit.collider.GetComponent<Terrain>(), hit.point);
//            }
//        }
//    }

//    void PlayMeshSound(MeshRenderer renderer)
//    {

//        if (audioS == null)
//        {
//            Debug.LogError("PlayMeshSound -- We have no audio source to play the sound from.");
//            return;
//        }

//        if (sc == null)
//        {
//            Debug.LogError("PlayMeshSound -- No sound manager!!!");
//            return;
//        }

//        if (textureTypes.Length > 0)
//        {
//            foreach (TextureType type in textureTypes)
//            {

//                if (type.footstepSounds.Length == 0)
//                {
//                    return;
//                }

//                foreach (Texture tex in type.textures)
//                {
//                    if (renderer.material.mainTexture == tex)
//                    {
//                        sc.PlaySound(audioS, type.footstepSounds[Random.Range(0, type.footstepSounds.Length)], true, 1, 1.2f);
//                    }
//                }
//            }
//        }
//    }

//    void PlayTerrainSound(Terrain t, Vector3 hitPoint)
//    {
//        if (audioS == null)
//        {
//            Debug.LogError("PlayTerrainSound -- We have no audio source to play the sound from.");
//            return;
//        }

//        if (sc == null)
//        {
//            Debug.LogError("PlayTerrainSound -- No sound manager!!!");
//            return;
//        }

//        if (textureTypes.Length > 0)
//        {
            
//            int textureIndex = TerrainSurface.GetMainTexture(hitPoint);

//            foreach (TextureType type in textureTypes)
//            {

//                if (type.footstepSounds.Length == 0)
//                {
//                    return;
//                }

//                foreach (Texture tex in type.textures)
//                {
//                    if (t.terrainData.splatPrototypes[textureIndex].texture )
//                    {
//                        sc.PlaySound(audioS, type.footstepSounds[Random.Range(0, type.footstepSounds.Length)], true, 1, 1.2f);
                    
//                }
//            }
//        }
//    }
//}
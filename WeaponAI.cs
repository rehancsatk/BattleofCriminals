using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class WeaponAI : MonoBehaviour
{
    private Collider col { get { return GetComponent<Collider>(); } set { col = value; } }
    private Rigidbody rigidBody { get { return GetComponent<Rigidbody>(); } set { rigidBody = value; } } 
   private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }
   private SoundSystem sc;
    public enum WeaponType
    {
        Pistol,
        Rifle,
    }
    public WeaponType weaponType;

    [System.Serializable]
    public class UserSettings
    {
        public Transform leftHandIKTarget;
        public Vector3 spineRotation;
    }
    [SerializeField]
    public UserSettings usersettings;

    [System.Serializable]
    public class WeaponSettings
    {
        [Header("..Bullet Options")]
        public Transform bulletSpan;
        public float damage = 5.0f;
        public float bulletSpread = 5.0f;
        public float fireRate = 0.2f;
        public LayerMask bulletLayers;
        public float range = 200.0f;

        [Header("...Effects")]

        public GameObject muzzleflash;
        public GameObject decal;
        public GameObject shell;
        public GameObject clip;

        [Header("----others---")]
        public GameObject CrossHiar_Prefab;
        public float reloadDuration = 2.0f;
        public Transform shellEjectSpot;
        public float shellEjectSpeed = 7.5f;
        public Transform clipEjectPos;
        public GameObject clipGo;

        [Header("--Positioning---")]

        public Vector3 equipPosition;
        public Vector3 equipRotation;
        public Vector3 unequipPosition;
        public Vector3 unequipRotation;

        [Header("----Animation---")]
        public bool UseAnimation;
        public int FireAnimationLayer = 0;
        public string fireAnimationName = "Fire";
    }
    [SerializeField]
    public WeaponSettings weaponSettings;

    [System.Serializable]
    public class SoundsSettings
    {
        public AudioClip[] gunshotSounds;
        public AudioClip reloadSound;
        [Range(0, 3)]
        public float pitchMin = 1;
        [Range(0, 3)]
        public float pitchMax = 1.2f;
        public AudioSource audioS;
    }
    [SerializeField]
    public SoundsSettings sounds;

    [System.Serializable]
    public class Ammuntion
    {
        public int carryingAmmo;
        public int clipAmmo;
        public int MaxClipAmmo;

    }
    [SerializeField]
    public Ammuntion Ammo;

   
    public Ray shootRay { protected get; set; }
    public bool ownerAiming { get; set; }

    WeaponHandlerAI owner;
    bool equipped;
    //bool pullingTrigger;
    bool resettingCartridge;

    // Start is called before the first frame update
    void Start()
    {
        GameObject check = GameObject.FindGameObjectWithTag("Sound Controller");

        if (check != null)
        {
            sc = check.GetComponent<SoundSystem>();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (owner)
        {
            DisableEnableComponents(false);
            if (equipped)
            {
                if (owner.userSettings.rightHand)
                {
                    Equip();
                }
            }
            else
            {
                if (weaponSettings.bulletSpan.childCount > 0)
                {
                    foreach (Transform t in weaponSettings.bulletSpan.GetComponentsInChildren<Transform>())
                    {
                        if (t != weaponSettings.bulletSpan)
                        {
                            Destroy(t.gameObject);
                        }
                    }
                }
                Unequip(weaponType);
            }
        }
        else
        { // If owner is null
            DisableEnableComponents(true);
            transform.SetParent(null);
        }

       
    }

    
        //if (owner)
        //{
        //    DisableEnableComponents(false);
        //    if (equipped)
        //    {
        //        if (owner.userSettings.rightHand)
        //        {
        //            Equip();

        //            if (pullingTrigger)
        //            {
        //                fire(shootRay);
        //            }
        //            if (ownerAiming)
        //            {
        //                PositionCrossHair(shootRay);
        //            }
        //            else
        //            {
        //                ToggleCrossHair(false);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Unequip(weaponType);
        //    }
        //}
        //else
        //{
        //    DisableEnableComponents(true);
        //    transform.SetParent(null);
        //}
    

    //----------------This will able to fire the weapon
    public void fire( Ray ray)
    {
        if (Ammo.clipAmmo <= 0 || resettingCartridge || !weaponSettings.bulletSpan || !equipped)
            return;

        RaycastHit hit;
        Transform bSpawn = weaponSettings.bulletSpan;
        Vector3 bSpawnPoint = bSpawn.position;
        Vector3 dir = ray.GetPoint(weaponSettings.range) - bSpawnPoint; // camera aur gun ki ray aik point py meet karein ghi wo point hu ga

        dir += (Vector3)Random.insideUnitCircle * weaponSettings.bulletSpread;

        if (Physics.Raycast(bSpawnPoint, dir, out hit, weaponSettings.range, weaponSettings.bulletLayers))
        {
            HitEffects(hit);

            if(hit.transform.root.GetComponent<Stats>())
            {
                hit.transform.root.SendMessage("Damage", weaponSettings.damage);

            }
        }
        GunEffects();

        if (weaponSettings.UseAnimation)
            animator.Play(weaponSettings.fireAnimationName, weaponSettings.FireAnimationLayer);

        Ammo.clipAmmo--;
        resettingCartridge = true;
        StartCoroutine(LoadNextBullet());

        
    }

    //---------------------------------------------------------------Loads the next bullet into the chamber
    IEnumerator LoadNextBullet()
    {
        yield return new WaitForSeconds(weaponSettings.fireRate);
        resettingCartridge = false;
    }

    //--------------------------------------------------------- this function will toggle on and off the crosshair prefab
    //void ToggleCrossHair(bool enabled)
    //{
    //    if(weaponSettings.CrossHiar_Prefab !=null)
    //    {
    //        weaponSettings.CrossHiar_Prefab.SetActive(enabled);
    //    }
    //}

    // --------------------------- this function is used for positioning our crosshair

    //void PositionCrossHair(Ray ray)
    //{

    //    RaycastHit hit;
    //    Transform bSpawn = weaponSettings.bulletSpan;
    //    Vector3 bSpawnPoint = bSpawn.position;
    //    Vector3 dir = ray.GetPoint(weaponSettings.range); // camera aur gun ki ray aik point py meet karein ghi wo point hu ga

        
    //    if (Physics.Raycast(bSpawnPoint, dir, out hit, weaponSettings.range, weaponSettings.bulletLayers))
    //    {
    //       if(weaponSettings.CrossHiar_Prefab !=null)
    //       {
    //           ToggleCrossHair(true);
    //           weaponSettings.CrossHiar_Prefab.transform.position = hit.point;
    //           weaponSettings.CrossHiar_Prefab.transform.LookAt(Camera.main.transform);

    //       }
    //    }
    //    else
    //    {
    //        ToggleCrossHair(false);
    //    }


    //}
    public void HitEffects(RaycastHit hit)
    {
        if (hit.collider.gameObject.isStatic)
        {
            if (weaponSettings.decal)
            {
                Vector3 hitPoint = hit.point;
                Quaternion lookRotation = Quaternion.LookRotation(hit.normal);
                GameObject decal = Instantiate(weaponSettings.decal, hitPoint, lookRotation) as GameObject;
                Transform decalIT = decal.transform;
                Transform hitT = hit.transform;
                decalIT.SetParent(hitT);
                Destroy(decal, Random.Range(30.0f, 45.0f));

            }

        }

    }

    void GunEffects()
    {
        #region muzzle Flash
        if (weaponSettings.muzzleflash)
        {
            Vector3 bulletSpawnPos = weaponSettings.bulletSpan.position;
            GameObject muzzleFlash = Instantiate(weaponSettings.muzzleflash, bulletSpawnPos, Quaternion.identity) as GameObject;
            Transform muzzleIT = muzzleFlash.transform;
            muzzleIT.SetParent(weaponSettings.bulletSpan);
            Destroy(muzzleFlash, 1.0f);                 //-----------------------------after 1 second muzzleflash jo show hu ga wo destroy hu jae ga after bullet ejection
        }
        #endregion


        #region shell
        if (weaponSettings.shell)
        {
            if (weaponSettings.shellEjectSpot)
            {
                Vector3 shellEjectPos = weaponSettings.shellEjectSpot.position;
                Quaternion shellEjectRot = weaponSettings.shellEjectSpot.rotation;
                GameObject shell = Instantiate(weaponSettings.shell, shellEjectPos, shellEjectRot) as GameObject;

                if (shell.GetComponent<Rigidbody>())
                {
                    Rigidbody rigiDB = shell.GetComponent<Rigidbody>();
                    rigiDB.AddForce(weaponSettings.shellEjectSpot.forward * weaponSettings.shellEjectSpeed, ForceMode.Impulse);

                }
                Destroy(shell, Random.Range(30.0f, 45.0f));
            }
        }
        PlayGunshotSound();
        #endregion
    }

    void PlayGunshotSound()
    {
        if (sc == null)
        {
            return;
        }

        if (sounds.audioS != null)
        {
            if (sounds.gunshotSounds.Length > 0)
            {
                sc.InstantiateClip(
                    weaponSettings.bulletSpan.position, // Where we want to play the sound from
                    sounds.gunshotSounds[Random.Range(0, sounds.gunshotSounds.Length)],  // What audio clip we will use for this sound
                    2, // How long before we destroy the audio
                    true, // Do we want to randomize the sound?
                    sounds.pitchMin, // The minimum pitch that the sound will use.
                    sounds.pitchMax); // The maximum pitch that the sound will use.
            }
        }
    }

 




    //-----------------------------------------------------------------Disables or Enables collider and rigidbody
    void DisableEnableComponents(bool enabled)
    {
        if (!enabled)
        {
            rigidBody.isKinematic = true;
            col.enabled = false;
        }
        else
        {
            rigidBody.isKinematic = false;
            col.enabled = true;
        }
    }

    //-------------Equips this Weapon to the hand of player
   public void Equip()
    {
        if (!owner)
            return;

        else if (!owner.userSettings.rightHand)
            return;

        transform.SetParent(owner.userSettings.rightHand);
        transform.localPosition = weaponSettings.equipPosition;
        Quaternion equipRot = Quaternion.Euler(weaponSettings.equipRotation);
        transform.localRotation = equipRot;
    }
    //------------------------Unequips the weapon and places it to the desired location
    void Unequip(WeaponType wpType)
    {
        if (!owner)
            return;

        switch (wpType)
        {
            case WeaponType.Pistol:
                transform.SetParent(owner.userSettings.pistolUnequipSpot);
                break;
            case WeaponType.Rifle:
                transform.SetParent(owner.userSettings.rifleUnequipSpot);
                break;
          
        }
        transform.localPosition = weaponSettings.unequipPosition;
        Quaternion unEquipRot = Quaternion.Euler(weaponSettings.unequipRotation);
        transform.localRotation = unEquipRot;
    }

    //---------------------------------------Loads the clip and calculate all the ammo
  
    
    public void LoadClip()
    {
        int ammoNeeded = Ammo.MaxClipAmmo - Ammo.clipAmmo;

        if (ammoNeeded >= Ammo.carryingAmmo)
        {
            Ammo.clipAmmo = Ammo.carryingAmmo;
            Ammo.carryingAmmo = 0;
        }
        else
        {
            Ammo.carryingAmmo -= ammoNeeded;
            Ammo.clipAmmo = Ammo.MaxClipAmmo;
        }
    }

    //---------------------------Set the Weapon Equip State
    public void SetEquipped(bool equip)
    {
        equipped = equip;
    }

    //-----------------------------pulls the trigger
    //public void PullTrigger(bool isPulling)
    //{
    //   // pullingTrigger = isPulling;
    //}

    //----------------------------Set the owner of weapon
    public void SetOwner(WeaponHandlerAI wp)
    {
        owner = wp;
    }

  
}


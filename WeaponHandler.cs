using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponHandler : MonoBehaviour
{
    Animator animator;
    SoundSystem sc;

    private PhotonView pv; 

    [System.Serializable]
    public class UserSettings
    {
        public Transform rightHand;
        public Transform pistolUnequipSpot;
        public Transform rifleUnequipSpot;
        public Transform knifeUnequipSpot;
    }

    [SerializeField]
    public UserSettings userSettings;

    [System.Serializable]
    public class Animations
    {
        public string weaponTypeInt = "WeaponType";
        public string reloadingBool = "isReloading";
        public string aimingBool = "isAiming";
        public string attack = "IsAttack";
    }
    [SerializeField]
    public Animations animations;
    
   
    public Weapon currentWeapon;
    public List<Weapon> weaponList = new List<Weapon>();
    public List<Weapon> OrginalList = new List<Weapon>();
    public int maxWeapons = 3;  /// you may Exceed when you want more weapons..............
    bool aim;
    bool reload;
    bool attack;
    int WeaponType;
    bool settingWeapon;

    public Transform player;
    public float weapondistance;

    public GameObject UI;
    public GameObject UI2;

    
    public GameObject playerGun1;
    public Transform groundGun1;
    public GameObject playerGun2;
    public Transform groundGun2;

    bool objectt=false;
    bool objectt1 = false;
   
    public float Distance = 0.1f;

    private bool isenable=false;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        groundGun1 = GameObject.FindGameObjectWithTag("Gun1").transform;
        groundGun2 = GameObject.FindGameObjectWithTag("Gun2").transform;
        UI = GameObject.FindGameObjectWithTag("1st");
        UI2 = GameObject.FindGameObjectWithTag("2nd");


    }
    void OnEnable()
    {
        GameObject check = GameObject.FindGameObjectWithTag("Sound Controller");
        if (check != null)
            sc = check.GetComponent<SoundSystem>();
        playerGun1.SetActive(false);
        playerGun2.SetActive(false);

        animator = GetComponent<Animator>();
        SetupWeapons();
       

    }

    
    void SetupWeapons()
    {
        if (currentWeapon )
        {
            currentWeapon.SetEquipped(true);
            currentWeapon.SetOwner(this);
            AddWeaponToList(currentWeapon);

            if (currentWeapon.Ammo.clipAmmo <= 0)
                Reload();

            if (reload)
                if (settingWeapon)
                    reload = false;//---------------jb weapon set ya switch hu rae hu then weapon will not able to reload
        }

        if (weaponList.Count > 0)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i] != currentWeapon)
                {
                    weaponList[i].SetEquipped(false);
                    weaponList[i].SetOwner(this);
                }
            }
        }
    }

    // adds weapon to the weapon list
    void AddWeaponToList(Weapon weapon)
    {
        if (weaponList.Contains(weapon))
            return;
        weaponList.Add(weapon);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            Animate();
            //Pick();
            PickWeapons();
        }
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    currentWeapon.SetEquipped(false);
        //}
    }


    //Animation the character weapons

    public void Animate()
    {
        pv.RPC("RPC_Animate", RpcTarget.All);


    }
    [PunRPC]

    void RPC_Animate()
    {
        if (!animator)
            return;
        animator.SetBool(animations.aimingBool, aim);
        animator.SetBool(animations.reloadingBool, reload);
        animator.SetBool(animations.attack, attack);
        animator.SetInteger(animations.weaponTypeInt, WeaponType);

        if (!currentWeapon)
        {
            WeaponType = 0;
            return;
        }
        if (currentWeapon.weaponType == Weapon.WeaponType.Pistol)
        {
            if (playerGun1.activeSelf && currentWeapon)
            {
                WeaponType = 1;
            }
        }
        if (currentWeapon.weaponType == Weapon.WeaponType.Rafle)
        {
            if (playerGun2.activeSelf && currentWeapon)
            {
                WeaponType = 2;
            }
        }


        //switch (currentWeapon.weaponType)
        //{
        //    case Weapon.WeaponType.Pistol:
        //        WeaponType = 1;
        //        break;
        //    case Weapon.WeaponType.Rifle:
        //        WeaponType = 2;
        //        break;
        //    case Weapon.WeaponType.Knife:
        //        WeaponType = 3;
        //        break;

        //}
    }

    // puts the finger on trigger and ask if we pulled or not

    [PunRPC]

    public void FireCurrentWeapon()
    {
        pv.RPC("RPC_FireCurrentWeapon", RpcTarget.All);


    }
    [PunRPC]
    
    public void RPC_FireCurrentWeapon(Ray aimRay)
    {
        if (currentWeapon.Ammo.clipAmmo == 0)
        {
         
            Reload();
            return;
            
        }

        currentWeapon.fire(aimRay);

    }


    public void Reload()
    {
        pv.RPC("RPC_Reload", RpcTarget.All);

       
    }
    [PunRPC]
    //Start reloading the weapon
    public void RPC_Reload()
    {
        if (reload || !currentWeapon)
            return;

        if (currentWeapon.Ammo.carryingAmmo <= 0 || currentWeapon.Ammo.clipAmmo == currentWeapon.Ammo.MaxClipAmmo)
            return;

        if (sc != null)
        {
            if (currentWeapon.sounds.reloadSound != null)
            {
                if (currentWeapon.sounds.audioS != null)
                {
                    sc.PlaySound(currentWeapon.sounds.audioS, currentWeapon.sounds.reloadSound, true, currentWeapon.sounds.pitchMin, currentWeapon.sounds.pitchMax);
                }
            }
        }
        reload = true;
        StartCoroutine(StopReload());
    }

    // stop reloading the weapons
    IEnumerator StopReload()
    {
        yield return new WaitForSeconds(currentWeapon.weaponSettings.reloadDuration);
        currentWeapon.LoadClip();
        reload = false;
    }

    //sets out aim bool to be what we pass it
    public void Aim(bool aiming)
    {
        aim = aiming;
    }
    

    //Drops the current weapon
    public void DropCurrentWeapon()
    {
         pv.RPC("RPC_DropCurrent", RpcTarget.All);
     
    }
    [PunRPC]
    void RPC_DropCurrent()
    {
        if (!currentWeapon)
            return;

        currentWeapon.SetEquipped(false);
        currentWeapon.SetOwner(null);
        
        weaponList.Remove(currentWeapon);
        currentWeapon = null;
        

    }
    //public void Pick()
    //{


    //        UI.SetActive(true);
    //        if (Input.GetKeyDown(KeyCode.T))
    //        {
    //            currentWeapon.SetEquipped(true);
    //            currentWeapon.SetOwner(this);
    //            weaponList.Add(currentWeapon);
    //        }
        
    //}


    //Switches to the next weapon
    public void SwitchWeapons()
    {
        pv.RPC("RPC_SwitchWeapon", RpcTarget.All);
       
    }
    [PunRPC]
    public void RPC_SwitchWeapon()
    {
        if (settingWeapon || weaponList.Count == 0)
            return;

        if (currentWeapon)
        {
            int currentweaponIndex = weaponList.IndexOf(currentWeapon);
            int nextWeaponIndex = (currentweaponIndex + 1) % weaponList.Count;

            currentWeapon = weaponList[nextWeaponIndex];
        }
        else
        {
            currentWeapon = weaponList[0];

        }
        settingWeapon = true;
        StartCoroutine(StopSettingWeapon());

        SetupWeapons();
    }

    //stops swaping weapon
    IEnumerator StopSettingWeapon()
    {
        yield return new WaitForSeconds(0.7f);
        settingWeapon = false;
    }


    public void OnAnimatorIK()
    {
        pv.RPC("RPC_OnAnimatorIK", RpcTarget.All);


    }
    [PunRPC]
    void RPC_OnAnimatorIK()
    {
        if (!animator)
            return;

        if (currentWeapon && currentWeapon.usersettings.leftHandIKTarget && WeaponType == 2 && !reload && !settingWeapon)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            Transform target = currentWeapon.usersettings.leftHandIKTarget;
            Vector3 targetPos = target.position;
            Quaternion targetRot = target.rotation;
            animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPos);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, targetRot);

        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
    }
    public void PickWeapons()
    {
        pv.RPC("RPC_PickWeapons", RpcTarget.All);

      
    }
   [PunRPC]
    void RPC_PickWeapons()
    {
        if (Vector3.Distance(groundGun1.position, transform.position) <= Distance && objectt==false)
        {

            UI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                playerGun1.SetActive(true);
                UI.SetActive(false);
                groundGun1.gameObject.SetActive(false);
                objectt = true;
                weaponList.Add(OrginalList[0]);
                if (weaponList.Count > 0)
                {
                    for (int i = 0; i < weaponList.Count; i++)
                    {
                        if (weaponList[i] != currentWeapon)
                        {
                            weaponList[i].SetEquipped(false);
                            weaponList[i].SetOwner(this);
                        }
                    }
                }

            }
        }else
        {
            UI.SetActive(false);
        }
        if (Vector3.Distance(groundGun2.position, transform.position) <= Distance && objectt1==false)
        {

            UI2.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                playerGun2.SetActive(true);
                UI.SetActive(false);
                groundGun2.gameObject.SetActive(false);
                objectt1 = true;
                weaponList.Add(OrginalList[1]);
                if (weaponList.Count > 0)
                {
                    for (int i = 0; i < weaponList.Count; i++)
                    {
                        if (weaponList[i] != currentWeapon)
                        {
                            weaponList[i].SetEquipped(false);
                            weaponList[i].SetOwner(this);
                        }
                    }
                }
            }
        }
       else
        {
            UI2.SetActive(false);
        }
    }
}
    //    //Vector3 fwd = transform.TransformDirection(Vector3.forward);
    //    //RaycastHit hit;
    //    //if (Physics.Raycast(transform.position, fwd, out hit))
    //    //{

    //    //if (hit.collider.tag == "Gun2")
    //    {
    //        //UI2.SetActive(true);
    //        //if (Input.GetKeyDown(KeyCode.T))
    //        //{
    //        //    groundGun2.SetActive(false);
    //        //    playerGun2.SetActive(true);
    //        //    UI2.SetActive(false);
    //        //    // weaponList.Add(weapon);


    //        //}
    //    }
    //}

        
    






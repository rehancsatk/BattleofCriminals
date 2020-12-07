using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandlerAI : MonoBehaviour
{

    private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }
    private SoundSystem sc;

    public bool isAI;

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

    public WeaponAI currentWeapon;
    public List<WeaponAI> weaponList = new List<WeaponAI>();
  
    public int maxWeapons = 3;  /// you may Exceed when you want more weapons..............
    bool aim;
    bool reload;
    bool attack;
    int WeaponType;
    bool settingWeapon;
    void OnEnable()
    {
        GameObject check = GameObject.FindGameObjectWithTag("Sound Controller");
        if (check != null)
            sc = check.GetComponent<SoundSystem>();
       
      SetupWeapons();
    
    }
  

    void SetupWeapons()
    {
        if (currentWeapon)
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
    void AddWeaponToList(WeaponAI weapon)
    {
        if (weaponList.Contains(weapon))
            return;
        weaponList.Add(weapon);
    }

   
    // Update is called once per frame
    void Update()
    {
       Animate();
       
    }


    //Animation the character weapons
    void Animate()
    {
        if (!animator)
            return;
        animator.SetBool(animations.aimingBool, aim);
        animator.SetBool(animations.reloadingBool, reload);
        animator.SetBool(animations.attack, attack);
        animator.SetInteger(animations.weaponTypeInt, WeaponType);

      
        switch (currentWeapon.weaponType)
        {
            case WeaponAI.WeaponType.Pistol:
                WeaponType = 1;
                break;
            case WeaponAI.WeaponType.Rifle:
                WeaponType = 2;
                break;
          
               
        }
    }
   
    // puts the finger on trigger and ask if we pulled or not

    public void FireCurrentWeapon(Ray aimRay)
    {
        if (currentWeapon.Ammo.clipAmmo == 0)
        {
            //if (WeaponType == 3)
            //{
            //    Attack();
            //}
            //else
            //{
                Reload();
                return;
            //}
        }
      
        currentWeapon.fire(aimRay);
      
    }
    //public void Attack()
    //{
    //    //if (Input.GetButton(input.fireButton) && aiming)
    //    { }
    //    animator.SetBool(animations.attack, attack);
    //}
    //public void FingerOnTrigger(bool pulling)
    //{
    //    if (!currentWeapon)
    //        return;
    //    currentWeapon.PullTrigger(pulling && aim && !reload); // yeh assure kre ga ky idle position me firing na kre
    //}


    //Start reloading the weapon
    public void Reload()
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
        if (!currentWeapon)
            return;

        currentWeapon.SetEquipped(false);
        currentWeapon.SetOwner(null);
        weaponList.Remove(currentWeapon);
        currentWeapon = null;

    }
    

    //Switches to the next weapon
    public void SwitchWeapons()
    {
        if (settingWeapon || weaponList.Count == 0 )
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
    
    void OnAnimatorIK()
    {
        if (!animator)
            return;

        if (currentWeapon && 
            currentWeapon.usersettings.leftHandIKTarget && 
            WeaponType == 2 
            && !reload 
            && !settingWeapon
            && !isAI)
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

}

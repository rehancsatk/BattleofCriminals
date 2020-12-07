using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Stand,
    Prone
}

public class playermovement : MonoBehaviour
{
    Animator anim;
    CharacterController character;
    WeaponHandler weaponHandler;
  Vector3 move_Dir = Vector3.zero;
   public float speed = 3f;
    float gravity = 20f;

    private PlayerState playerstate;
   float crouchSpeed = 1f;
    float jump_force = 10f;
  
 

    void Start()
    {
        playerstate = PlayerState.Stand;
        anim = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        weaponHandler = GetComponent<WeaponHandler>();
    }

    void Update()
    {
        if(playerstate==PlayerState.Stand)
        {
            MovePlayer();
        }
        if(playerstate==PlayerState.Prone)
        {
            Crouch();
        }
       
        Jumping();
        //GetKey();
        //Pistol();
        
    }
  

 
    
    //move the player
    void GetKey()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (anim.GetBool("running") == true || anim.GetBool("backward") ==true || anim.GetBool("left") ==true  || anim.GetBool("right")==true)
            {
                anim.SetBool("running", false);
                anim.SetBool("backward", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
                anim.SetInteger("condition", 0);
                move_Dir = new Vector3(0, 0, 0);
            }
            else if (anim.GetBool("running") == false || anim.GetBool("backward") == false || anim.GetBool("left") == false || anim.GetBool("right") == false)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        anim.SetBool("Attack", true);
        anim.SetInteger("condition", 6);
        yield return new WaitForSeconds(1);
        anim.SetInteger("condition", 0);
        anim.SetBool("Attack", false);
    }

    void Jumping()
    {
        if (character.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {

                //move_Dir = new Vector3(0, 0, 0);
                anim.SetBool("jumping", true);
                anim.SetInteger("condition", 2);
                // move_Dir = new Vector3(0, 1, 0);
                // move_Dir *=4;

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetBool("jumping", false);
                anim.SetInteger("condition", 0);
                // move_Dir = new Vector3(0, 0, 0);
            }
        }
    }
    void Pistol()
    {
        if (character.isGrounded)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {

                //move_Dir = new Vector3(0, 0, 0);
                anim.SetBool("Pistol", true);
                 
                // move_Dir = new Vector3(0, 1, 0);
                // move_Dir *=4;

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetBool("Pistol", false);
                anim.SetInteger("condition", 0);
                // move_Dir = new Vector3(0, 0, 0);
            }
        }
    }

 

    void MovePlayer()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerstate = PlayerState.Prone;
        }
        anim.SetBool("ProneIdle", false);
        anim.SetInteger("condition", 0);
            // look_Root.localPosition = new Vector3(0f, crouch_height, 0f);
        if (character.isGrounded)
        {
            //------------------------------------------------------------------------------------
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("left", true);
                anim.SetInteger("condition", 3);
                move_Dir = new Vector3(-1, 0, 0);
                move_Dir *= speed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetBool("left", false);
                anim.SetInteger("condition", 0);
                move_Dir = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("backward", true);
                anim.SetInteger("condition", 5);
                move_Dir = new Vector3(0, 0, -1);
                move_Dir *= speed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool("backward", false);
                anim.SetInteger("condition", 0);
                move_Dir = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("right", true);
                anim.SetInteger("condition", 4);
                move_Dir = new Vector3(1, 0, 0);
                move_Dir *= speed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetBool("right", false);
                anim.SetInteger("condition", 0);
                move_Dir = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("running", true);
                anim.SetInteger("condition", 1);
                move_Dir = new Vector3(0, 0, 1);
                move_Dir *= speed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("running", false);
                anim.SetInteger("condition", 0);

                move_Dir = new Vector3(0, 0, 0);
            }
        }
            move_Dir.y -= gravity * Time.deltaTime;
            character.Move(move_Dir * Time.deltaTime);

        }
    void Crouch()
    {
        if (Input.GetKey(KeyCode.P))
        {
            
            playerstate = PlayerState.Stand;

        }
         //anim.SetBool("running", false);
           // anim.SetBool("left", false);
            //anim.SetBool("right", false);
           // anim.SetBool("backward", false);

            anim.SetInteger("condition", 10);
            anim.SetBool("ProneIdle", true);

            // look_Root.localPosition = new Vector3(0f, crouch_height, 0f);
            
            //------------------------------------------------------------------------------------
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("ProneLeft", true);

                move_Dir = new Vector3(-1, 0, 0);
                move_Dir *= crouchSpeed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetBool("ProneLeft", false);

                move_Dir = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("ProneBackward", true);
                move_Dir = new Vector3(0, 0, -1);
                move_Dir *= crouchSpeed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool("ProneBackward", false);
                anim.SetInteger("condition", 10);
                move_Dir = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("ProneRight", true);
                move_Dir = new Vector3(1, 0, 0);
                move_Dir *= crouchSpeed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetBool("ProneRight", false);
                anim.SetInteger("condition", 10);
                move_Dir = new Vector3(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.W))
            {

                anim.SetBool("ProneForward", true);
                move_Dir = new Vector3(0, 0, 1);
                move_Dir *= crouchSpeed;
                move_Dir = transform.TransformDirection(move_Dir);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("ProneForward", false);

                anim.SetInteger("condition", 10);
                move_Dir = new Vector3(0, 0, 0);
            }

            // move_Dir.y -= gravity * Time.deltaTime;
            character.Move(move_Dir * Time.deltaTime);





        }
   
    


    public PlayerState Player_State
    {
        get;
        set;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdolHandler : MonoBehaviour
{
    private Collider[] colliders { get { return GetComponentsInChildren<Collider>(); } set { colliders = value; } }
    private Rigidbody[] rigidbody { get { return GetComponentsInChildren<Rigidbody>(); } set { rigidbody = value; } }
    private Animator animator { get { return GetComponentInParent<Animator>(); } set { animator = value; } }
   
    // Start is called before the first frame update
    void Start()
    {
        if (colliders.Length == 0)
            return;
        if (rigidbody.Length == 0)
            return;
        foreach (Collider col in colliders)
        {
            col.isTrigger = true;
        }
        foreach (Rigidbody r in rigidbody)
        {
            r.isKinematic = true;
        }
        
    }
   public void Ragdoll()
    {
        if (animator == null)
            return;
        if (colliders.Length == 0)
            return;
        if (rigidbody.Length == 0)
            return;

        animator.enabled = false;
        foreach(Collider col in colliders)
        {
            col.isTrigger = false;
        }
        foreach(Rigidbody r in rigidbody)
        {
            r.isKinematic = false;
        }

    }

  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Create random movement for bunny across grid, i.e just roam around the terrain by hopping and rotating its body

public class Animal : MonoBehaviour
{
   
    //State:
    float timeToDeathByHunger = 200;
    float timeToDeathByThirst = 200;


    private bool isGrounded;
   

    private bool isJumping;

    private Rigidbody rb;
    public float jumpForce;
    public float speed = 1f;
    private Animator anim;
    // Start is called before the first frame update
    protected void Start()
    {
        /*
        startPos = transform.position;
        endPos = transform.position;
        endPos.x = transform.position.x + jumpDist;
        */
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isGrounded = true;
       
    }


    protected void Update()
    {
        rb.AddForce(new Vector3(2, 0, 0) * speed, ForceMode.Impulse);
        if (isGrounded)
        {
            anim.SetBool("isJumping", true);
            float h = 2f * .3f;
            float v = 2f * .2f;
            
            rb.AddForce(new Vector3(0,3,0) * speed, ForceMode.Impulse);
            
            anim.SetBool("isJumping", false);
            isGrounded = false;
            Debug.Log(isGrounded + "value in update");
        }
                
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "plane")
        {
            isGrounded= true;
            Debug.Log(isGrounded + "value in Oncollisionenter");
        }
    }
}

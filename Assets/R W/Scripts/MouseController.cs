using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour

{
    public float jetpackForce = 75.0f;
    private Rigidbody2D playerRigibody;
    public float forwardMovementSpeed = 3.0f;
    
    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;
    public ParticleSystem jetpack;

    void Start()
    {
        playerRigibody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();
    }

    void FixedUpdate(){
        bool jetpackActive = Input.GetButton("Fire1");
        if (jetpackActive)
        {
            playerRigibody.AddForce(new Vector2(0, jetpackForce));
        }

        Vector2 newVelocity = playerRigibody.velocity;
        newVelocity.x = forwardMovementSpeed;
        playerRigibody.velocity = newVelocity;
        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);


    }

    void UpdateGroundedStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetpack(bool jetpackActive)
   {
        var jetpackEmission = jetpack.emission;
        jetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
           {
               jetpackEmission.rateOverTime = 300.0f;
            }
        else
            {
                jetpackEmission.rateOverTime = 75.0f;
            }
   }


}

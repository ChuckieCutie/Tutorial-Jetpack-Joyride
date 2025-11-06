using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class MouseController : MonoBehaviour

{
    public float jetpackForce = 75.0f;
    private Rigidbody2D playerRigidbody;
    public float forwardMovementSpeed = 3.0f;
    
    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;
    public ParticleSystem jetpack;

    private bool isDead = false;
    private uint coins = 0;

    public TMP_Text coinsCollectedLabel;

    public Button restartButton;
    public AudioClip coinCollectSound;
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;

    public ParallaxScroll parallax;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();
    }

    void FixedUpdate(){
        bool jetpackActive = Input.GetButton("Fire1");
        jetpackActive = jetpackActive && !isDead;

        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        if (!isDead)
        {
            Vector2 newVelocity = playerRigidbody.velocity;
            newVelocity.x = forwardMovementSpeed;
            playerRigidbody.velocity = newVelocity;
        }

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);

        if (isDead && isGrounded)
        {
            restartButton.gameObject.SetActive(true);
        }
    AdjustFootstepsAndJetpackSound(jetpackActive);

    parallax.offset = transform.position.x;

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
    void OnTriggerEnter2D(Collider2D collider)
    {
       if (collider.gameObject.CompareTag("Coins"))
    {
        CollectCoin(collider);
    }
    else
    {
        HitByLaser(collider);
    }
    }

    void HitByLaser(Collider2D laserCollider)
    {
        if (!isDead)
{
    AudioSource laserZap = laserCollider.gameObject.GetComponent<AudioSource>();
    laserZap.Play();
}
        isDead = true;
        mouseAnimator.SetBool("isDead", true);

    }

    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        coinsCollectedLabel.text = coins.ToString();
        Destroy(coinCollider.gameObject);
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("RocketMouse");
    }
void AdjustFootstepsAndJetpackSound(bool jetpackActive)
{
    footstepsAudio.enabled = !isDead && isGrounded;
    jetpackAudio.enabled = !isDead && !isGrounded;
    if (jetpackActive)
    {
        jetpackAudio.volume = 1.0f;
    }
    else
    {
        jetpackAudio.volume = 0.5f;
    }
}


}

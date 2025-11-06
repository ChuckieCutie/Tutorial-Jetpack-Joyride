using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{   
    //1
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;
    //2
    public float toggleInterval = 0.5f;
    public float rotationSpeed = 0.0f;
    //3
    private bool isLaserOn = true;
    private float timeUntilNextToggle;
    //4
    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    void Start()
    {
        timeUntilNextToggle = toggleInterval;
        laserCollider = gameObject.GetComponent<Collider2D>();
        laserRenderer = gameObject.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        timeUntilNextToggle -= Time.deltaTime; 
        if (timeUntilNextToggle <= 0)
        {
            isLaserOn = !isLaserOn;
            laserCollider.enabled = isLaserOn;
            if (isLaserOn)
            {
                laserRenderer.sprite = laserOnSprite;
            }
            else
            {
                laserRenderer.sprite = laserOffSprite;
            }
            timeUntilNextToggle = toggleInterval;
        }
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

    }
}

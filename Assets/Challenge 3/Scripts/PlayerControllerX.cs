using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    private bool permitKeyPress = true;
    private float pressDelay = 0.2f; //If at upper boundary don't allow input for a time
    private float nextPress = -1f;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    private float upperYBoundary = 13.74f;
    private float upSpeed = 6.0f;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;
    private float timeDiff = 0f;


    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        playerRb = this.GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
 
        //check Y position of player. If at upper boundary stop accepting input
        if (transform.position.y >= upperYBoundary)
        {
            Debug.Log("No input allowed");
            permitKeyPress = false;
            nextPress = Time.time + pressDelay;
            transform.position = new Vector3(transform.position.x, upperYBoundary, transform.position.z);
        }
        
        if(permitKeyPress == false && Time.time > nextPress)
        {
            permitKeyPress = true;
        }

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && permitKeyPress == true)
        {

            Debug.Log(" Space Down and moving " + Vector3.up * floatForce);
            playerRb.AddForce(Vector3.up * floatForce);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
            
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        // if player collides with ground, bounce if game is stil active
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            Debug.Log("Bounce");               
            playerAudio.PlayOneShot(bounceSound, 1.0f);
            playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);

        }

    }

}

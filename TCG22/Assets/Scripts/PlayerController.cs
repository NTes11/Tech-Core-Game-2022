using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script in order to allow the player to move, jump, and shoot.
 * You must apply this script to the player object.
 * 
 * Author: Jamie Taube
 */
public class PlayerController : MonoBehaviour
{
    private GameObject Weapon;

    private Rigidbody2D playerRb;

    public Camera playerCamera;
    public Camera overviewCamera;

    public bool FacingRight = true;
    public bool isOnGround = true;
    public bool canMove;
    public bool canAttack;
    public bool hasWeapon = false;
    public bool isAlive = true;

    public int maxJumps = 2;
    public int playerHealth = 100;
    private int jumps;
    public int movementTime = 10; // time in seconds; default 10
    public int attackTime = 30; // time in seconds; default 30 

    //public float launchPower = 10;
    public float jumpForce = 10;
    public float gravityModifier = 1;
    public float speed = 20.0f;
    private float horizontalInput;

    private Vector2 jumpDirection = Vector2.up;
    private Vector2 moveDirection = Vector2.right;
    //public Vector2 launchVelocityVector;
    //public Vector2 launchPositionVector;
    //private Vector3 projectileOffset = Vector3.up * 2;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        Physics.gravity *= gravityModifier;

        canMove = true;
        canAttack = false;
        StartCoroutine(MovementCountdownRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        // Left/right player movement with A, D keys
        horizontalInput = Input.GetAxis("Horizontal");
        if (canMove)
        {
            // Calculate vector in direction of intended movement
            Vector2 movement = horizontalInput * moveDirection;

            // When the player is moving, rotate to face the direction of movement
            if (movement != Vector2.zero)
            {
                if (horizontalInput > 0 && !FacingRight)
                {
                    Flip();
                }
                else if (horizontalInput < 0 && FacingRight)
                {
                    Flip();
                }
            }

            // Move the player
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }

        // Jump with space key
        // NOTE: Make sure that the player object has a RigidBody component with gravity enabled!
        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            Jump();
        }

        //launchVelocityVector = (transform.forward + transform.up) * launchPower;
        //launchPositionVector = transform.position + projectileOffset;
        
        // Launch a projectile from the player on left click
        if (Input.GetMouseButtonDown(0) && hasWeapon && canAttack)
        {
            Weapon.GetComponent<PickUpWeapon>().Shoot();
        }

        // Check if player is still alive
        if (playerHealth < 0)
        {
            isAlive = false;
        }
    }

    // Method to flip the character sprite
    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    

    // Method to control the double jump mechanic
    private void Jump()
    {
        if (jumps > 0)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            jumps = jumps - 1;
        }
        if (jumps == 0)
        {
            return;
        }
    }

    // Determine if the player has collided with an object, such as the ground.
    // NOTE: Make sure the player has a Collider component of some kind, and
    // that the ground is tagged as "Ground"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumps = maxJumps;
            isOnGround = true;
        }
    }

    // Timer to control how long a player can move for
    IEnumerator MovementCountdownRoutine()
    {
        ShowPlayerView();
        yield return new WaitForSeconds(movementTime);
        canMove = false;
        ShowOverheadView();

        // Need to call the turn timer within move routine to have sequential execution
        StartCoroutine(AttackTurnCountdownRoutine());
    }

    // Timer to control how long a player's turn (i.e. attack time) lasts for
    IEnumerator AttackTurnCountdownRoutine()
    {
        canAttack = true;
        yield return new WaitForSeconds(attackTime);
        canAttack = false;
    }

    // Call this function to disable player camera,
    // and enable overview camera.
    public void ShowOverheadView()
    {
        playerCamera.enabled = false;
        overviewCamera.enabled = true;
    }

    // Call this function to enable player camera,
    // and disable overview camera.
    public void ShowPlayerView()
    {
        playerCamera.enabled = true;
        overviewCamera.enabled = false;
    }

    // Method to control collision events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon 1") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 1(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 2") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 2(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 3") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 3(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 4") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 4(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 5") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 5(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 6") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 6(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
            else if (collision.CompareTag("Weapon 7") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 7(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 8") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 8(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 9") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 9(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }
        else if (collision.CompareTag("Weapon 10") && !collision.GetComponent<PickUpWeapon>().held)
        {
            collision.transform.parent = this.transform;
            if (this.transform.rotation.y.Equals(-1))
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(-0.04f, 0.02f, 0));
                Debug.Log("If passed");
            }
            else
            {
                collision.transform.position = this.gameObject.transform.position + (new Vector3(0.04f, 0.02f, 0));
            }
            collision.transform.rotation = this.gameObject.transform.rotation;

            Weapon = GameObject.Find("Weapon 10(Clone)");
            Weapon.GetComponent<PickUpWeapon>().held = true;
            hasWeapon = true;
        }

        /*
        // Detect projectile hit - CURRENTLY BROKEN; PLAYER'S OWN PROJECTILE TRIGGERS THIS
        if (collision.CompareTag("Projectile"))
        {
            // NOTE: Change this from a hard-coded number to a field of the projectile for variable damage
            playerHealth -= 5;

        }
        */
    }
}
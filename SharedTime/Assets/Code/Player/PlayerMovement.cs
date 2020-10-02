using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float Speed;
    public float RunSpeed;
    public float jumpHeight;
    public bool isJumping = false;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
    public bool Character2;
    SpriteRenderer S;
    //public GameObject Gun;
    //public bool holstered;
    GameManager gm;
    void Start()
    {
        Character2 = false;
        rb = GetComponent<Rigidbody2D>();
        //Gun.SetActive(false);
        //ShotGun.SetActive(false);
        //holstered = true;
        //ShotGunholstered = true;
        gm = FindObjectOfType<GameManager>();
        S = GetComponent<SpriteRenderer>();
        S.color = Color.blue;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Character2 == false)
        {
            Character2 = true;
            Debug.Log("Character 2  Playing");
            S.color = Color.green;
        }
        else if (Input.GetKeyDown(KeyCode.X) && Character2)
        {
            Character2 = false;
            Debug.Log("Character 1  Playing");
            S.color = Color.blue;
        }
        if (Character2 == false)
        {
            jumpHeight = 5;
        }
        if (Character2)
        {
            jumpHeight = 10;
        }
        //Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && gm.SprintTime > 0 || Input.GetKeyDown(KeyCode.RightShift) && gm.SprintTime > 0)
        {
            gm.sprint = true;
            Speed = 10;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || gm.SprintTime <= 0)
        {
            gm.sprint = false;
            Speed = 5;
        }


        //Using weapon
        if (Input.GetKeyDown(KeyCode.F) && timeBtwAttack <= 0)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                Debug.Log("Enemy damage" + enemiesToDamage[i]);
                enemiesToDamage[i].GetComponent<Patrol>().health -= damage;

            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }


        //Movement
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            rb.velocity = new Vector3(-Speed, rb.velocity.y, 0f);
            //transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            rb.velocity = new Vector3(Speed, rb.velocity.y, 0f);
            //transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }


        //Jumping
        if (isJumping)
        {
            this.gameObject.transform.parent = null;

            if (!Input.GetKey(KeyCode.Space) && rb.velocity.y > 2)
            {
                rb.gravityScale = 2.7f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            isJumping = true;
        }

        //Testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (gm.playerHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        //Acquiring keys
        if (col.gameObject.CompareTag("key"))
        {
            gm.keys++;
            Destroy(col.gameObject);
            Debug.Log(gm.keys);
        }


        //Going through doors
        if (col.gameObject.CompareTag("Door") && gm.keys <= 0)
        {
            Debug.Log("locked");
        }

        if (col.gameObject.CompareTag("Door") && gm.keys >= 1)
        {
            gm.keys--;
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Ground") && isJumping)
        {
            isJumping = false;
            rb.gravityScale = 1;
            Debug.Log("Check");
            this.gameObject.transform.parent = null;
            if (isJumping == true)
            {
                this.gameObject.transform.parent = null;
            }
        }
        if (col.gameObject.CompareTag("Danger"))
        {
            gm.playerHealth -= 50;
            Debug.Log(gm.playerHealth);
        }

        if (col.gameObject.CompareTag("End"))
        {
            SceneManager.LoadScene(0);
        }


        //Acquiring health kit
        if (col.gameObject.CompareTag("Health"))
        {
            gm.playerHealth += 10;
            Destroy(col.gameObject);
            Debug.Log(gm.playerHealth);
        }
    }
}

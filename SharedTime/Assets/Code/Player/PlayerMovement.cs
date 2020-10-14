using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float Speed;
    public float RunSpeed;
	public bool isFlipped = false;
	
	private int jumpCharge = 1;
    public float jumpHeight;
    public bool isJumping = false;
	
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
	
    public bool Character2;
	public bool characterEnabled = true;
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
		if (Input.GetKeyDown(KeyCode.X) && !isJumping)
			StartCoroutine(updateCharacterSwitch());
		
		/*
        if (Character2 == false)
        {
            jumpHeight = 5;
        }
        if (Character2)
        {
            jumpHeight = 10;
        }
		*/
		
		if (characterEnabled){
			if (Character2)
				updateCharacterOne();
			else
				updateCharacterTwo();
		}
		
		updateJumping();

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
	
	void FixedUpdate(){
		//Movement
		//left
		if (characterEnabled)
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
			isFlipped = true;
			
			if (checkPush())
				rb.velocity = new Vector3(-Speed, rb.velocity.y, 0f);
			else
				rb.velocity = new Vector3(0, rb.velocity.y, 0f);
			
            //transform.localScale = new Vector3(-1f, 1f, 1f);
			
			if (gm.sprint)
				gm.reduceSprint();
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        {
			isFlipped = false;
			
			if (checkPush())
				rb.velocity = new Vector3(Speed, rb.velocity.y, 0f);
			else
				rb.velocity = new Vector3(0, rb.velocity.y, 0f);
			
            //transform.localScale = new Vector3(1f, 1f, 1f);
			
			if (gm.sprint)
				gm.reduceSprint();
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
	}
	
	//CONTROLS AND UPDATE FUNCTIONS
	IEnumerator updateCharacterSwitch(){
		//SWITCH TO CHARACTER 2
		if (!Character2){
			characterEnabled = false;
			rb.velocity = new Vector3(0, rb.velocity.y, 0f);
			yield return new WaitForSeconds(1);
			characterEnabled = true;
				
			Character2 = true;
			Debug.Log("Character 2  Playing");
			S.color = Color.green;
				
		//SWITCH TO CHARACTER 1
		}else{
			characterEnabled = false;
			rb.velocity = new Vector3(0, rb.velocity.y, 0f);
			yield return new WaitForSeconds(1);
			characterEnabled = true;
				
			Character2 = false;
			Debug.Log("Character 1  Playing");
			S.color = Color.blue;
		}
	}

	void updateCharacterOne(){
		//Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && gm.SprintTime > 0 || Input.GetKeyDown(KeyCode.RightShift) && gm.SprintTime > 0)
        {
            gm.sprint = true;
            Speed = 10;
        } 
		else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || gm.SprintTime <= 0)
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
	}
	
	void updateCharacterTwo(){
		//Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && gm.SprintTime > 0 || Input.GetKeyDown(KeyCode.RightShift) && gm.SprintTime > 0)
        {
            gm.sprint = true;
            Speed = 8;
        } 
		else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || gm.SprintTime <= 0)
        {
            gm.sprint = false;
            Speed = 4;
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
	}
	
	private bool checkPush(){
		//RAYCAST IN CURRENT DIRECTION
		RaycastHit2D pushHit1, pushHit2;
		Vector2 pushHit1position, pushHit2position;
		pushHit1position = new Vector2(transform.position.x, transform.position.y + 0.3f);
		pushHit2position = new Vector2(transform.position.x, transform.position.y - 0.3f);
		
		//RAYCAST LEFT
		if (isFlipped){
			pushHit1 = Physics2D.Raycast(pushHit1position, Vector2.left, 10.0f, LayerMask.GetMask("Ground"));
			pushHit2 = Physics2D.Raycast(pushHit2position, Vector2.left, 10.0f, LayerMask.GetMask("Ground"));
			
			//
			//IDEA IS THAT IF RAYCAST COLLIDE WITH WALL, STOP MOVING, ELSE IF A CRATE AND YOU'RE THE PIRATE, MOVE IT
			//
			
			if (pushHit1){
				Debug.Log(pushHit1.distance);
				
				if (pushHit1.distance < 0.6f){
					if (pushHit1.collider.CompareTag("Ground")){
						return false;
					} else {
						return Character2;
					}
				} 
			}
			
			if (pushHit2){
				//Debug.Log(pushHit.distance);
				
				if (pushHit2.distance < 0.6f){
					if (pushHit2.collider.CompareTag("Ground")){
						return false;
					} else {
						return Character2;
					}
				} 
			}
			
		//RAYCAST RIGHT
		} else {
			pushHit1 = Physics2D.Raycast(pushHit1position, Vector2.right, 10.0f, LayerMask.GetMask("Ground"));
			pushHit2 = Physics2D.Raycast(pushHit2position, Vector2.right, 10.0f, LayerMask.GetMask("Ground"));
			
			if (pushHit1){
				Debug.Log(pushHit1.distance);
				
				if (pushHit1.distance < 0.6f){
					if (pushHit1.collider.CompareTag("Ground")){
						return false;
					} else {
						return Character2;
					}
				} 
			}
			
			if (pushHit2){
				//Debug.Log(pushHit.distance);
				
				if (pushHit2.distance < 0.6f){
					if (pushHit2.collider.CompareTag("Ground")){
						return false;
					} else {
						return Character2;
					}
				} 
			}
		}
		
		return true;
	}
	
	
	void updateJumping(){
		if (isJumping)
        {
            this.gameObject.transform.parent = null;

			//Short jump
            if (!Input.GetKey(KeyCode.Space) && rb.velocity.y > 2)
            {
                rb.gravityScale = 2.7f;
            }
        }
		
		//CHECKING IF IN AIR TO RECHARGE JUMPS
		if (!checkInAir()){
			//RECHARGE
			if (Character2)
				jumpCharge = 1;
			else
				jumpCharge = 2;
		}
		
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumpCharge > 0 && characterEnabled)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
			jumpCharge-=1;
            //isJumping = true;
        }
	}
	
	private bool checkInAir(){
		//RAYCAST TO DETERMINE IF JUMPING OR NOT
		RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 10.0f, LayerMask.GetMask("Ground"));
		
		if (groundHit){
			//Debug.Log(groundHit.distance);
			
			if (groundHit.distance < 0.43f){
				isJumping = false;
				return isJumping;
			}
		}
		
		isJumping = true;
		return isJumping;
	}
	//
	//

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
	UI_manager UI;
    public bool Character2;
	public bool characterEnabled = true;
    SpriteRenderer S;
	public float delay;

	public GameObject right_slash_1;
	public GameObject right_slash_2;
	public GameObject right_slash_3;
	
	public GameObject left_slash_1;
	public GameObject left_slash_2;
	public GameObject left_slash_3;
	
	private int melee_sequence = 0;
	
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
		UI = FindObjectOfType<UI_manager>();
		Time.timeScale = 1;

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
				updateCharacterTwo();
			else
				updateCharacterOne();
		}
		
		updateJumping();

        //Testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (gm.playerHealth <= 0)
        {
			UI.InfoText.text = "Great now we are dead, hope you enjoy being stuck together for entirety";
			Time.timeScale = 0;
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

	//CHARACTER ONE ATTACK AND UPDATES
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
		//SHOOTING STANCE
        if (Input.GetKeyDown(KeyCode.F) && timeBtwAttack <= 0)
        {
			doCharacterOneShootingStance();
			/*
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                Debug.Log("Enemy damage" + enemiesToDamage[i]);
                enemiesToDamage[i].GetComponent<Patrol>().health -= damage;

            }
			*/
            timeBtwAttack = startTimeBtwAttack;
		//LOBBING GRENADE
        } else if (Input.GetKeyDown(KeyCode.G) && timeBtwAttack <= 0) {
			doCharacterOneGrenadeThrow();
		}
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
	}
	
	//SHOOTING STANCE
	void doCharacterOneShootingStance(){
		//RESET CHARACTER'S MOMENTUM
		rb.velocity = new Vector2(0,rb.velocity.y);
	}
	
	//LOBBING GRENADE
	void doCharacterOneGrenadeThrow(){
		//RESET CHARACTER'S MOMENTUM
		rb.velocity = new Vector2(0,rb.velocity.y);
	}
	
	//CHARACTER TWO ATTACK AND UPDATES
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
		//MELEE STRIKE
        if (Input.GetKeyDown(KeyCode.F) && timeBtwAttack <= 0)
        {
			doCharacterTwoMeleeSwipe();
			
			/*
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                Debug.Log("Enemy damage" + enemiesToDamage[i]);
                enemiesToDamage[i].GetComponent<Patrol>().health -= damage;

            }
            timeBtwAttack = startTimeBtwAttack;
			*/
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
	}
	
	void doCharacterTwoMeleeSwipe(){
		//RESET CHARACTER'S MOMENTUM
		rb.velocity = new Vector2(0,rb.velocity.y);
		//WIND UP ANIMATION AND VELOCITY
		
		//DO ATTACK
		if (isFlipped){
			if (melee_sequence == 0){
				StartCoroutine(gm.createAttack(rb.position, left_slash_1, new Vector2(-0.6f, 0.2f), 0.0f, 0.16f, 0.16f));
				StartCoroutine(speedTemp(0.1f,-2.0f));
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else if (melee_sequence == 1){
				StartCoroutine(gm.createAttack(rb.position, left_slash_2, new Vector2(-0.6f, 0.2f), 0.0f, 0.16f, 0.16f));
				StartCoroutine(speedTemp(0.1f,-2.0f));
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else {
				StartCoroutine(gm.createAttack(rb.position, left_slash_3, new Vector2(-1.3f, 0.2f), 0.0f, 0.16f, 0.16f));
				StartCoroutine(speedTemp(0.2f,-4.0f));
				StartCoroutine(disableTemp(0.18f));
				melee_sequence = 0;
			}
		} else {
			if (melee_sequence == 0){
				StartCoroutine(gm.createAttack(rb.position, right_slash_1, new Vector2(0.6f, 0.2f), 0.0f, 0.16f, 0.16f));
				StartCoroutine(speedTemp(0.1f,2.0f));
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else if (melee_sequence == 1){
				StartCoroutine(gm.createAttack(rb.position, right_slash_2, new Vector2(0.6f, 0.2f), 0.0f, 0.16f, 0.16f));
				StartCoroutine(speedTemp(0.1f,2.0f));
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else {
				StartCoroutine(gm.createAttack(rb.position, right_slash_3, new Vector2(1.3f, 0.2f), 0.0f, 0.16f, 0.16f));
				StartCoroutine(speedTemp(0.2f,4.0f));
				StartCoroutine(disableTemp(0.18f));
				melee_sequence = 0;
			}
		}
	}
	
	//
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
			
			rb.gravityScale = 1.0f;
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
			StartCoroutine(Key());

		}


        //Going through doors
        if (col.gameObject.CompareTag("Door") && gm.keys <= 0)
        {
			StartCoroutine(Locked());

		}

        if (col.gameObject.CompareTag("Door") && gm.keys >= 1)
        {
            gm.keys--;
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Ground") && isJumping)
        {
            //isJumping = false;
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
			StartCoroutine(Danger());
		}

        if (col.gameObject.CompareTag("End"))
        {
			UI.InfoText.text = "Time to blow this place";
			Time.timeScale = 0;
        }


        //Acquiring health kit
        if (col.gameObject.CompareTag("Health"))
        {
            gm.playerHealth += 10;
            Destroy(col.gameObject);
            Debug.Log(gm.playerHealth);
			StartCoroutine(Health());
		}
    }

	IEnumerator Locked()
	{
		UI.InfoText.text = "This is pointless, you land lover we are missing the key";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "";
	}
	IEnumerator Key()
	{
		UI.InfoText.text = "Wonder where this goes";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "";
	}
	IEnumerator Danger()
	{
		UI.InfoText.text = "Watch there lassy, too many of those encounters we be visitng Davy Jones";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "Don't call me that you drunk";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "Whatever, just be careful";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "";
	}
	IEnumerator Health()
	{
		UI.InfoText.text = "Nothing like a bottle of rum";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "You and your booze, do you ever stop chugging";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "Like you don't do the same with whatever the hell you enjoy";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "Fair Point";
		yield return new WaitForSeconds(delay);
		UI.InfoText.text = "";


	}
	
	private IEnumerator disableTemp(float seconds){
		characterEnabled = false;
		yield return new WaitForSeconds(seconds);
		characterEnabled = true;
	}
	
	private IEnumerator speedTemp(float seconds, float speed){
		rb.velocity = new Vector2(speed,rb.velocity.y);
		yield return new WaitForSeconds(seconds);
		rb.velocity = new Vector2(0,rb.velocity.y);
	}
}

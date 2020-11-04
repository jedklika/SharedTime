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
	public Sprite Modern;
	public Sprite Pirate;
	public float delay;

	//HERO GUN
	public Transform gun_position;
	public SpriteRenderer hero_gun;
	
	public bool characterShooting = false;
	public int ammo;
	public int max_ammo;
	public GameObject bulletPrefab;
	
	public int hero_gun_capacity;
	public int max_hero_gun_capacity;
	public float reload_hero_gun_rate;
	
	//C4
	public int c4_ammo;
	public int max_c4_ammo;
	public GameObject c4PrefabLeft;
	public GameObject c4PrefabRight;
	public GameObject[] currentc4s;
	public int c4_count = 0;
	public int max_c4_on_field;					//for all c4s currently set up
	
	public float c4_cooldown;
	
	//MELEE
	public Transform sword_position;
	public SpriteRenderer pirate_sword;
	
	public GameObject right_slash_1;
	public GameObject right_slash_2;
	public GameObject right_slash_3;
	
	public GameObject left_slash_1;
	public GameObject left_slash_2;
	public GameObject left_slash_3;
	
	private int melee_sequence = 0;
	
	//FLINTLOCK
	public Transform flintlock_position;
	public SpriteRenderer flintlock;
	
	public int flintlock_ammo;
	public int max_flintlock_ammo;
	public GameObject flintlockBulletPrefab;
	
	public int flintlock_capacity;
	public int max_flintlock_capacity;
	public float reload_flintlock_rate;
	
	//LADDER
	public bool onLadder = false;
	
	//ANIMATION
	public Animator PlayerAnimator;
	private int animationState = 0;
	private bool animationLock;
	
	//GAMEMANAGER
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
		S.sprite = Pirate;
		UI = FindObjectOfType<UI_manager>();
		Time.timeScale = 1;
		ammo = max_ammo;
		c4_ammo = max_c4_ammo;
	}
	
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.X) && !isJumping && !characterShooting && !onLadder)
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
		/*
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
		*/
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
			updateMovement();
		
		
		//UPDATING C4 COUNT
		
		//UPDATE ALL C4S IN GAME AND ADD TO CURRENT C4S
		currentc4s = GameObject.FindGameObjectsWithTag("C4");
		
		c4_count = currentc4s.Length;
	}
	
	private void flipWeapons(bool flip){
		if (flip){
			gun_position.localPosition = new Vector3(-Math.Abs(gun_position.localPosition.x), gun_position.localPosition.y, 0);
			hero_gun.flipX = true;
			
			sword_position.localPosition = new Vector3(-Math.Abs(sword_position.localPosition.x), sword_position.localPosition.y, 0);
			pirate_sword.flipX = true;
			
			flintlock_position.localPosition = new Vector3(-Math.Abs(flintlock_position.localPosition.x), flintlock_position.localPosition.y, 0);
			flintlock.flipX = true;
		} else {
			gun_position.localPosition = new Vector3(Math.Abs(gun_position.localPosition.x), gun_position.localPosition.y, 0);
			hero_gun.flipX = false;
			
			sword_position.localPosition = new Vector3(Math.Abs(sword_position.localPosition.x), sword_position.localPosition.y, 0);
			pirate_sword.flipX = false;
			
			flintlock_position.localPosition = new Vector3(Math.Abs(flintlock_position.localPosition.x), flintlock_position.localPosition.y, 0);
			flintlock.flipX = false;
		}
	}
	
	private void updateMovement(){
		//CHECK IF ON A LADDER
		if (!onLadder){
			if (Input.GetAxisRaw("Horizontal") < 0f)
			{
				isFlipped = S.flipX = true;
				flipWeapons(true);
				
				if (checkPush())
					rb.velocity = new Vector3(-Speed, rb.velocity.y, 0f);
				else
					rb.velocity = new Vector3(0, rb.velocity.y, 0f);
				
				//transform.localScale = new Vector3(-1f, 1f, 1f);
				
				if (gm.sprint)
					gm.reduceSprint();
					
				if (Character2)
					SetAnimation(21);
				else
					SetAnimation(1);
			}
			else if (Input.GetAxisRaw("Horizontal") > 0f)
			{
				isFlipped = S.flipX = false;
				flipWeapons(false);
				
				if (checkPush())
					rb.velocity = new Vector3(Speed, rb.velocity.y, 0f);
				else
					rb.velocity = new Vector3(0, rb.velocity.y, 0f);
				
				//transform.localScale = new Vector3(1f, 1f, 1f);
				
				if (gm.sprint)
					gm.reduceSprint();
					
				if (Character2)
					SetAnimation(21);
				else
					SetAnimation(1);
			}
			else
			{
				rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
				
				if (Character2)
					SetAnimation(20);
				else
					SetAnimation(0);
			}
		}
	}
	
	//CONTROLS AND UPDATE FUNCTIONS
	IEnumerator updateCharacterSwitch(){
		//SWITCH TO CHARACTER 2
		if (!Character2){
			characterEnabled = false;
			SetAnimation(1);
			rb.velocity = new Vector3(0, rb.velocity.y, 0f);
			yield return new WaitForSeconds(1);
			SetAnimation(21);
			characterEnabled = true;
			
			Character2 = true;
			Debug.Log("Character 2  Playing");
			
			showSword();
			unshowHeroGun();
			unshowFlintlock();
				
		//SWITCH TO CHARACTER 1
		}else{
			characterEnabled = false;
			SetAnimation(21);
			rb.velocity = new Vector3(0, rb.velocity.y, 0f);
			yield return new WaitForSeconds(1);
			SetAnimation(1);
			characterEnabled = true;
				
			Character2 = false;
			Debug.Log("Character 1  Playing");
			
			unshowSword();
			unshowHeroGun();
			unshowFlintlock();
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
		if (characterShooting){
			if (Input.GetMouseButtonDown(0) && timeBtwAttack <= 0){
				doCharacterOneShooting();
			}
			
			if (Input.GetKeyDown(KeyCode.R)){
				StartCoroutine(doCharacterOneReload());
				timeBtwAttack = startTimeBtwAttack;
			}
			
		} 
		
		if (timeBtwAttack <= 0){
			if (Input.GetKeyDown(KeyCode.F)){
				if (characterShooting){
					unshowHeroGun();
					characterShooting = false;
				} else {
					showHeroGun();
					characterShooting = true;
				}
				
				timeBtwAttack = startTimeBtwAttack;
			//LOBBING GRENADE
			} else if (Input.GetKeyDown(KeyCode.G)) {
				if (characterShooting){
					unshowHeroGun();
					characterShooting = false;
				}
				
				doCharacterOneC4();
			} else if (Input.GetKeyDown(KeyCode.H)) {
				doCharacterOneActivateC4();
			}
		} else {
			timeBtwAttack -= Time.deltaTime;
		}
	}
	
	void showHeroGun(){
		hero_gun.color = Color.white;
	}
	
	void unshowHeroGun(){
		hero_gun.color = Color.clear;
	}
	
	//SHOOTING STANCE
	void doCharacterOneShooting(){
		//REQUIREMENTS
		if (hero_gun_capacity > 0){
			//GET MOUSE POSITION
			Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 target_position = new Vector2(mouse.x, mouse.y);
			
			Debug.Log(target_position);
			
			if (isFlipped){
				StartCoroutine(gm.createMovingAttack(rb.position, bulletPrefab, new Vector2(-0.6f, 0.1f), 0.0f, target_position, 0.05f, 1.1f));
				rb.AddForce(Vector2.right * 7.0f, ForceMode2D.Impulse);
				disableTemp(0.12f);
			} else {
				StartCoroutine(gm.createMovingAttack(rb.position, bulletPrefab, new Vector2(0.6f, 0.1f), 0.0f, target_position, 0.05f, 1.1f));
				rb.AddForce(Vector2.left * 7.0f, ForceMode2D.Impulse);
				disableTemp(0.12f);
			}
			
			hero_gun_capacity-=1;
			
			timeBtwAttack = startTimeBtwAttack;
		}
	}
	
	IEnumerator doCharacterOneReload(){
		yield return new WaitForSeconds(reload_hero_gun_rate);
		
		//GET CURRENT AMMO AMOUNT NEEDED FOR RELOAD
		int test_use = ammo - (max_hero_gun_capacity - hero_gun_capacity);
		
		if (test_use >= 0){
			ammo = test_use;
			hero_gun_capacity = max_hero_gun_capacity;
		} else {
			//EMPTYING ALL CURRENT AMMO
			hero_gun_capacity = ammo;
			ammo = 0;
		}
	}
	
	//LOBBING C4
	void doCharacterOneC4(){
		//CHECK REQUIREMENTS
		if (c4_ammo > 0 && c4_count < max_c4_on_field){
			//RESET CHARACTER'S MOMENTUM
			rb.velocity = new Vector2(0,rb.velocity.y);
			
			//THROW A C4 BASED ON FLIP
			if (isFlipped){
				StartCoroutine(gm.createTrapAttack(rb.position, c4PrefabLeft, new Vector2(-0.6f, 0.1f), 0.0f, 0.05f));
				disableTemp(0.08f);
			} else {
				StartCoroutine(gm.createTrapAttack(rb.position, c4PrefabRight, new Vector2(0.6f, 0.1f), 0.0f, 0.05f));
				rb.AddForce(Vector2.left * 7.0f, ForceMode2D.Impulse);
				disableTemp(0.08f);
			}
			
			c4_ammo-=1;
			
			timeBtwAttack = startTimeBtwAttack;
		}
	}
	
	void doCharacterOneActivateC4(){
		//BLOW UP THE NEXT AVAILABLE C4 IF ANY
		if (c4_count > 0){
			currentc4s[0].transform.GetChild(0).gameObject.GetComponent<trapDetect>().activateTrap();
		}
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
		//SHOOTING STANCE
		if (characterShooting){
			if (Input.GetMouseButtonDown(0) && timeBtwAttack <= 0){
				doCharacterTwoShooting();
			}
			
			if (Input.GetKeyDown(KeyCode.R)){
				StartCoroutine(doCharacterTwoReload());
				timeBtwAttack = startTimeBtwAttack;
			}
		} 
		
		if (timeBtwAttack <= 0){
			if (Input.GetKeyDown(KeyCode.F)){
				if (!characterShooting){
					unshowSword();
					showFlintlock();
				
					characterShooting = true;
				} else {
					showSword();
					unshowFlintlock();
				
					characterShooting = false;
				}
				
				timeBtwAttack = startTimeBtwAttack;
			//LOBBING GRENADE
			} else if (Input.GetKeyDown(KeyCode.G)) {
				if (characterShooting){
					showSword();
					unshowFlintlock();
				
					characterShooting = false;
				}
				
				doCharacterTwoMeleeSwipe();
			}
		} else {
			timeBtwAttack -= Time.deltaTime;
		}
	}
	
	void showFlintlock(){
		flintlock.color = Color.white;
	}
	
	void unshowFlintlock(){
		flintlock.color = Color.clear;
	}
	
	void doCharacterTwoShooting(){
		//REQUIREMENTS
		if (flintlock_capacity > 0){
			//GET MOUSE POSITION
			Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 target_position = new Vector2(mouse.x, mouse.y);
			
			Debug.Log(target_position);
			
			if (isFlipped){
				StartCoroutine(gm.createMovingAttack(rb.position, flintlockBulletPrefab, new Vector2(-0.6f, 0.1f), 0.0f, target_position, 0.05f, 1.1f));
				rb.AddForce(Vector2.right * 7.0f, ForceMode2D.Impulse);
				disableTemp(0.12f);
			} else {
				StartCoroutine(gm.createMovingAttack(rb.position, flintlockBulletPrefab, new Vector2(0.6f, 0.1f), 0.0f, target_position, 0.05f, 1.1f));
				rb.AddForce(Vector2.left * 7.0f, ForceMode2D.Impulse);
				disableTemp(0.12f);
			}
			
			flintlock_capacity-=1;
			
			timeBtwAttack = startTimeBtwAttack;
		}
	}
	
	IEnumerator doCharacterTwoReload(){
		yield return new WaitForSeconds(reload_flintlock_rate);
		
		//GET CURRENT AMMO AMOUNT NEEDED FOR RELOAD
		int test_use = flintlock_ammo - (max_flintlock_capacity - flintlock_capacity);
		
		if (test_use >= 0){
			flintlock_ammo = test_use;
			flintlock_capacity = max_flintlock_capacity;
		} else {
			//EMPTYING ALL CURRENT AMMO
			flintlock_capacity = flintlock_ammo;
			flintlock_ammo = 0;
		}
	}
	
	void showSword(){
		pirate_sword.color = Color.white;
	}
	
	void unshowSword(){
		pirate_sword.color = Color.clear;
	}
	
	void doCharacterTwoMeleeSwipe(){
		//RESET CHARACTER'S MOMENTUM
		rb.velocity = new Vector2(0,rb.velocity.y);
		//WIND UP ANIMATION AND VELOCITY
		
		//DO ATTACK
		if (isFlipped){
			if (melee_sequence == 0){
				StartCoroutine(gm.createAttack(rb.position, left_slash_1, new Vector2(-0.6f, 0.2f), 0.0f, 0.11f, 0.16f));
				rb.AddForce(Vector2.left * 2.0f, ForceMode2D.Impulse);
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else if (melee_sequence == 1){
				StartCoroutine(gm.createAttack(rb.position, left_slash_2, new Vector2(-0.6f, 0.2f), 0.0f, 0.11f, 0.16f));
				rb.AddForce(Vector2.left * 2.0f, ForceMode2D.Impulse);
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else {
				StartCoroutine(gm.createAttack(rb.position, left_slash_3, new Vector2(-1.3f, 0.2f), 0.0f, 0.11f, 0.16f));
				rb.AddForce(Vector2.left * 4.0f, ForceMode2D.Impulse);
				StartCoroutine(disableTemp(0.18f));
				melee_sequence = 0;
				timeBtwAttack = startTimeBtwAttack/2;
			}
		} else {
			if (melee_sequence == 0){
				StartCoroutine(gm.createAttack(rb.position, right_slash_1, new Vector2(0.6f, 0.2f), 0.0f, 0.11f, 0.16f));
				rb.AddForce(Vector2.right * 2.0f, ForceMode2D.Impulse);
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else if (melee_sequence == 1){
				StartCoroutine(gm.createAttack(rb.position, right_slash_2, new Vector2(0.6f, 0.2f), 0.0f, 0.11f, 0.16f));
				rb.AddForce(Vector2.right * 2.0f, ForceMode2D.Impulse);
				StartCoroutine(disableTemp(0.18f));
				melee_sequence++;
			} else {
				StartCoroutine(gm.createAttack(rb.position, right_slash_3, new Vector2(1.3f, 0.2f), 0.0f, 0.11f, 0.16f));
				rb.AddForce(Vector2.right * 4.0f, ForceMode2D.Impulse);
				StartCoroutine(disableTemp(0.18f));
				melee_sequence = 0;
				timeBtwAttack = startTimeBtwAttack/2;
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
				//Debug.Log(pushHit1.distance);
				
				if (pushHit1.distance < 0.5f){
					if (pushHit1.collider.CompareTag("Ground") || pushHit1.collider.CompareTag("explodableDoor")){
						return false;
					} else if (pushHit1.collider.CompareTag("Door")){
						return gm.keys > 0;
					} else {
						return Character2;
					}
				} 
			}
			
			if (pushHit2){
				//Debug.Log(pushHit.distance);
				
				if (pushHit2.distance < 0.5f){
					if (pushHit2.collider.CompareTag("Ground") || pushHit1.collider.CompareTag("explodableDoor")){
						return false;
					} else if (pushHit1.collider.CompareTag("Door")){
						return gm.keys > 0;
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
				//Debug.Log(pushHit1.distance);
				
				if (pushHit1.distance < 0.5f){
					if (pushHit1.collider.CompareTag("Ground")){
						return false;
					} else {
						return Character2;
					}
				} 
			}
			
			if (pushHit2){
				//Debug.Log(pushHit.distance);
				
				if (pushHit2.distance < 0.5f){
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
			
			rb.gravityScale = 1.2f;
		}
		
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumpCharge > 0 && characterEnabled)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
			jumpCharge-=1;
            //isJumping = true;
			rb.gravityScale = 1.2f;
			
			if (onLadder){
				onLadder = false;
			}
        }
		
		if (onLadder)
			rb.gravityScale = 0.0f;
	}
	
	private bool checkInAir(){
		//RAYCAST TO DETERMINE IF JUMPING OR NOT
		RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 10.0f, LayerMask.GetMask("Ground"));
		
		if (groundHit){
			//Debug.Log(groundHit.distance);
			
			if (groundHit.distance < 0.50f){
				isJumping = false;
				return isJumping;
			}
		}
		if (onLadder)
		{
			isJumping = false;
			return isJumping;
		}
		isJumping = true;
		
		return isJumping;
	}
	//
	//
	public void snapXToPosition(float x_position){
		rb.MovePosition(new Vector2(x_position,rb.position.y));
	}
	
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
            //Debug.Log("Check");
            this.gameObject.transform.parent = null;
            if (isJumping == true)
            {
                this.gameObject.transform.parent = null;
            }
        }
		
        if (col.gameObject.CompareTag("LowDanger"))
        {
            gm.playerHealth -= 20;
            //Debug.Log(gm.playerHealth);
			StartCoroutine(Danger());
		}
		
		if (col.gameObject.CompareTag("HighDanger"))
        {
            gm.playerHealth -= 50;
            //Debug.Log(gm.playerHealth);
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
            //Debug.Log(gm.playerHealth);
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
	
	
	//FOR ANIMATIONS
	private IEnumerator tempAnimLock(float seconds){
		animationLock = true;
		yield return new WaitForSeconds(seconds);
		animationLock = false;
	}
	
	private void changeAnimation(string animationName){
		if (!animationLock){
			PlayerAnimator.Play(animationName);
		}
	}
	
	public void SetAnimation(int animationNumber){
		switch (animationNumber){
			case 0:
				SetToHeroIdle();
			break;
			case 1:
				SetToHeroWalk();
			break;
			case 2:
				//SetToHeroRun();
			break;
			case 4:
				SetToHeroClimb();
			break;
			case 20:
				SetToPirateIdle();
			break;
			case 21:
				SetToPirateWalk();
			break;
			case 24:
				SetToPirateClimb();
			break;
		}
	}
	
	void SetToHeroIdle()
	{
		animationState = 0;
		changeAnimation("hero_standing");
	}
	
	void SetToHeroWalk()
	{
		animationState = 1;
		PlayerAnimator.Play("hero_walking");
	}
	
	void SetToHeroClimb()
	{
		
	}
	
	void SetToPirateIdle()
	{
		animationState = 20;
		changeAnimation("pirate_standing");
	}
	
	void SetToPirateWalk()
	{
		animationState = 21;
		PlayerAnimator.Play("pirate_walking");
	}
	
	void SetToPirateClimb()
	{
	
	}
}

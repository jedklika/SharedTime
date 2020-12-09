using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public float playerHealth = 100;
	public float playerMaxHealth = 100;
	
	public float SprintTime = 30;
	public bool sprint;
	private bool exhausted = false;
	public float keys;
	public bool canBeDamaged = true;
	
	UI_manager ui;
	
	// Start is called before the first frame update
	void Start()
	{
		ui = FindObjectOfType<UI_manager>();
	}

	// Update is called once per frame
	void Update()
	{
		ui.updateHPBar((float)playerHealth/(float)playerMaxHealth);
		updateSprint();
	}
	
	public bool CanSprint()
	{
		//Recover a bit if exhausted
		if (exhausted)
		{
			if (SprintTime > 10.0f)
				exhausted = false;

			return false;
		}
		else
		{
			if (SprintTime < 1.0f)
				exhausted = true;

			return true;
		}
	}
	
	private void updateSprint(){
		//ONLY INCREASE THIS WHEN NOT SPRINTING
		if (!sprint && SprintTime < 30){
			SprintTime += Time.deltaTime * 2.0f;
			
			if (SprintTime > 30)
				SprintTime = 30;
		}
		
		ui.updateSprintBar((float)SprintTime/30.0f);
	}
	
	public void reduceSprint(){
		SprintTime -= Time.deltaTime * 3.5f;
	}
		
	//Managing health
	public void setDamage(float new_damage)
	{
		
		//Checking bounds
		//Max
		if (playerHealth > playerMaxHealth)
		{
			playerHealth = playerMaxHealth;
		//Min									--The player dies if this reaches 0
		} 
		else if (playerHealth <= 0)
		{
			playerHealth = 0;
		}
		
		//Flashing and sounds
		//Damage	(make sure not invincible)
		if (new_damage > 0 && canBeDamaged)
		{
			playerHealth-=new_damage;
			StartCoroutine(setInvincible());
		} 
		else if (new_damage < 0)
		{
			playerHealth-=new_damage;
		}
	}
	
	IEnumerator setInvincible()
	{
		canBeDamaged = false;
		
		yield return new WaitForSeconds(1.5f);
		
		canBeDamaged = true;
	}
	
	///SYSTEM FOR CREATING HITBOXES
	//THE SPRITE OF A HITBOX IS MANAGED BY PLAYER OR ENEMIES
	
	//HITBOX ITSELF
	//WITH MOVEMENT
	public IEnumerator createMovingAttack(Vector2 user_position, GameObject prefab, Vector2 position, float rotation, Vector2 target, float delay_before_creating, float delay_before_killing){
		yield return new WaitForSeconds(delay_before_creating);
		//CREATE AN EMPTY HITBOX PREFAB (SO IF ANY OF THE HITBOXES TOUCH AN ENEMY, THE MAIN HIT,BOX SCRIPT IS FIRED)
		GameObject hitbox_prefab = Instantiate(prefab, user_position, new Quaternion(0,0,rotation, 1));

		hitbox_prefab.transform.Translate(position);
		
		//GET HITBOX SCRIPT AND CHANGE TARGET
		hitbox_prefab.GetComponent<hitboxBase>().setTarget(target);
		
		Destroy(hitbox_prefab, delay_before_killing);
	}
	
	//WITHOUT SPEED
	public IEnumerator createAttack(Vector2 user_position, GameObject prefab, Vector2 position, float rotation, float delay_before_creating, float delay_before_killing){
		yield return new WaitForSeconds(delay_before_creating);
		//CREATE AN EMPTY HITBOX PREFAB (SO IF ANY OF THE HITBOXES TOUCH AN ENEMY, THE MAIN HIT,BOX SCRIPT IS FIRED)
		GameObject hitbox_prefab = Instantiate(prefab, user_position, new Quaternion(0,0,rotation, 1));

		hitbox_prefab.transform.Translate(position);
		
		Destroy(hitbox_prefab, delay_before_killing);
	}
	
	//WITHOUT TIME
	public IEnumerator createTrapAttack(Vector2 user_position, GameObject prefab, Vector2 position, float rotation, float delay_before_creating){
		yield return new WaitForSeconds(delay_before_creating);
		//CREATE AN EMPTY HITBOX PREFAB (SO IF ANY OF THE HITBOXES TOUCH AN ENEMY, THE MAIN HIT,BOX SCRIPT IS FIRED)
		GameObject hitbox_prefab = Instantiate(prefab, user_position, new Quaternion(0,0,rotation, 1));

		hitbox_prefab.transform.Translate(position);
	}
}

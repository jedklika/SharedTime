using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public float playerHealth = 90;
	public float playerMaxHealth = 100;

	public float SprintTime = 30;
	public bool sprint;
	private bool exhausted = false;
	public float keys;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
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
	}
	
	public void reduceSprint(){
		SprintTime -= Time.deltaTime * 2;
	}
		
	public void setDamage(float new_damage)
	{
		playerHealth -= new_damage;

	}
}

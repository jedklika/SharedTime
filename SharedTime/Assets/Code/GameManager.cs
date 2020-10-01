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
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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
	public void setDamage(float new_damage)
	{
		playerHealth -= new_damage;

	}
}

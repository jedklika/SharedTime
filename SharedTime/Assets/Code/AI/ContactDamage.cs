using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
	public int damageDealt;

	GameManager gm;

	// Start is called before the first frame update
	void Start()
	{
		gm = FindObjectOfType<GameManager>();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//Check if the player was hit
		if (col.gameObject.CompareTag("Player"))
		{
			gm.setDamage(damageDealt);
		}
	}
}

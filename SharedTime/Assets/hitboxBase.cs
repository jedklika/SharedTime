using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class hitboxBase : MonoBehaviour
{
	public int damageDealt;
	private List<int> enemiesToDamage;
	
    // Start is called before the first frame update
    void Start()
    {
        enemiesToDamage = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public int dealDamage(int enemy_id){
		//CHECK IF ENEMY ID IS IN BASE
		if (enemiesToDamage.Exists(x => x == enemy_id)){
			return 0;
		} else {
			enemiesToDamage.Add(enemy_id);
			Debug.Log("Do ouch for " + damageDealt + " damage!");
			return damageDealt;
		}
	}
}

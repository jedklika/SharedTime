using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDetect : MonoBehaviour
{
	hitboxBase base_hitbox;
	
    // Start is called before the first frame update
    void Start()
    {
        //GET BASE HITBOX FROM PARENT
		base_hitbox = gameObject.GetComponentInParent<hitboxBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col){
		Debug.Log(col.tag);
		
		if (col.CompareTag("Enemy")){
			//GET THE PATROL SCRIPT
			Patrol enemyScript = col.GetComponent<Patrol>();
			
			//MAKING SURE IT ACTUALLY EXISTS
			if (enemyScript != null){
				int damage_result = base_hitbox.dealDamage(enemyScript.enemyID);
				
				//GET ID AND DEAL DAMAGE TO THEM
				enemyScript.TakeDamage(damage_result);
			}
		}
	}
}

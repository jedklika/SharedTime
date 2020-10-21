using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapDetect : MonoBehaviour
{
    trapBase base_trap;
	GameManager gm;
	
    // Start is called before the first frame update
    void Start()
    {
		gm = FindObjectOfType<GameManager>();
    
        //GET BASE HITBOX FROM PARENT
		base_trap = gameObject.GetComponentInParent<trapBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col){
		//Debug.Log(col.tag);
		
		if (col.CompareTag("Enemy")){
			activateTrap();
		}
	}
	
	public void activateTrap(){
		if (!base_trap.trapActivated){
			//START THE COROUTINE WHEN ACTIVATING THIS TRAP
			StartCoroutine(gm.createAttack(transform.position, base_trap.trapTrigger, new Vector2(0.0f, 0.7f), 0.0f, 0.04f, 0.6f));
			
			//DELETE AT END
			//
			
			base_trap.trapActivated = true;
			
			//START DELETION
			StartCoroutine(base_trap.delayDelete(0.1f));
		}
	}
	
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Changer : MonoBehaviour
{
	Enemy_Manager em;
	
	public int enemy_set_to_set_active;
	
    // Start is called before the first frame update
    void Start()
    {
        em = FindObjectOfType<Enemy_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")){
			em.setEnemiesActive(enemy_set_to_set_active);
		}
	}
}

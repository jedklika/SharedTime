using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
	public GameObject[][] enemy_sets;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void setEnemiesActive(int enemy_set){
		set_enemies_inactive();
	}
	
	private void set_enemies_inactive(){
		
	}
}

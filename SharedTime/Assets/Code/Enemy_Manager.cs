using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
	public List<GameObject> enemy_room_1;
	public List<GameObject> enemy_room_2;
	public List<GameObject> enemy_room_3;
	
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
		
		switch (enemy_set){
			case 0:
				set_enemy_room_one_active();
			break;
			case 1:
				set_enemy_room_two_active();
			break;
			case 2:
				set_enemy_room_three_active();
			break;
		}
	}
	
	private void set_enemies_inactive(){
		//SETTING ENEMIES IN ALL ROOMS INACTIVE
		int i;
		
		foreach (GameObject enemy in enemy_room_1){
			if (enemy != null)
				enemy.SetActive(false);
		}
		
		foreach (GameObject enemy in enemy_room_2){
			if (enemy != null)
				enemy.SetActive(false);
		}
		
		foreach (GameObject enemy in enemy_room_3){
			if (enemy != null)
				enemy.SetActive(false);
		}
	}
	
	void set_enemy_room_one_active(){
		foreach (GameObject enemy in enemy_room_1){
			if (enemy != null)
				enemy.SetActive(true);
		}
	}
	
	void set_enemy_room_two_active(){
		foreach (GameObject enemy in enemy_room_2){
			if (enemy != null)
				enemy.SetActive(true);
		}
	}
	
	void set_enemy_room_three_active(){
		foreach (GameObject enemy in enemy_room_3){
			if (enemy != null)
				enemy.SetActive(true);
		}
	}
}

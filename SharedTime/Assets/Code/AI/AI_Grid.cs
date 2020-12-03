using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Grid : MonoBehaviour
{
	public Transform playerPosition;
	
	bool is_suspicious = false;
	public SpriteRenderer question;
	
	//USE THIS FOR AI PATHFINDING
	public BoxCollider2D patrolField;
	public float field_cut_rate;
	
	//THESE SHOULD BE CREATED FROM PATROL FIELD
	private int[,] patrol_grid;   //INT VALUES ARE THE COLORS
	private int patrol_grid_x_size;
	private int patrol_grid_y_size;
	
	private int self_index_x;
	private int self_index_y;
	
	Patrol P;
	Attack A;
	
    // Start is called before the first frame update
    void Start()
    {
        patrol_grid = new int[(int)(patrolField.size.x/field_cut_rate),(int)(patrolField.size.y/field_cut_rate)];
		
		patrol_grid_x_size = (int)(patrolField.size.x/field_cut_rate);
		patrol_grid_y_size = (int)(patrolField.size.y/field_cut_rate);
		//Debug.Log(patrol_grid_x_size);
		//Debug.Log(patrol_grid_y_size);
		
		P = GetComponent<Patrol>();
		A = GetComponent<Attack>();
		
		A.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (updatePatrolGrid()){
			case 1:
				//Debug.Log("Suspicious");
				if (!is_suspicious)
					StartCoroutine(suspicious());
			break;
			case 2:
				//Debug.Log("Spotted");
				P.enabled = false;
				A.enabled = true;
			break;
			default:
				//P.enabled = true;
				
				if (A.enabled){
					//CANCEL ATTACK IF FAR ENOUGH
					float player_distance = Vector2.Distance(gameObject.transform.position, playerPosition.position);
					
					if (player_distance > 8f){
						P.enabled = true;
						A.enabled = false;
					}
				}
				
			break;
		}
		
		if (P.enabled)
			P.updateMove();
		else if (A.enabled)
			A.updateAttack();
    }
	
	IEnumerator suspicious(){
		//SET THE QUESTION MARK ON THEN OFF
		question.color = Color.white;
		is_suspicious = true;
		yield return new WaitForSeconds(2.0f);
		question.color = Color.clear;
		is_suspicious = false;
	}
	
	public int updatePatrolGrid(){
		float patrol_grid_draw_x_iteration = 0, patrol_grid_draw_y_iteration = 0;
		int x_index = 0, y_index = 0;
		int patrol_flag = 0;
		
		for (;x_index < patrol_grid_x_size; x_index++){
			for (;y_index < patrol_grid_y_size; y_index++){
				float x_spot, y_spot;
				x_spot = patrol_grid_draw_x_iteration + patrolField.transform.position.x - patrolField.size.x/2;
				y_spot = patrol_grid_draw_y_iteration + patrolField.transform.position.y - patrolField.size.y/2;
				
				//CHECK FOR GROUND BELOW
				if (checkForGround(x_spot, y_spot)){
					patrol_grid[x_index, y_index] = 10;
				} else if (checkForEnemies(x_spot, y_spot)){
					patrol_grid[x_index, y_index] = 20;
				} else if (checkForPlayer(x_spot, y_spot)){
					if (patrol_grid[x_index, y_index] == 22){
						patrol_flag = 1;
					} else if (patrol_grid[x_index, y_index] == 21){
						patrol_flag = 2;
					}
					
					patrol_grid[x_index, y_index] = 30;
				} else {
					patrol_grid[x_index, y_index] = 0;
				}
				
				if (checkForSelf(x_spot, y_spot, x_index, y_index)){
					//DUNNO WHAT TO PUT HERE, BUT IN CASE OF SOME IMPLEMENTATION, THIS IS LEFT OPEN
					
				}
				
				patrol_grid_draw_y_iteration+=field_cut_rate;
			}
			
			patrol_grid_draw_y_iteration = 0;
			y_index = 0;
			
			patrol_grid_draw_x_iteration+=field_cut_rate;
		}
		
		return patrol_flag;
	}
	
	
	void OnDrawGizmos(){
		float patrol_grid_draw_x_iteration = 0, patrol_grid_draw_y_iteration = 0;
		int x_index = 0, y_index = 0;
		
		for (;x_index < patrol_grid_x_size; x_index++){
			for (;y_index < patrol_grid_y_size; y_index++){
				float x_spot, y_spot;
				x_spot = patrol_grid_draw_x_iteration + patrolField.transform.position.x - patrolField.size.x/2;
				y_spot = patrol_grid_draw_y_iteration + patrolField.transform.position.y - patrolField.size.y/2;
				
				//CHECK FOR GROUND BELOW
				if (patrol_grid[x_index, y_index] == 10){
					Gizmos.color = new Color(0f,0f,0f,0.5f);
				} else if (patrol_grid[x_index, y_index] == 20){
					Gizmos.color = new Color(1f,0f,0f,0.5f);
					//Debug.Log("Enemy Spot: " + x_index + " " + y_index);
				} else if (patrol_grid[x_index, y_index] == 21){
					Gizmos.color = new Color(1f,1f,0f,0.5f);
				} else if (patrol_grid[x_index, y_index] == 22){
					Gizmos.color = new Color(0.5f,0.5f,0.5f,0.5f);
				} else if (patrol_grid[x_index, y_index] == 30){
					Gizmos.color = new Color(0f,1f,0f,0.5f);
					//Debug.Log("Player Spot: " + x_index + " " + y_index);
				} else {
					Gizmos.color = new Color(0f,0f,1f,0.5f);
				}
				
				Gizmos.DrawCube(new Vector3(x_spot, y_spot, 0), new Vector3(field_cut_rate*0.9f,field_cut_rate*0.9f,1.0f));
				
				patrol_grid_draw_y_iteration+=field_cut_rate;
			}
			
			patrol_grid_draw_y_iteration = 0;
			y_index = 0;
			
			patrol_grid_draw_x_iteration+=field_cut_rate;
		}
	}
	
	
	bool checkForGround(float x_to_raycast, float y_to_raycast){
		Vector2 ground_spot = new Vector2(x_to_raycast, y_to_raycast);
		RaycastHit2D check = Physics2D.Raycast(ground_spot, Vector2.zero, 0.0f, LayerMask.GetMask("Ground"));
		
		return (check.collider != null);
	}
	
	bool checkForEnemies(float x_to_raycast, float y_to_raycast){
		Vector2 ground_spot = new Vector2(x_to_raycast, y_to_raycast);
		RaycastHit2D check = Physics2D.Raycast(ground_spot, Vector2.zero, 0.0f, LayerMask.GetMask("Enemies"));
		
		if (check.collider != null){
			if (check.collider.CompareTag("Enemy")){
				return true;
			}
		}
		
		return false;
	}
	
	//THIS WORKS ABOUT THE SAME AS CHECKING FOR SELF, BUT APPLIED TO PLAYER
	bool checkForPlayer(float x_to_raycast, float y_to_raycast){
		Vector2 ground_spot = new Vector2(x_to_raycast, y_to_raycast);
		RaycastHit2D check = Physics2D.Raycast(ground_spot, Vector2.zero, 0.0f, LayerMask.GetMask("Default"));
		
		if (check.collider != null){
			return (check.collider.CompareTag("Player"));
		} 
		
		return false;
	}
	
	bool checkForSelf(float x_to_spot, float y_to_spot, int x_index, int y_index){
		if ((transform.position.x > x_to_spot - field_cut_rate/2 && transform.position.x < x_to_spot + field_cut_rate/2) &&
			(transform.position.y > y_to_spot - field_cut_rate/2 && transform.position.y < y_to_spot + field_cut_rate/2)){
			self_index_x = x_index;
			self_index_y = y_index;
			
			//Debug.Log(x_index + " " + y_index);
			
			return true;
		}
		
		return false;
	}
	
	public bool get_valid_spot_relative_to_AI(int x_offset, int y_offset){
		return patrol_grid[self_index_x+x_offset,self_index_y+y_offset] != 10;
	}
	
	public bool get_filled_spot_relative_to_AI(int x_offset, int y_offset){
		return patrol_grid[self_index_x+x_offset,self_index_y+y_offset] == 10;
	}
	
	public void updateSight(int strength, int notice, bool flipped){
		StartCoroutine(updateSight(self_index_x, self_index_y, 0, strength, notice, flipped));
	}
	
	IEnumerator updateSight(int sight_index_x, int sight_index_y, int growth, int strength, int notice, bool flipped){
		yield return new WaitForSeconds(0.001f);
		
		if (sight_index_x >= 1 &&
			sight_index_x < patrol_grid_x_size-1 &&
			sight_index_y >= 1 &&
			sight_index_y < patrol_grid_y_size-1 &&
			strength > 0){
				
			if (flipped){
				if (patrol_grid[sight_index_x,sight_index_y] != 10){
					if (strength > notice)
						patrol_grid[sight_index_x,sight_index_y] = 21;
					else
						patrol_grid[sight_index_x,sight_index_y] = 22;
					
					StartCoroutine(updateSight(sight_index_x-1, sight_index_y, growth+1, strength-1, notice, flipped));
					
					if (growth == 1){
						StartCoroutine(updateSight(sight_index_x-1, sight_index_y-1, 0, strength-1, notice, flipped));
						StartCoroutine(updateSight(sight_index_x-1, sight_index_y+1, 0, strength-1, notice, flipped));
					}
				}
			} else {
				if (patrol_grid[sight_index_x,sight_index_y] != 10){
					if (strength > notice)
						patrol_grid[sight_index_x,sight_index_y] = 21;
					else
						patrol_grid[sight_index_x,sight_index_y] = 22;
						
					StartCoroutine(updateSight(sight_index_x+1, sight_index_y, growth+1, strength-1, notice, flipped));
					
					if (growth == 1){
						StartCoroutine(updateSight(sight_index_x+1, sight_index_y-1, 0, strength-1, notice, flipped));
						StartCoroutine(updateSight(sight_index_x+1, sight_index_y+1, 0, strength-1, notice, flipped));
					}
					
				}
			}	
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    //[SerializeField]

    public float speed;
    public bool canFlip;
    public bool FoeFlipped;
    public int health;
    Rigidbody2D Rb;
    int waypointIndex = 0;
	public SpriteRenderer S;
	
	public int enemyID;

	public Transform playerPosition;
	
	//USE THIS FOR AI PATHFINDING
	public BoxCollider2D patrolField;
	public float field_cut_rate;
	
	//THESE SHOULD BE CREATED FROM PATROL FIELD
	private int[,] patrol_grid;   //INT VALUES ARE THE COLORS
	private int patrol_grid_x_size;
	private int patrol_grid_y_size;
	
	private int self_index_x;
	private int self_index_y;
	
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
		
		patrol_grid = new int[(int)(patrolField.size.x/field_cut_rate),(int)(patrolField.size.y/field_cut_rate)];
		patrol_grid_x_size = (int)(patrolField.size.x/field_cut_rate);
		patrol_grid_y_size = (int)(patrolField.size.y/field_cut_rate);
		//Debug.Log(patrol_grid_x_size);
		//Debug.Log(patrol_grid_y_size);
    }

    // Update is called once per frame
    void Update()
    {
		updatePatrolGrid();
		updateMove();
        
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
	
	void updatePatrolGrid(){
		float patrol_grid_draw_x_iteration = 0, patrol_grid_draw_y_iteration = 0;
		int x_index = 0, y_index = 0;
		
		for (;x_index < patrol_grid_x_size; x_index++){
			for (;y_index < patrol_grid_y_size; y_index++){
				float x_spot, y_spot;
				x_spot = patrol_grid_draw_x_iteration + patrolField.transform.position.x - patrolField.size.x/2;
				y_spot = patrol_grid_draw_y_iteration + patrolField.transform.position.y - patrolField.size.y/2;
				
				//CHECK FOR GROUND BELOW
				if (checkForGround(x_spot, y_spot)){
					patrol_grid[x_index, y_index] = 1;
				} else if (checkForEnemies(x_spot, y_spot)){
					patrol_grid[x_index, y_index] = 2;
				} else if (checkForPlayer(x_spot, y_spot)){
					patrol_grid[x_index, y_index] = 3;
				} else {
					patrol_grid[x_index, y_index] = 0;
				}
				
				checkForSelf(x_spot, y_spot, x_index, y_index);
				
				patrol_grid_draw_y_iteration+=field_cut_rate;
			}
			
			patrol_grid_draw_y_iteration = 0;
			y_index = 0;
			
			patrol_grid_draw_x_iteration+=field_cut_rate;
		}
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
				if (checkForGround(x_spot, y_spot)){
					Gizmos.color = new Color(0f,0f,0f,0.5f);
				} else if (checkForEnemies(x_spot, y_spot)){
					Gizmos.color = new Color(1f,0f,0f,0.5f);
					//Debug.Log("Enemy Spot: " + x_index + " " + y_index);
				} else if (checkForPlayer(x_spot, y_spot)){
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
	
	void checkForSelf(float x_to_spot, float y_to_spot, int x_index, int y_index){
		if ((transform.position.x > x_to_spot - field_cut_rate/2 && transform.position.x < x_to_spot + field_cut_rate/2) &&
			(transform.position.y > y_to_spot - field_cut_rate/2 && transform.position.y < y_to_spot + field_cut_rate/2)){
			self_index_x = x_index;
			self_index_y = y_index;
			
			Debug.Log(x_index + " " + y_index);
		}
	}
	
	void updateMove(){
		if (FoeFlipped){
			//GO LEFT
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 5f, transform.position.y), speed*Time.deltaTime);
			
			if (patrol_grid[self_index_x-2,self_index_y-2] != 1 ||
				patrol_grid[self_index_x-2,self_index_y+1] == 1)
				FoeFlipped = S.flipX = false;
			
		} else {
			//GO RIGHT
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 5f, transform.position.y), speed*Time.deltaTime);
			
			if (patrol_grid[self_index_x+2,self_index_y-2] != 1 ||
				patrol_grid[self_index_x+2,self_index_y+1] == 1)
				FoeFlipped = S.flipX = true;
		}
	}
   
   public void TakeDamage(int damage)
   {
        health -= damage;

        //Check if dead
        if (health <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
   }
}

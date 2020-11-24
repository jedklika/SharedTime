using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
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
	public float x_field_offset, y_field_offset;
	
	//THESE SHOULD BE CREATED FROM PATROL FIELD
	private int[,] patrol_grid;   //INT VALUES ARE THE COLORS
	private int patrol_grid_x_size;
	private int patrol_grid_y_size;
	
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        transform.position = waypoints[waypointIndex].transform.position;
		patrol_grid = new int[(int)(patrolField.size.x/field_cut_rate),(int)(patrolField.size.y/field_cut_rate)];
		patrol_grid_x_size = (int)(patrolField.size.x/field_cut_rate);
		patrol_grid_y_size = (int)(patrolField.size.y/field_cut_rate);
		//Debug.Log(patrol_grid_x_size);
		//Debug.Log(patrol_grid_y_size);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);
        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
	
	void updatePatrolGrid(){
		
	}
	
	void OnDrawGizmos(){
		int patrol_grid_draw_x_iteration = -patrol_grid_x_size/2, patrol_grid_draw_y_iteration = -patrol_grid_y_size/2;
		int x_index = 0, y_index = 0;
		
		for (;patrol_grid_draw_x_iteration < patrol_grid_x_size/2; patrol_grid_draw_x_iteration++){
			for (;patrol_grid_draw_y_iteration < patrol_grid_y_size/2; patrol_grid_draw_y_iteration++){
				float x_spot, y_spot;
				x_spot = patrol_grid_draw_x_iteration + patrolField.transform.position.x + x_field_offset;
				y_spot = patrol_grid_draw_y_iteration + patrolField.transform.position.y + y_field_offset;
				
				//CHECK FOR GROUND BELOW
				if (checkForGround(x_spot, y_spot)){
					Gizmos.color = new Color(0f,0f,0f,0.5f);
					patrol_grid[x_index, y_index] = 1;
				} else if (checkForSelf(x_spot, y_spot)){
					Gizmos.color = new Color(1f,0f,0f,0.5f);
					Debug.Log("Enemy Spot: " + x_index + " " + y_index);
					patrol_grid[x_index, y_index] = 2;
				} else if (checkForPlayer(x_spot, y_spot)){
					Gizmos.color = new Color(0f,1f,0f,0.5f);
					Debug.Log("Player Spot: " + x_index + " " + y_index);
					patrol_grid[x_index, y_index] = 3;
				} else {
					Gizmos.color = new Color(0f,0f,1f,0.5f);
					patrol_grid[x_index, y_index] = 0;
				}
				
				Gizmos.DrawCube(new Vector3(x_spot, y_spot, 0), new Vector3(field_cut_rate*0.9f,field_cut_rate*0.9f,1.0f));
				
				y_index++;
			}
			
			patrol_grid_draw_y_iteration = -patrol_grid_y_size/2;
			y_index = 0;
			x_index++;
		}
	}
	
	bool checkForGround(float x_to_raycast, float y_to_raycast){
		Vector2 ground_spot = new Vector2(x_to_raycast, y_to_raycast);
		RaycastHit2D check = Physics2D.Raycast(ground_spot, Vector2.zero, 0.0f, LayerMask.GetMask("Ground"));
		
		return (check.collider != null);
	}
	
	bool checkForSelf(float x_to_position, float y_to_position){
		float self_x_position, self_y_position;
		
		float x_estimation = field_cut_rate/2f;
		float y_estimation = field_cut_rate/2f;
		
		self_x_position = gameObject.transform.position.x;
		self_y_position = gameObject.transform.position.y;
		
		bool x_test, y_test;
		
		x_test = (self_x_position > x_to_position - x_estimation) && (self_x_position < x_to_position + x_estimation);
		
		y_test = (self_y_position > y_to_position - y_estimation) && (self_y_position < y_to_position + y_estimation);
		
		return x_test && y_test;
	}
	
	//THIS WORKS ABOUT THE SAME AS CHECKING FOR SELF, BUT APPLIED TO PLAYER
	bool checkForPlayer(float x_to_position, float y_to_position){
		float self_x_position, self_y_position;
		
		float x_estimation = field_cut_rate/2f;
		float y_estimation = field_cut_rate/2f;
		
		self_x_position = playerPosition.position.x;
		self_y_position = playerPosition.position.y;
		
		bool x_test, y_test;
		
		x_test = (self_x_position > x_to_position - x_estimation) && (self_x_position < x_to_position + x_estimation);
		
		y_test = (self_y_position > y_to_position - y_estimation) && (self_y_position < y_to_position + y_estimation);
		
		return x_test && y_test;
	}
	
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hitting trigger of tag name " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Right") && canFlip)
        {
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            FoeFlipped = S.flipX = true;
        }

        if (collision.gameObject.CompareTag("Left") && canFlip)
        {
            //transform.localScale = new Vector3(1f, 1f, 1f);
            FoeFlipped = S.flipX =  false;
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

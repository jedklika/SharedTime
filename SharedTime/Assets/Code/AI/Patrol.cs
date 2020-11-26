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
	
	AI_Grid grid;
	
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
		grid = GetComponent<AI_Grid>();
    }

    // Update is called once per frame
    void Update()
    {
		//updateMove();
        
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
	
	public void updateMove(){
		if (FoeFlipped){
			grid.updateSight(12, FoeFlipped);
			
			//GO LEFT
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 5f, transform.position.y), speed*Time.deltaTime);
			
			if (grid.get_valid_spot_relative_to_AI(-2, -3) ||
				grid.get_filled_spot_relative_to_AI(-3, 0))
				FoeFlipped = S.flipX = false;
			
		} else {
			grid.updateSight(12, FoeFlipped);
			
			//GO RIGHT
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 5f, transform.position.y), speed*Time.deltaTime);
			
			if (grid.get_valid_spot_relative_to_AI(2, -3)||
				grid.get_filled_spot_relative_to_AI(3, 0))
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

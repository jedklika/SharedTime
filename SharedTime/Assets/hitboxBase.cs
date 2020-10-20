using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class hitboxBase : MonoBehaviour
{
	public int damageDealt;
	private List<int> enemiesToDamage;
	public int threshold;
	public int speed;
	Vector2 targetMove;
	
    // Start is called before the first frame update
    void Start()
    {
        enemiesToDamage = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        updateDelete();
    }
	
	public int dealDamage(int enemy_id){
		//CHECK IF ENEMY ID IS IN BASE
		if (enemiesToDamage.Exists(x => x == enemy_id)){
			return 0;
		} else {
			enemiesToDamage.Add(enemy_id);
			//Debug.Log("Do ouch for " + damageDealt + " damage!");
			return damageDealt;
		}
	}
	
	//FOR TARGETING
	public void setTarget(Vector2 newTarget){
		Debug.Log("Setting spot");
		targetMove = newTarget;
		moveTowards();
	}
	
	public void moveTowards(){
		int childIndex = 0;
		//SET UP THE VECTOR FIRST
		float x_difference, y_difference;
		x_difference = targetMove.x - transform.position.x;
		y_difference = targetMove.y - transform.position.y;
		
		float angle1 = Vector2.SignedAngle(new Vector2(x_difference, y_difference), Vector2.right);
		
		float cos, sin;
		cos = (float)Math.Cos((angle1)*Math.PI/180.0f)*speed;
		sin = -(float)Math.Sin((angle1*Math.PI)/180.0f)*speed;

		Debug.Log(angle1);
		
		for (; childIndex < transform.childCount; childIndex++){
			transform.GetChild(childIndex).GetComponent<Rigidbody2D>().velocity = new Vector2(cos, sin);
		}
	}
	
	private void updateDelete(){
		if (threshold <= enemiesToDamage.Count){
			Destroy(gameObject);
		} 
	}
}

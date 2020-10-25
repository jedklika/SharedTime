using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class explosionBlast : MonoBehaviour
{
	//FOR DAMAGING PLAYER
	GameManager gm;
    public int damageDealt;
	public float force;
	private bool hit_player = false;
	
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
		Debug.Log(other.tag);
		
		if (other.CompareTag("explodableDoor")){
			Debug.Log("Destroying Door");
			Destroy(other.gameObject);
		}
		
        if (!hit_player && other.CompareTag("Player"))
        {
			//DESTRUCTION ALREADY MANAGED ELSEWHERE
			hit_player = true;
            gm.setDamage(damageDealt);
			
			//GET PLAYER'S RIGIDBODY
			Rigidbody2D player_rb = other.GetComponent<Rigidbody2D>();
			
			blastPush(player_rb);
        }
    }
	
	private void blastPush(Rigidbody2D rb){
		//GET ANGLE BETWEEN EXPLOSION AND BLAST
		Vector2 angle_position = new Vector2(rb.position.x - transform.position.x, rb.position.y - transform.position.y);
		float angle = Vector2.SignedAngle(angle_position, Vector2.right);
		float cos, sin;
		
		cos = (float)Math.Cos((angle)*Math.PI/180.0f);
		sin = -(float)Math.Sin((angle)*Math.PI/180.0f);
		
		//ADD IMPULSE TO PLAYER WITH THAT ANGLE AND FORCE
		rb.AddForce(new Vector2(cos, sin)*force);
	}
}

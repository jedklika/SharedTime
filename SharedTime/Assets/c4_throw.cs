using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//THIS IS MOSTLY FOR GIVING THE C4 SOME PROPULSION WHEN CREATED

public class c4_throw : MonoBehaviour
{
	Rigidbody2D rb;
	public float x_push;
	public float y_push;
	bool grounded = false;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		
		rb.velocity = new Vector2(x_push,y_push);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col){
		if (!grounded){
			if (col.CompareTag("Ground")){
				rb.bodyType = RigidbodyType2D.Static;
			}
		}
	}
}

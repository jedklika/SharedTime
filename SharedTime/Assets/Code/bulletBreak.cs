using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBreak : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
		//CONNECT TO ALL OF GROUND OR CRATE
		if (other.CompareTag("Ground") ||
			other.CompareTag("Door") ||
			other.CompareTag("explodableDoor") ||
			other.CompareTag("Crate")){
			Destroy(gameObject);
		}
	}
}

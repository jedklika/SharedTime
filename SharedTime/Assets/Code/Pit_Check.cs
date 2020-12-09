using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit_Check : MonoBehaviour
{
	public GameObject to_send_back_to;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")){
			//SEND TO POSITION
			col.transform.position = to_send_back_to.transform.position;
		}
	}
}

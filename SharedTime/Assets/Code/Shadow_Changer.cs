using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow_Changer : MonoBehaviour
{
	Shadow_Manager sm;
	public int to_set_active;
	
    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<Shadow_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")){
			sm.setShadowActive(to_set_active);
		}
	}
}

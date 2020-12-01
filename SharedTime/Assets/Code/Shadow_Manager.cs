using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Shadow_Manager : MonoBehaviour
{
	public Tilemap[] shadow_sprites;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void setShadowActive(int shadow_index){
		setShadowsInactive();
		
		//SET THE GIVEN INDEX ACTIVE
		shadow_sprites[shadow_index].color = Color.white;
	}
	
	private void setShadowsInactive(){
		//SET ALL INACTIVE
		for (int i = 0; i < shadow_sprites.Length; i++)
			shadow_sprites[i].color = Color.clear;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
	public Sprite character_image;
	
	public Transform hp_bar;
	public Transform sprint_bar;
	public Text InfoText;
	
    // Start is called before the first frame update
    void Start()
    {
		InfoText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void updateHPBar(float ratio){
		//CHANGING SIZE
		Vector2 new_bar = new Vector2(ratio*170.0f, 15);
		
		
		//CHANGING POSITION
		Vector2 new_position = new Vector2((1.0f - ratio)*(-85.0f), 0);
		
		hp_bar.localScale = Vector2.Lerp(hp_bar.localScale, new_bar, 1.25f);
		hp_bar.localPosition = Vector2.Lerp(hp_bar.localPosition, new_position, 1.25f);
	}
	
	public void updateSprintBar(float ratio){
		//CHANGING SIZE
		Vector2 new_bar = new Vector2(ratio*170.0f, 15);
		
		
		//CHANGING POSITION
		Vector2 new_position = new Vector2((1.0f - ratio)*(-85.0f), 0);
		
		sprint_bar.localScale = Vector2.Lerp(sprint_bar.localScale, new_bar, 1.25f);
		sprint_bar.localPosition = Vector2.Lerp(sprint_bar.localPosition, new_position, 1.25f);
	}
}

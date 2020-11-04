using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicQueue : MonoBehaviour
{
	public int trackNumber;
	
	GameMusic music_control;
	
    // Start is called before the first frame update
    void Start()
    {
        music_control = FindObjectOfType<GameMusic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			PlayMusicCue();
		}
	}
	
	private void PlayMusicCue()
	{
		music_control.PlayTrack(trackNumber);
	}
}

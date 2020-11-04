using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
	public AudioSource music;
	
	public AudioClip maintheme;
	
	private int musicState;
	
    // Start is called before the first frame update
    void Start()
    {
        musicState = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void PlayTrack(int trackNum)
	{
		if (musicState != trackNum)
		{
			//Debug.Log("playing track number " + trackNum);
			music.Stop();
			
			switch (trackNum)
			{
			case 0:
				PlayMainTheme();
			break;
			case 1:
				//PlayDiner();
			break;
			case 2:
				//PlaySewer();
			break;
			case 3:
				//PlayTbc();
			break;
			}
		}
	}
	
	private void PlayMainTheme()
	{
		music.clip = maintheme;
		music.Play();
		musicState = 0;
	}
}

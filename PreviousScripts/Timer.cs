using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public int startTime; //number to count down from
	public Text timeLeft; //UI timer thing

	private int timer; //current value (somewhere between startTime and 0)

	// Use this for initialization
	void Start () {
		timer = startTime; //start at the start time
		SetTimeLeft (); //init the on screen display
		InvokeRepeating ("DecreaseTimeRemaining", 1.0f, 1.0f); //run DecreaseTimeRemaining() once every second starting one second from now
	}
	
	// Update is called once per frame
	void Update () {
		if (timer == 0){ //if the timer hits 0
			CancelInvoke ("DecreaseTimeRemaining"); //stop counting down
		}
	}

	void DecreaseTimeRemaining(){ //count down
		timer--; //reduce the time by 1
		SetTimeLeft (); //update the on screen display
		BroadcastMessage ("CheckTime", timer); //send out the current time to anything on this object with the CheckTime(int) function
	}

	void SetTimeLeft(){ //set the display
		int sec;
		int min;
		min = timer / 60; //int division
		sec = timer % 60; //is gay as fuck but it actually works really well in this one specific usage
		timeLeft.text = min.ToString() + ":" + ((sec.ToString().Length==1)?"0":"") + sec.ToString(); //set the timer up in m:ss format, using a "fancy" inline if statement that i forget the name of (sounds boosted)
		if (timer == 0){ //if the timer hits 0
			timeLeft.text = "Get to the Mech!"; //replace the timer text with the notification to get to the mech
		}
	}


	void InMech (bool isIn){ //I dont even use this do i? Edit: holy shit im a god
		if (isIn) { //but i really didnt need to pass in a variable at all
			timeLeft.text = ""; //i could have just done this part, this only gets called the one time
		} //but fuck me i did it the hard way
	}

}

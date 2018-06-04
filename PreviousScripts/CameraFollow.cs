using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour { //for the camera to follow the player

	public GameObject player; //player reference

	private Vector3 offset; //the base offset of the player and camera

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player"); //find the (hopefully) only object with the "Player" tag (the player)
		offset = transform.position - player.transform.position; // set the offset to the difference between the player and the initialized camera position
	}
	
	// Update is called once per frame
	void LateUpdate () { //update after the player(but before the refresh), that way the camera isnt always a step behind
		transform.position = player.transform.position + offset; //camera position is the offset of the player's position
	}
}


/* 
 * Send a message to a function here
 * and have that update the camera
 * (for when in the mech)
 * oh and make a function where pressing the F key toggles the camera, it will make testing way easier
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthTest : MonoBehaviour { //move the sprite depth based on the screen position

	private Rigidbody2D rb; //rigidbody reference
	private SpriteRenderer sprite; //sprite renderer reference
	private BoxCollider2D feet; //checking for the colliders (normally feet, even though walls dont have feet yet)

    /** OK, this shouldnt be this hard
 * check for the bottom side of the collider
 * is it above the top of the player's feet box
 * change based on that
 * ggez?
 * */

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> (); //init rigidbody
		sprite = GetComponent<SpriteRenderer> (); //init sprite renderer
		feet = GetComponent<BoxCollider2D> (); //init foot box collider
	}
	
	// Update is called once per frame
	void Update () { //there really has to be a better way to do this, but ill figure it out later
		Vector3 playerPos = GameObject.Find ("playerTemp").GetComponent<Transform> ().position; //check the player position

		float yPos = playerPos.y-0.116f; //.116 - set this y position variable
		float feetOffset = feet.offset.y - feet.size.y - 0.05f; //i really dont know why i did it this way, but this is the collider location ISH (cause im a moron)

		//player is always in sorting position 1
		if ((rb.GetComponent<Transform> ().position.y + feetOffset) - 0.055f > yPos) { //if the foot location is above the player (based on Y values)
			sprite.sortingOrder = 0; //move the object to the sorting layer behind the player
		} else {
			sprite.sortingOrder = 2; //move the object to the sorting layer in front of the player
		}
	}
}



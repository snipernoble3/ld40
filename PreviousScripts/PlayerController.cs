using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Sprite baseSprite; //Base Sprite Image
	private float baseSpriteScale; //Scale of the Base Sprite
	public Sprite mechSprite; //Mech Sprite Image
	private float mechSpriteScale; //Scale of the Mech Sprite

	public GameObject projectilePrefab; //Reference to the Bullet Prefab
	private List<GameObject> Projectiles = new List<GameObject> (); //Holds all active bullets


	public float speed; //Movement Speed
	public Text uCount; //Held Uranium Count Display
	public Text mechUDisplay; //Display on Mech showing total Uranium
	public Slider slider; //Energy Bar
	public float projectileVelocity = 7f; //Speed of bullets
	public float fireRate = 15f; //bullets per second

	private Rigidbody2D rb; //Player rigidbody
	private SpriteRenderer spriteR; //tool for changing out sprites

	private int currU; //int Uranium being carried
	float totalU; //Total Uranium in the game at start
	private float mechU; //Uranium in the mech
	private float maxMechPower; //max mech power
	private float mechPower; //current mech power
	private bool timeLeft; //time left in collect period
	private bool inMech; //in mech true or false
	private float nextTimeToFire = 0f; //used to track last fire time (for fireRate spacing)


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> (); //init rigidbody reference
		timeLeft = true; //dont want to start in mech, that would be bad
		GameObject[] u = GameObject.FindGameObjectsWithTag ("Uranium"); //make an array for total uranium objects
		totalU = u.Length; //make the array length the total uranium value
	}

	void Update(){

		//Shooting
		if (inMech) { //so you can only shoot in the mech
			if (Input.GetKey (KeyCode.UpArrow) && Time.time >= nextTimeToFire) { //if hitting the up arrow and firing is off cooldown
				nextTimeToFire = Time.time + 1f / fireRate; //reset fire rate cooldown
				GameObject bullet = (GameObject)Instantiate (projectilePrefab, transform.position, Quaternion.identity); //make the bullet object
				Projectiles.Add (bullet); //add it to the list of active bullets
			}

			for (int i = 0; i < Projectiles.Count; i++) { //check the active bullet list
				GameObject moveBullet = Projectiles[i]; //select the bullet in position i of the list
				if (moveBullet != null){ //if the list position isnt null
					moveBullet.transform.Translate (new Vector3(0,1) * Time.deltaTime * projectileVelocity); //move the bullet at the velocity speed

					Vector3 bulletScreenPos = Camera.main.WorldToScreenPoint (moveBullet.transform.position); //get the position of the bullet in relation to the camera
					if (bulletScreenPos.y >= Screen.height || bulletScreenPos.y <= 0){ //if the bullet is off the screen
						DestroyObject (moveBullet); //delete the bullet from the game
						Projectiles.Remove (moveBullet); //and the active bullet list
					}
				}

			}

		}

	}

	// Update is called once per frame
	void FixedUpdate () {

		//Variables that should honestly be in start
		spriteR = GetComponent<SpriteRenderer> (); //init sprite renderer

		baseSpriteScale = 3.0f; //set scale of base sprite
		mechSpriteScale = 5.0f; //set scale of mech sprite

		//Mech only UI
		slider.gameObject.SetActive (inMech); //only show the energy bar while in the mech

		//Moving
		float moveH = Input.GetAxis ("Horizontal"); //normally this would work with arrow keys but i changed the settings so i can use them to fire instead
		float moveV = Input.GetAxis ("Vertical"); //aka these use WASD only

		rb.velocity = new Vector2 (moveH * speed, moveV * speed); //move in a direction at the speed




	}


	void OnTriggerEnter2D(Collider2D other) //when you first hit something (only that instant, nothing past that, so collisions only)
	{

		if (other.gameObject.CompareTag ("Uranium")) //if you hit Uranium
		{
			
			other.gameObject.SetActive(false); //turn it off on the screen
			currU++; //add 1 to the counter of carried uranium
			SetUCountText (); //update the UI display

		}

	}
	
	void OnTriggerStay2D(Collider2D other){ //when you hit something and stay there (this is the activating with buttons one)


		if (other.gameObject.CompareTag ("Mech")) { //if you are near the mech aka hitting its trigger collider
			if (Input.GetKey (KeyCode.E)) { //and you press e
				if (timeLeft) { //and its the collect period
					
					AddMechU (); //add uranium to the mech
					currU = 0; //dump your current carried uranium
					SetUCountText (); //reset the counter on screen

				} else if (!timeLeft) { //if you arent in the collect period
					EnterMech (other); //hmm i wonder what this does? maybe enter the mech. /shrug

				}
			}

		}

	}

	//fuck, my music stopped
	//fixed

	void AddMechU(){ //funct for adding uranium to the mech
		mechU += currU; //add carried uranium
		mechUDisplay.text = mechU.ToString (); //update the number on the mech
	}

	void SetUCountText (){ //sets the uranium counter UI
		uCount.text = "Uranium Held: " + currU.ToString ();
		if (!timeLeft) { //if time runs out
			uCount.text = ""; //turn it off
		}
	}

	void CheckTime(int timer){ //check the timer (i actually figured out how to get this from a different script that throws it to this one)
		if (timer == 0) { //if the timer hits 0
			timeLeft = false; //there isnt time left
			EndCollectStage (); //stop collecting shit
		} else { //if the timer is active
			timeLeft = true; //keep collecting
		}
	}

	void EndCollectStage(){ //shuts off all the uranium stuff
		GameObject[] u = GameObject.FindGameObjectsWithTag ("Uranium"); //find all remaining uranium
		for (int x = 0; x < u.Length; x++){ //loop through it
			u [x].SetActive (false); //turn it off
		}
		currU = 0; //set current uranium counter to 0, dont need to be poisoned while in the mech
		SetUCountText (); //will make it pass the if statement
	}

	void EnterMech(Collider2D other){ //when you press E to get in the mech
		inMech = true; //duh
		SendMessageUpwards ("InMech", true); //tells timer to remove "get to mech" message
		other.gameObject.SetActive (false); //destroy mech model
		spriteR.sprite = mechSprite;//replace player sprite with mech sprite
		rb.transform.localScale = new Vector3(mechSpriteScale, mechSpriteScale, 0.0f);//rescale sprite
		CalcPower(); //convert the collected uranium to a % of total power
		InvokeRepeating ("RechargePowerSlider", 1.0f, 1.0f);//add power meter to UI once every second - prob gonna speed this up, and will need to change the recharge rate to balance that
		//add power meter to mech
		//activate weapons - may need to be a class unlocked with inMech
		//complete ui
	}

	void CalcPower(){ //calculates available max power
		

		maxMechPower = (mechU/totalU)*100; //percent of the total uranium in the game that made it into the mech
		slider.value = maxMechPower; //fill the energy bar
		mechPower = maxMechPower; //set the current power level to the max power level

	}

	void RechargePowerSlider(){ //natural energy restoration
		
		//update amount
		int rechargeRate = 5; //per second
		if (mechPower < maxMechPower) { //if its currently less than max power
			if (mechPower + rechargeRate >= maxMechPower) { //but a standard recharge would put it over
				mechPower = maxMechPower; //set it to the max value
			} else { //if its way less
				mechPower += rechargeRate; //add the recharge to the current value
			}
		}
		slider.value = mechPower; //update the bar to show the updated value

	}
}

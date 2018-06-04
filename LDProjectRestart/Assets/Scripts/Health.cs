using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public Slider s;
	public int maxHealth;

	private int health;
	private int regen = 2;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		s.maxValue = maxHealth;
		s.value = maxHealth;
		InvokeRepeating ("RegenHealth", 1.0f, 1.0f);
	}

	void FixedUpdate (){
		if (Input.GetKey (KeyCode.X)) {
			health -= (int)Random.Range(1.0f, 6.0f) * 5;
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		s.value = health;
	}

	void RegenHealth() {
		if (health + regen > maxHealth) {
			health = maxHealth;
		} else {
			health += regen;
		}
	}


}

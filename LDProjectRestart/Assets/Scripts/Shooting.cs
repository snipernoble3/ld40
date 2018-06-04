using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public GameObject projectilePrefab;
	public float projectileVelocity;
	public float fireRate;
	public float range;

	private List<GameObject> upProjectiles = new List<GameObject>();
	private List<GameObject> downProjectiles = new List<GameObject>();
	private List<GameObject> leftProjectiles = new List<GameObject>();
	private List<GameObject> rightProjectiles = new List<GameObject>();
	private float nextTimeToFire = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * So surely this can be optimized
		 * and broken into a few methods
		 * and really be cleaned up
		 * but for now this should work
		 * */

		//For shooting bullets UP
		
		if (Input.GetKey(KeyCode.UpArrow) && Time.time >= nextTimeToFire){
			nextTimeToFire = Time.time + 1f / fireRate;
			GameObject bullet = (GameObject)Instantiate (projectilePrefab, transform.position, Quaternion.identity);
			upProjectiles.Add (bullet);
		}

		for (int u = 0; u < upProjectiles.Count; u++) {
			GameObject currentBullet = upProjectiles [u];

			if (currentBullet != null) {
				currentBullet.transform.Translate (new Vector3 (0, 1) * Time.deltaTime * projectileVelocity);

				Vector3 bulletScreenPos = Camera.main.WorldToScreenPoint (currentBullet.transform.position);
				if(bulletScreenPos.y >= Screen.height/2 + range){
					DestroyObject (currentBullet);
					upProjectiles.Remove (currentBullet);
				}
			}
		}

		//---------------------------------------------------------------------------------------------------------------

		//For shooting bullets DOWN

		if (Input.GetKey(KeyCode.DownArrow) && Time.time >= nextTimeToFire){
			nextTimeToFire = Time.time + 1f / fireRate;
			GameObject bullet = (GameObject)Instantiate (projectilePrefab, transform.position, Quaternion.identity);
			bullet.transform.Rotate (Vector3.forward*180);
			downProjectiles.Add (bullet);
		}

		for (int d = 0; d < downProjectiles.Count; d++) {
			GameObject currentBullet = downProjectiles [d];

			if (currentBullet != null) {
				currentBullet.transform.Translate (new Vector3 (0, 1) * Time.deltaTime * projectileVelocity);

				Vector3 bulletScreenPos = Camera.main.WorldToScreenPoint (currentBullet.transform.position);
				if(bulletScreenPos.y <= Screen.height/2 - range){
					DestroyObject (currentBullet);
					downProjectiles.Remove (currentBullet);
				}
			}
		}

		//---------------------------------------------------------------------------------------------------------------

		//For shooting bullets LEFT

		if (Input.GetKey(KeyCode.LeftArrow) && Time.time >= nextTimeToFire){
			nextTimeToFire = Time.time + 1f / fireRate;
			//float[] test = Quaternion.Euler (0,0,90);
			//spriteRotation.z = test[0];
			GameObject bullet = (GameObject)Instantiate (projectilePrefab, transform.position, Quaternion.identity);
			bullet.transform.Rotate (Vector3.forward * 90);
			leftProjectiles.Add (bullet);
		}

		for (int l = 0; l < leftProjectiles.Count; l++) {
			GameObject currentBullet = leftProjectiles [l];

			if (currentBullet != null) {
				currentBullet.transform.Translate (new Vector3 (0, 1) * Time.deltaTime * projectileVelocity);

				Vector3 bulletScreenPos = Camera.main.WorldToScreenPoint (currentBullet.transform.position);
				if(bulletScreenPos.x <= Screen.width/2 - range){
					DestroyObject (currentBullet);
					leftProjectiles.Remove (currentBullet);
				}
			}
		}

		//---------------------------------------------------------------------------------------------------------------

		//For shooting bullets RIGHT

		if (Input.GetKey(KeyCode.RightArrow) && Time.time >= nextTimeToFire){
			nextTimeToFire = Time.time + 1f / fireRate;
			GameObject bullet = (GameObject)Instantiate (projectilePrefab, transform.position, Quaternion.identity);
			bullet.transform.Rotate (Vector3.forward * -90);
			rightProjectiles.Add (bullet);
		}

		for (int r = 0; r < rightProjectiles.Count; r++) {
			GameObject currentBullet = rightProjectiles [r];

			if (currentBullet != null) {
				currentBullet.transform.Translate (new Vector3 (0, 1) * Time.deltaTime * projectileVelocity);

				Vector3 bulletScreenPos = Camera.main.WorldToScreenPoint (currentBullet.transform.position);
				if(bulletScreenPos.x >= Screen.width/2 + range){
					DestroyObject (currentBullet);
					rightProjectiles.Remove (currentBullet);
				}
			}
		}



	}
}

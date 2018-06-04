using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Camera cam;
	public float size;
	public float xPos;
	public float yPos;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = cam.transform.position - this.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (this.isActiveAndEnabled){
			cam.transform.position = this.transform.position + offset;
		}
	}
}

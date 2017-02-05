using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GV{ // global variables
//	public static float pathLength; 
	public static Vector2 speed;
}

// Moving people doesn't work with transform.position.Set but transform.Translate

public class IntegratingSpeed : MonoBehaviour {

	public GameObject player;
	public GameObject playerCam;

	public float speedMultiplier = 0.2f;

	public Vector3 adjSpeed; // adjusted for rotations of the camera

	// Use this for initialization
	void Start () {

		// this returns the parent to be transformed (player.transform.position)
		player = GameObject.Find ("Player");
		// this can also be accomplished with transform.root.position

		playerCam = GameObject.Find ("Main Camera");
		// yaw = playerCam.transform.eulerAngles.y

	}
	
	// Update is called once per frame
	void Update () {

		// debug speed
//		UnityEngine.Debug.Log (string.Format ("Speed Vx: {0}\tVy: {1}",
//			GV.speed.x.ToString (),
//			GV.speed.y.ToString ()));

		// rotate to accomodate the rotation of the camera
		adjSpeed.x = GV.speed.x;
		adjSpeed.y = 0;
		adjSpeed.z = GV.speed.y;
		adjSpeed = Quaternion.AngleAxis (playerCam.transform.eulerAngles.y, Vector3.up) * adjSpeed;

		// move the person accordingly
		player.transform.Translate (adjSpeed.x * speedMultiplier,
									0,
									adjSpeed.z * speedMultiplier);


	}
}

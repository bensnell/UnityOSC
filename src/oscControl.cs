//
//	  UnityOSC - Example of usage for OSC receiver
//
//	  Copyright (c) 2012 Jorge Garcia Martin
//
// 	  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// 	  documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// 	  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// 	  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// 	  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// 	  of the Software.
//
// 	  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// 	  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// 	  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// 	  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// 	  IN THE SOFTWARE.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class oscControl : MonoBehaviour {
	
	private Dictionary<string, ServerLog> servers;

	public Vector2 globalForce;	// force on the person's body that will move them around

	public float avgFrameDuration = 0.0166f;
	public float easingFPS = 0.97f;

	public float easingSpeed = 0.95f;
	public float maxSpeed = 20.0f;

	public bool bDebug = false;

	// Script initialization
	void Start() {	
		
		globalForce = new Vector2(0, 0);
		GV.speed = new Vector2 (0, 0);

		OSCHandler.Instance.Init(); //init OSC
		servers = new Dictionary<string, ServerLog>();
	}

	// NOTE: The received messages at each server are updated here
    // Hence, this update depends on your application architecture
    // How many frames per second or Update() calls per frame?
	void Update() {

		// update the average frame duration
		avgFrameDuration = avgFrameDuration * easingFPS + (1.0f - easingFPS) * Time.deltaTime;
		
		OSCHandler.Instance.UpdateLogs();

		servers = OSCHandler.Instance.Servers;

		globalForce.x = 0;
		globalForce.y = 0;

		foreach (KeyValuePair<string, ServerLog> item in servers) {
			// If we have received at least one packet
			if (item.Value.packets.Count > 0) {

				// print the number of packets since last update
//				UnityEngine.Debug.Log (String.Format ("There are {0} packets and {1} logs",
//					item.Value.packets.Count,
//					item.Value.log.Count));

				// find the average force and apply it to the speed
				for (int i = 0; i < item.Value.packets.Count; i++) {

					// debug
//					UnityEngine.Debug.Log (String.Format ("SERVER: {0}\tADDR: {1}\tVALUE: {2}\tTIME: {3}",
//						item.Key,
//						item.Value.packets [i].Address,
//						item.Value.packets [i].Data [0].ToString (),
//						item.Value.packets[i].TimeStamp.ToString()));

					// sum components
					globalForce.x += float.Parse(item.Value.packets [i].Data [0].ToString());
					globalForce.y += float.Parse(item.Value.packets [i].Data [1].ToString());
				}
				// divide by the number of packets
				globalForce /= (float)item.Value.packets.Count;

				// clear all packets
				item.Value.packets.Clear();
				item.Value.log.Clear ();

				// debug force
//				UnityEngine.Debug.Log (String.Format ("Force Vx: {0}\tVy: {1}",
//					globalForce.x.ToString (),
//					globalForce.y.ToString ()));
			}
		}

//		UnityEngine.Debug.Log (String.Format ("Before calculations: Speed: {0}\tForce: {1}",
//			speed.magnitude.ToString (),
//			globalForce.magnitude.ToString ()));

		// update the speed
		GV.speed += avgFrameDuration * globalForce;

//		UnityEngine.Debug.Log (String.Format ("After addition: Speed: {0}\tForce: {1}",
//			speed.magnitude.ToString (),
//			globalForce.magnitude.ToString ()));

		// apply a frictional force
		GV.speed *= easingSpeed;

		// cap the speed
		if (GV.speed.magnitude > maxSpeed) {
//			UnityEngine.Debug.Log ("HEREEEE");
			GV.speed = GV.speed / GV.speed.magnitude * maxSpeed;
		}

//		UnityEngine.Debug.Log (String.Format ("Speed Vx: {0}\tVy: {1}",
//			speed.x.ToString (),
//			speed.y.ToString ()));

		// now the camera can be moved depending on where it is looking...
	}
}
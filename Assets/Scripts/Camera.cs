using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = new Vector3(
			GameCore.instance.player.transform.position.x,
			gameObject.transform.position.y,
			gameObject.transform.position.z
		);
	}
}

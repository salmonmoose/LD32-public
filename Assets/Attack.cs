using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	bool hit = false;
	public int damage = 50;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Hit() {
		hit = true;
	}

	public void HitEnd() {
		hit = false;
	}

	void OnTriggerEnter2D (Collider2D other) {

	}

	void OnTriggerStay2D (Collider2D other) {
		if (hit) {
			other.gameObject.SendMessage("ApplyDamage", damage);
			other.gameObject.SendMessage("Knockback");
			hit = false;
		}
	}
}

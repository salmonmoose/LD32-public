using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private Animator anim;
	private Rigidbody2D rigidBody;
	public int damage = 10;


	public float threshold = 0.1f;
	private AudioSource audio;

	public AudioClip launch;
	public AudioClip impact;

	void Awake() {
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		audio.Stop();
		audio.clip = launch;
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D collision) {
		foreach (ContactPoint2D contact in collision.contacts) {
			if (1 - Vector2.Dot(contact.normal, Vector2.up) < threshold) {
				anim.SetTrigger("floor");
				Destroy(rigidBody);
			} else if (1 - Mathf.Abs(Vector2.Dot(contact.normal, Vector2.right)) < threshold) {
				anim.SetTrigger("wall");
			}
		}

		audio.Stop();
		audio.clip = impact;
		audio.Play();

		collision.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
	}

	void Clean() {
		Destroy(gameObject);
	}
}

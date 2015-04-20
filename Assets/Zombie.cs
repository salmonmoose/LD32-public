using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

	public int health = 100;
	public float maxSpeed = 0.5f;
	public float minSpeed = 0.1f;
	public float moveForce = 100.0f;
	private Animator anim;
	private Rigidbody2D rigidBody;
	private float speed;
	private bool alive = true;
	private bool facingRight;

	private AudioSource audio;
	public AudioClip groan;
	public AudioClip hurtSound;
	public AudioClip dieSound;


	void Awake() {
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		audio.clip = groan;
		audio.PlayDelayed(Random.Range(0,50));

		speed = Random.Range(minSpeed, maxSpeed);

	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0 && alive) {
			anim.SetTrigger("Die");
			alive = false;
			audio.Stop();
			audio.clip = dieSound;
			audio.Play();

			Destroy(gameObject.GetComponent<PolygonCollider2D>());
			Destroy(rigidBody);
		}

		if (alive) {
			if (GameCore.instance.player.gameObject.transform.position.x < gameObject.transform.position.x) {
				rigidBody.AddForce(-Vector2.right * moveForce);

				anim.SetBool("Right", false);
				facingRight = false;
			} else {
				rigidBody.AddForce(Vector2.right * moveForce);

				anim.SetBool("Right", true);
				facingRight = true;
			}

			if (Mathf.Abs(rigidBody.velocity.x) > speed) {
				rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * speed, rigidBody.velocity.y);
			}
		}
	}

	void FixedUpdate() {

	}

	void OnTriggerEnter2D(Collider2D other) {
	}

	void ApplyDamage(int amount) {
		if (alive) {
			health = health - amount;
			anim.SetTrigger("Hurt");
			audio.Stop();
			audio.clip = hurtSound;
			audio.Play();
		}
	}

	void Knockback() {
		if (GameCore.instance.player.gameObject.transform.position.x < gameObject.transform.position.x) {
			rigidBody.AddForce(new Vector2(1000f, 350f));
		} else {
			rigidBody.AddForce(new Vector2(-1000f,350f));
		}
	}

}

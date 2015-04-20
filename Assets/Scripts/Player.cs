using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float maxSpeed = 5.0f;
	public float moveForce = 365.0f;
	public float jumpForce = 1000.0f;
	public float airPenalty = 0.25f;
	public float threshold = 0.1f;

	public float shootForce = 100.0f;

	public float hitRate = 0.5f;
	private float nextHit = 0.0f;

	public float shootRate = 0.5f;
	private float nextShoot = 0.0f;

	public float chokeRate = 0.5f;
	private float nextChoke = 0.0f;

	private Animator anim;
	private Rigidbody2D rigidBody;
	private bool grounded = false;
	private bool facingRight;
	private bool jump = false;
	private bool hit = false;
	private bool armed = false;
	private bool choke = false;

	private AudioSource audio;
	public AudioClip hitSound;


	public GameObject hitLeft;
	public GameObject hitRight;

	public GameObject projectile;

	void Awake() {
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
	}

	void FixedUpdate() {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		if (h * rigidBody.velocity.x < maxSpeed && !armed) {
			if (grounded) {
				rigidBody.AddForce(Vector2.right * h * moveForce);
			} else {
				rigidBody.AddForce(Vector2.right * h * moveForce * airPenalty);
			}
		}

		if (Mathf.Abs (rigidBody.velocity.x) > maxSpeed) {
			rigidBody.velocity = new Vector2(Mathf.Sign (rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);
		}

		anim.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

		if (h > 0 && !facingRight) {
			Flip();
		} else if (h < 0 && facingRight) {
			Flip();
		}

		if (jump) {
			rigidBody.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && Time.time > nextHit) {
			nextHit = Time.time + hitRate;
			anim.SetTrigger("Hit");
		}

		if (Input.GetButton("Fire2") && grounded) {
			anim.SetBool("Shooting", true);
			if (Time.time > nextShoot && armed) {
				nextShoot = Time.time + shootRate;
				anim.SetTrigger("Shoot");
				
				if (facingRight) {
					GameObject egg = Instantiate(projectile, transform.position + new Vector3(0.25f, 0.5f, 0f), transform.rotation) as GameObject;
					egg.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootForce);
				} else {
					GameObject egg = Instantiate(projectile, transform.position + new Vector3(-0.25f, 0.5f, 0f), transform.rotation) as GameObject;
					egg.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * shootForce);
					egg.transform.localScale = new Vector3(-1,1,1);
				}
			}
		} else {
			anim.SetBool("Shooting", false);
		}

		if (Input.GetButton("Fire3") && Time.time > nextChoke) {
			nextChoke = Time.time + chokeRate;
			anim.SetTrigger("Choke");
		}

		if (Input.GetButton("Jump") && grounded) {
			jump = true;
			grounded = false;
		}

		if (Mathf.Abs(gameObject.transform.position.x) > 2) {
			GameCore.instance.HideLogo();
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		foreach (ContactPoint2D contact in collision.contacts) {
			if(1 - Vector2.Dot(contact.normal, Vector2.up) < threshold) {
				grounded = true;
			}
		}
	}

	void OnCollisionStay2D(Collision2D collision) {

	}

	void Flip() {
		facingRight = !facingRight;

		anim.SetBool("Right", facingRight);
	}

	void Arm() {
		armed = !armed;
		nextShoot = Time.time + shootRate;
	}

	void Hit() {
		if (facingRight) {
			hitRight.GetComponent<Attack>().Hit();
		} else {
			hitLeft.GetComponent<Attack>().Hit();
		}

		audio.Stop();
		audio.clip = hitSound;
		audio.Play();
	}

	void HitEnd() {
		if (facingRight) {
			hitRight.GetComponent<Attack>().HitEnd();
		} else {
			hitLeft.GetComponent<Attack>().HitEnd();
		}
	}

	void AttackHit() {
		Debug.Log("AttackHit");
	}
}

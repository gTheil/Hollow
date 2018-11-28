using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : Fireball {

	public AudioClip clipHurt;

	private Player player;
	private AudioSource audioHurt;
	private AudioManager am;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		player = Player.FindObjectOfType<Player>();

		if (!player.facingRight) {
			speed *= -1;
			Flip();
		}

		am = FindObjectOfType<AudioManager>();
		audioHurt = am.AddAudio (clipHurt, false, false, 1f);
	}

	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2(speed, 0f);
		Destroy (gameObject, 1f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Enemy enemy = other.GetComponent<Enemy>();
		if (enemy != null) {
			audioHurt.Play();
			if (enemy.blueBuffOn && !player.blueBuffSpell)
				player.SetPlayerSpell (player.database.GetSpell (3));
			if (player.redBuff)
				enemy.TakeDamage ((attack * 2)); // Causa dano ao personagem
			else
				enemy.TakeDamage(attack);
			Destroy(gameObject);
		}
	}
}

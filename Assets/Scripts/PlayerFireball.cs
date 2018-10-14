using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : Fireball {

	private Player player;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		player = Player.FindObjectOfType<Player>();

		if (!player.facingRight) {
			speed *= -1;
			Flip();
		}
	}

	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2(speed, 0f);
	}

	void OnCollisionEnter2D(Collision2D other) {
		Enemy enemy = other.gameObject.GetComponent<Enemy>();
		if (enemy != null) {
			enemy.TakeDamage(attack); // Causa dano ao personagem
		}
		Destroy(gameObject);
	}
}

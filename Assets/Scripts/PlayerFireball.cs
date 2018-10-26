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
		Destroy (gameObject, 1f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Enemy enemy = other.GetComponent<Enemy>();
		if (enemy != null) {
			if (player.redBuff)
				enemy.TakeDamage ((attack * 2)); // Causa dano ao personagem
			else
				enemy.TakeDamage(attack);
			Destroy(gameObject);
		}
	}
}

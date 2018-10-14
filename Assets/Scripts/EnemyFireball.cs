using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : Fireball {

	private Enemy enemy;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		enemy = Enemy.FindObjectOfType<Enemy>();

		if (!enemy.facingRight) {
			speed *= -1;
			Flip();
		}
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2(speed, 0f);
	}

	void OnCollisionEnter2D(Collision2D other) {
		Player player = other.gameObject.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage(attack); // Causa dano ao personagem
			if (!player.fireballSpell)
				player.SetPlayerSpell(player.database.GetSpell(1));
		}
		Destroy(gameObject);
	}
}

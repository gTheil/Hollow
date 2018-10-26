using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : Fireball {

	private Player player;
	private Enemy enemy;
	private Vector3 playerDistance;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		enemy = Enemy.FindObjectOfType<Enemy>();
		player = Player.FindObjectOfType<Player> ();
		playerDistance = player.transform.position - transform.position;

		if (playerDistance.x < 0) {
			speed *= -1;
			Flip ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2(speed, 0f);
		Destroy (gameObject, 1f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player>();
		if (player != null) {
			Destroy (gameObject);
			if (enemy.redBuffOn) {
				player.TakeDamage ((attack * 2)); // Causa dano ao personagem
			} else {
				player.TakeDamage (attack); // Causa dano ao personagem
			}
			Vector2 kbForce = new Vector2(knockback.x * (playerDistance.x / Mathf.Abs(playerDistance.x)), knockback.y);
			player.GetComponent<Rigidbody2D>().AddForce(kbForce, ForceMode2D.Impulse); // Empurra o personagem uma determinada distância
			if (!player.fireballSpell)
				player.SetPlayerSpell (player.database.GetSpell (1));
		}
	}
}
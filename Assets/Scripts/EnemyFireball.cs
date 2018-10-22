using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireball : Fireball {

	private Attack attackObj;
	private Player player;
	private Enemy enemy;
	private Vector3 playerDistance;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		enemy = Enemy.FindObjectOfType<Enemy>();
		player = Player.FindObjectOfType<Player> ();
		attackObj = Attack.FindObjectOfType<Attack> ();
		playerDistance = player.transform.position - transform.position;

		if (!enemy.facingRight) {
			speed *= -1;
			Flip();
		}
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2(speed, 0f);
		Destroy (gameObject, 3f);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player>();
		if (player != null) {
			if (enemy.redBuffOn) {
				player.TakeDamage ((attack * 2)); // Causa dano ao personagem
			} else {
				player.TakeDamage (attack); // Causa dano ao personagem
			}
			player.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * knockback * (playerDistance.x / Mathf.Abs (playerDistance.x)), ForceMode2D.Impulse); // Empurra o personagem uma determinada distância
			if (!player.fireballSpell)
				player.SetPlayerSpell (player.database.GetSpell (1));
			Destroy (gameObject);
		} else if (attackObj != null) {
			Destroy (gameObject);
		}
	}
}

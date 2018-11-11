using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

	public int damage;
	public float damageRate;

	private Player player;
	private bool onSpike;
	private float nextDamage;

	void Start () {
		player = Player.FindObjectOfType<Player>();
	}

	void Update () {
		if (onSpike && Time.time > nextDamage) {
			player.TakeDamage (20);
			nextDamage = Time.time + damageRate;
		}
	}

	// Chamado ao personagem entrar em contato com o objeto no mapa
	private void OnCollisionEnter2D(Collision2D other) {
		Player player = other.gameObject.GetComponent<Player>();
		if (player != null) {
			onSpike = true;
		}
	}

	// Chamado ao personagem entrar em contato com o objeto no mapa
	private void OnCollisionExit2D(Collision2D other) {
		Player player = other.gameObject.GetComponent<Player>();
		if (player != null) {
			onSpike = false;
		}
	}
}

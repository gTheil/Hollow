using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWeapon : MonoBehaviour {

	public Weapon weapon; // Referência a arma associada ao objeto no mapa

	private SpriteRenderer sprite; // Imagem utilizada pelo objeto no mapa

	// Inicialização
	void Start () {
		// Atribui ao objeto a imagem da arma associada a ele
		sprite = GetComponent<SpriteRenderer>();
		sprite.sprite = weapon.image;
	}

	// Chamado ao personagem entrar em contato com o objeto no mapa
	private void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player>();
		if (player != null) {
			player.AddWeapon(weapon); // Chama o método AddWeapon da classe Player, equipando a arma associada ao objeto cujo personagem entrou em contato 
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConsumable : MonoBehaviour {

	public Consumable consumable; // Referência ao consumível associado ao objeto no mapa

	private SpriteRenderer sprite; // Imagem utilizada pelo objeto no mapa

	// Inicialização
	void Start () {
		// Atribui ao objeto no mapa a imagem do consumível associado a ele
		sprite = GetComponent<SpriteRenderer>();
		sprite.sprite = consumable.image;
	}

	// Chamado ao personagem entrar em contato com o objeto no mapa
	private void OnTriggerEnter2D (Collider2D other) {
		// Verifica se foi o personagem que entrou em contato com o objeto
		if (other.CompareTag("Player")) {
			FindObjectOfType<UIManager>().SetMessage(consumable.message);
			Inventory.playerInventory.AddConsumable(consumable); // Adiciona ao inventário do jogador o consumível associada ao objeto cujo personagem entrou em contato 
			Destroy(gameObject); // Remove o objeto do mapa
		}
	}
}

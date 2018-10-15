using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapKey : MonoBehaviour {

	public Key key; // Referência a chave associada ao objeto no mapa

	private SpriteRenderer sprite; // Imagem utilizada pelo objeto no mapa

	// Inicialização
	void Start () {
		// Atribui ao objeto no mapa a imagem da chave associada a ele
		sprite = GetComponent<SpriteRenderer>();
		sprite.sprite = key.image;

		if (Inventory.playerInventory.CheckKey (key))
			Destroy (gameObject);
	}

	// Chamado ao personagem entrar em contato com o objeto no mapa
	private void OnTriggerEnter2D(Collider2D other){
		// Verifica se foi o personagem que entrou em contato com o objeto
		Player player = other.GetComponent<Player>();
		if (player != null) {
			FindObjectOfType<UIManager>().SetMessage(key.message);
			PlayerInventory.playerInventory.AddKey(key); // Adiciona ao inventário do jogador a chave associada ao objeto cujo personagem entrou em contato 
			Destroy(gameObject); // Remove o objeto do mapa
		}
	}
}

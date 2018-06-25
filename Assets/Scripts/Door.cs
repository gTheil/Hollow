using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public Key key; // Referência a chave associada à porta especificada
	public Sprite doorOpen; // Imagem que é exibida quando a porta está aberta

	private SpriteRenderer sprite; // Imagem utilizada pelo objeto no mapa
	private BoxCollider2D boxCollider; // Caixa de colisão da porta no mapa

	// Inicialização
	void Start () {
		sprite = GetComponent<SpriteRenderer>(); // Atribui à porta sua imagem
		boxCollider = GetComponent<BoxCollider2D>(); // Ativa a caixa de colisão da porta
	}

	// Chamado ao personagem entrar em contato com o objeto no mapa
	private void OnCollisionEnter2D(Collision2D other) {
		// Verifica se foi o personage que entrou em contato com o objeto
		if (other.gameObject.CompareTag("Player")){
			// Verifica se a chave associada à esta porta está na lista de chaves do inventário do jogador
			if(Inventory.inventory.CheckKey(key)) {
				sprite.sprite = doorOpen; // Altera a imagem da porta padrão para a imagem de "porta aberta"
				boxCollider.enabled = false; // Desativa a caixa de colisão da porta
			} else
				FindObjectOfType<UIManager>().SetMessage("Requer uma Chave!");
		}
	}
}
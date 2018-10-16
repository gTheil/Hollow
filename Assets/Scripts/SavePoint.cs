using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

	private bool contact = false;

	void Update() {
		if (contact) {
			if (Input.GetKeyDown (KeyCode.B)) {
				FindObjectOfType<UIManager> ().SetMessage ("Jogo Salvo!");
				GameManager.gm.Save (); // Salva os dados do personagem
			} else if (Input.GetKeyDown (KeyCode.V)) {
				FindObjectOfType<UIManager>().CallShop();
			}
		}
	}

	// Ao jogador entrar em contato com o save point
	public void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			contact = true;
			FindObjectOfType<UIManager>().SetMessage("Pressione V para acessar a loja\nPressiona B para salvar o jogo");
		}
	}

	public void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			contact = false;
		}
	}
}

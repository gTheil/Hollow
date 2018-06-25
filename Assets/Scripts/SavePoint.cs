using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

	// Ao jogador entrar em contato com o save point
	public void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			FindObjectOfType<UIManager>().SetMessage("Jogo Salvo!");
			GameManager.gm.Save (); // Salva os dados do personagem
			Player player = FindObjectOfType<Player> ();
			player.saved = true;
		}
	}
}

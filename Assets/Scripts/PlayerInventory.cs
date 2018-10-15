using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory {

	// Inicialização do inventário, impede que haja mais de um inventário em cena para que itens não sejam perdidos ao serem adicionados
	void Awake () {
		if (playerInventory == null) {
			playerInventory = this;
		}
		else if (playerInventory != this){
			Destroy(gameObject);
		}

		// Faz com que o inventário permaneça o mesmo caso outra cena seja carregada
		DontDestroyOnLoad(gameObject);
		LoadInventory();
	}

	void LoadInventory() {
		for (int i = 0; i < GameManager.gm.playerSkills.Length; i++) {
			AddSkill(database.GetSkill(GameManager.gm.playerSkills[i]));
		}
		for (int i = 0; i < GameManager.gm.playerSpells.Length; i++) {
			AddSpell(database.GetSpell(GameManager.gm.playerSpells[i]));
		}
		for (int i = 0; i < GameManager.gm.playerConsumables.Length; i++) {
			AddConsumable(database.GetConsumable(GameManager.gm.playerConsumables[i]));
		}
		for (int i = 0; i < GameManager.gm.playerKeys.Length; i++) {
			AddKey(database.GetKey(GameManager.gm.playerKeys[i]));
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory {

	private GameManager gm; // Referência ao GameManager

	// Inicialização do inventário, impede que haja mais de um inventário em cena para que itens não sejam perdidos ao serem adicionados
	void Awake () {
		if (playerInventory == null) {
			playerInventory = this;
		}
		else if (playerInventory != this){
			Destroy(gameObject);
		}

		gm = GameManager.gm;
		LoadInventory();
	}



	void LoadInventory() {
		for (int i = 0; i < gm.playerSkills.Length; i++) {
			AddSkill(database.GetSkill(gm.playerSkills[i]));
		}
		for (int i = 0; i < gm.playerSpells.Length; i++) {
			AddSpell(database.GetSpell(gm.playerSpells[i]));
		}
		for (int i = 0; i < gm.playerConsumables.Length; i++) {
			AddConsumable(database.GetConsumable(gm.playerConsumables[i]));
		}
		for (int i = 0; i < gm.playerKeys.Length; i++) {
			AddKey(database.GetKey(gm.playerKeys[i]));
		}
	}
}

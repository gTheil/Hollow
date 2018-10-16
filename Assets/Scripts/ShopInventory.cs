using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : Inventory {

	private GameManager gm; // Referência ao GameManager

	// Inicialização do inventário, impede que haja mais de um inventário em cena para que itens não sejam perdidos ao serem adicionados
	void Awake () {
		if (shopInventory == null) {
			shopInventory = this;
		}
		else if (shopInventory != this){
			Destroy(gameObject);
		}

		// Faz com que o inventário permaneça o mesmo caso outra cena seja carregada
		//DontDestroyOnLoad(gameObject);
	}

	// Inicialização para carregamento do inventário
	void Start () {
		gm = GameManager.gm;
		LoadInventory();
	}

	void LoadInventory() {
		for (int i = 0; i < gm.shopSkills.Length; i++) {
			AddSkill(database.GetSkill(gm.shopSkills[i]));
		}
	}
}

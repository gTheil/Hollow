using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour {

	public List<Weapon> weapons; // Lista de armas
	public List<Armor> armors; // Lista de armaduras
	public List<Key> keys; // Lista de chaves
	public List<Consumable> consumables; // Lista de consumíveis
	public List<Skill> skills; // Lista de habilidades

	public Skill GetSkill (int ID) {
		foreach (var item in skills) { // Verifica todos os itens na lista de habilidades
			if (item.skillID == ID) // Caso o ID passado conste no item sendo verificado
				return item; // Retorna o item
		}
		return null; // Caso não encontre nada, retorna nulo
	}

	public Consumable GetConsumable (int ID) {
		foreach (var item in consumables) { // Verifica todos os itens na lista de consumíveis
			if (item.itemID == ID) // Caso o ID passado conste no item sendo verificado
				return item; // Retorna o item
		}
		return null; // Caso não encontre nada, retorna nulo
	}

	public Key GetKey (int ID) {
		foreach (var item in keys) {  // Verifica todos os itens na lista de chaves
			if (item.itemID == ID) // Caso o ID passado conste no item sendo verificado
				return item; // Retorna o item
		}
		return null; // Caso não encontre nada, retorna nulo
	}

	public Weapon GetWeapon (int ID) {
		foreach (var item in weapons) {  // Verifica todos os itens na lista de armas
			if (item.itemID == ID) // Caso o ID passado conste no item sendo verificado
				return item; // Retorna o item
		}
		return null; // Caso não encontre nada, retorna nulo
	}

	public Armor GetArmor (int ID) {
		foreach (var item in armors) {  // Verifica todos os itens na lista de armaduras
			if (item.itemID == ID) // Caso o ID passado conste no item sendo verificado
				return item; // Retorna o item
		}
		return null; // Caso não encontre nada, retorna nulo
	}

}

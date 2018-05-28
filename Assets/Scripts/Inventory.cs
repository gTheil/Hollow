﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory inventory; // Referência ao inventário do jogador

	public List<Weapon> weapons; // Lista de armas do jogador
	public List<Key> keys; // Lista de chaves do jogador

	// Inicialização do inventário, impede que haja mais de um inventário em cena para que itens não sejam perdidos ao serem adicionados
	void Awake () {
		if (inventory == null) {
			inventory = this;
		}
		else if (inventory != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
	
	public void AddWeapon(Weapon weapon) {
		weapons.Add(weapon); // Adiciona a arma coletada à lista de armas no inventário
	}

	public void AddKey(Key key) {
		keys.Add(key); // Adiciona a chave coletada à lista de armas no inventário
	}

	// Verifica se há uma chave específica na lista de chaves do inventário do jogador
	public bool CheckKey(Key key) {
		for (int i = 0; i < keys.Count; i++) {
			// Caso a chave especificada seja encontrada, retorna o valor de verdadeira
			if (keys[i] == key) {
				return true;
			}
		}
		return false; // Caso a chave especificada não seja encontrada, retorna o valor de falso
	}
}

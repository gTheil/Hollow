using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory playerInventory; // Referência ao inventário do jogador
	public static Inventory shopInventory; // Referência ao inventário da loja

	public List<Key> keys; // Lista de chaves do jogador
	public List<Consumable> consumables; // Lista de consumíveis do jogador
	public List<Skill> skills; // Lista de habilidades do jogador
	public List<Spell> spells;

	public Database database; // Referência à base de dados de itens do jogador

	public void AddKey(Key key) {
		keys.Add(key); // Adiciona a chave coletada à lista de armas no inventário
	}

	public void AddConsumable (Consumable consumable) {
		consumables.Add(consumable); // Adiciona o consumível coletado à lista de consumíveis no inventário
	}

	public void AddSkill (Skill skill) {
		skills.Add(skill); // Adiciona a habilidade desbloqueada à lista de habilidades no inventário
	}

	public void AddSpell (Spell spell) {
		spells.Add(spell);
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

	// Chamado ao consumível ser utilizado
	public void RemoveConsumable (Consumable consumable) {
		// Verifica a posição no inventário do consumível utilizado
		for (int i = 0; i < consumables.Count; i++) {
			// Caso o consumível seja encontrado, remove o mesmo do inventário e encerra o laço de repetição
			if (consumables [i] == consumable) {
				consumables.RemoveAt(i);
				break;
			}
		}
	}

	// Método que retorna o a quantidade, no inventário do jogador, do item equipado no slot de uso rápido
	public int CountConsumable(Consumable consumable) {
		int numberOfConsumables = 0;
		for (int i = 0; i < consumables.Count; i++) {
			if (consumable == consumables[i])
				numberOfConsumables++;
		}
		return numberOfConsumables;
	}

	// Chamado ao comprar uma habilidade da loja
	public void RemoveSkill (Skill skill) {
		// Verifica a posição no inventário
		for (int i = 0; i < skills.Count; i++) {
			// Caso a habilidade seja encontrada, remove do inventário
			if (skills [i] == skill) {
				skills.RemoveAt(i);
				break;
			}
		}
	}
}

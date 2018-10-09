using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Facilita criação de instâncias de um objeto através do menu Assets/Create do Unity
public class Spell : ScriptableObject {

	public int spellID; // ID para referência ao objeto
	public string spellName; // Nome da habilidade para visualização
	public string description; // Descrição da habilidade para visualização
	public Sprite image; // Imagem para representar visualmente a habilidade
	public string message; // Mensagem exibida ao desbloquear a habilidade
	public int manaCost; // Custo de mana da habilidade, gasto pelo jogador ao usá-la
}

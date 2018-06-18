using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Armor : ScriptableObject {

	public int itemID; // ID para referência ao objeto
	public string armorName; // Nome da armadura para visualização
	public string description; // Descrição da armadura para visualização
	public int defense; // Defesa concedida pela armadura
	public Sprite image; // Imagem para representar visualmente a armadura
	public string message; // Mensagem exibida ao coletar a armadura

}

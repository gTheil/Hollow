using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Consumable : ScriptableObject {

	public int itemID; // ID para referência ao objeto
	public string itemName; // Nome do item para visualização
	public string description; // Descrição do item para visualização
	public Sprite image; // Imagem para representar visualmente o item
	public int hpGain; // Quantidade de pontos de vida recuperada pelo item
	public int mpGain; // Quantidade de pontos de mana recuperada pelo item
	public string message; // Mensagem exibida ao coletar o item
}

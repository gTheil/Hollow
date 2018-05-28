using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Key : ScriptableObject {

	public int itemID; // ID da chave, usado para fazer referência ao objeto
	public string keyName; // Nome da chave, para representação
	public string description; // Descrição da chave dentro do inventário do jogador
	public Sprite image; // Imagem para representação da chave tanto no cenário como no inventário
	public string message; // Mensagem exibida ao coletar a chave


}

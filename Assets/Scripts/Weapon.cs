using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Facilita criação de instâncias de um objeto através do menu Assets/Create do Unity
public class Weapon : ScriptableObject {

	public int itemID; // ID para referência ao objeto
	public string weaponName; // Nome da arma para visualização
	public string description; // Descrição da arma para visualização
	public int attack; // Valor de ataque da arma, utilizado no cálculo do dano causado aos inimigos pelo personagem
	public Sprite image; // Imagem para representar visualmente a arma em menus
	public AnimationClip animation; // Animação reproduzida ao atacar com a arma
	public string message; // Mensagem exibida ao adquirir a arma

}

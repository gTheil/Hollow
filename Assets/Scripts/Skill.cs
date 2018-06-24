using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Facilita criação de instâncias de um objeto através do menu Assets/Create do Unity
public class Skill : ScriptableObject {

	public int skillID; // ID para referência ao objeto
	public string skillName; // Nome da habilidade para visualização
	public string description; // Descrição da habilidade para visualização
	public Sprite image; // Imagem para representar visualmente a habilidade
	public string message; // Mensagem exibida ao desbloquear a habilidade
}

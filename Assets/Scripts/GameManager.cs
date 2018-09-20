using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// Dados do jogador a serem salvos
[Serializable]
class PlayerData {
	
	public int maxHP; // Vida máxima
	public int maxMP; // Mana máxima
	public int gold; // Ouro
	public float positionX, positionY; // Posição do personagem em X e Y
	public float cameraMinX, cameraMaxX, cameraMinY, cameraMaxY; // Posições mínimas e máximas da câmera em X e Y
	public int[] playerSkills; // Conjunto de habilidades
	public int[] playerConsumables; // Conjunto de consumíveis
	public int[] playerKeys; // Conjunto de chaves
	public int equipWepID; // Arma equipada
	public int equipArmID; // Armadura equipada
	public bool jumpSkill; // Se o jogador possui habilidade de pulo
	public bool attackSkill; // Se o jogador possui habilidade de ataque
	public bool deathSkill; // Se o jogador possui habilidade de morte
	public int[] shopSkills; // Conjunto de habilidades da loja
}

public class GameManager : MonoBehaviour {

	public static GameManager gm; // instância única do GameManager

	// Dados do jogador
	public int maxHP = 100; // Vida máxima
	public int maxMP = 50; // Mana máxima
	public int gold = 0; // Ouro
	public float positionX, positionY; // Posição do personagem em X e Y
	public float cameraMinX, cameraMaxX, cameraMinY, cameraMaxY; // Posições mínimas e máximas da câmera em X e Y
	public int[] playerSkills; // Conjunto de habilidades
	public int[] playerConsumables; // Conjunto de consumíveis
	public int[] playerKeys; // Conjunto de chaves
	public int equipWepID; // Arma equipada
	public int equipArmID; // Armadura equipada
	public bool jumpSkill = false; // Se o jogador possui habilidade de pulo
	public bool attackSkill = false; // Se o jogador possui habilidade de ataque
	public bool deathSkill = false; // Se o jogador possui habilidade de morte
	public int[] shopSkills; // Conjunto de habilidades da loja

	private string filePath; // Caminho onde o arquivo de save deve ser salvo

	// Inicialização, impede que haja mais de um game manager para impedir conflitos nos dados do personagem
	void Awake () {
		if (gm == null)
			gm = this;
		else if (gm != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		filePath = Application.persistentDataPath + "/playerInfo.dat"; // Define o caminho onde deve ser salvo o arquivo de save

		Load(); // Carrega os dados
	}

	// Método que salva os dados do personagem
	public void Save() {

		Player player = FindObjectOfType<Player>(); // Referência ao personagem
		CameraFollow camera = FindObjectOfType<CameraFollow>(); // Referência oa script que controla a movimentação da câmera

		// Instancia vetores para todos os tipos de item encontrados no inventário do jogador
		playerSkills = new int[PlayerInventory.playerInventory.skills.Count]; // Habilidades
		playerConsumables = new int[PlayerInventory.playerInventory.consumables.Count]; // Consumíveis
		playerKeys = new int[PlayerInventory.playerInventory.keys.Count]; // Chaves

		// Salva todas as habilidades encontradas no inventário
		for (int i = 0; i < playerSkills.Length; i++) {
			playerSkills[i] = PlayerInventory.playerInventory.skills[i].skillID;
		}

		// Salva todos os consumíveis encontrados no inventário
		for (int i = 0; i < playerConsumables.Length; i++) {
			playerConsumables[i] = PlayerInventory.playerInventory.consumables[i].itemID;
		}

		// Salva todas as chaves encontradas no inventário
		for (int i = 0; i < playerKeys.Length; i++) {
			playerKeys[i] = PlayerInventory.playerInventory.keys[i].itemID;
		}

		// Salva todas as habilidades encontradas no inventário da loja
		for (int i = 0; i < shopSkills.Length; i++) {
			shopSkills[i] = PlayerInventory.shopInventory.skills[i].skillID;
		}

		BinaryFormatter bf = new BinaryFormatter(); // Usado para serializar o arquivo em formato binário
		FileStream file = File.Create(filePath); // Criação do arquivo de save

		PlayerData data = new PlayerData(); // Referência aos dados do jogador

		// Salva os dados atuais do personagem ao arquivo
		data.maxHP = player.maxHP; // Vida máxima
		data.maxMP = player.maxMP; // Mana máxima
		data.gold = player.gold; // Ouro
		data.positionX = player.transform.position.x; // Posição em X
		data.positionY = player.transform.position.y; // Posição em Y
		data.cameraMinX = camera.minXAndY.x; // Posição mínima da câmera em X
		data.cameraMaxX = camera.maxXAndY.x; // Posição máxima da câmera em X
		data.cameraMinY = camera.minXAndY.y; // Posição mínima da câmera em Y
		data.cameraMaxY = camera.maxXAndY.y; // Posição máxima da câmera em Y

		// Caso o personagem tiver alguma arma equipada
		if (player.weaponEquipped != null)
			data.equipWepID = player.weaponEquipped.itemID; // Arma atual
		
		// Caso o personagem tiver alguma armadura equipada
		if (player.armorEquipped != null)
			data.equipArmID = player.armorEquipped.itemID; // Armadura atual

		data.jumpSkill = player.jumpSkill; // Se o jogador possui habilidade de pulo
		data.attackSkill = player.attackSkill; // Se o jogador possui habilidade de ataque
		data.deathSkill = player.deathSkill; // Se o jogador possui habilidade de morte

		data.playerSkills = playerSkills; // Habilidades
		data.playerConsumables = playerConsumables; // Consumíveis
		data.playerKeys = playerKeys; // Chaves
		data.shopSkills = shopSkills; // Habilidades da loja

		bf.Serialize(file, data); // Salva, no arquivo de save, os dados do personagem

		file.Close(); // Fecha o arquivo de save ao terminar de salvar
	}

	// Método que carrega os dados do personagem
	public void Load() {
		if (File.Exists(filePath)) { // Carrega os dados caso o arquivo de save exista
			BinaryFormatter bf = new BinaryFormatter(); // Usado para desserializar o arquivo em formato binário
			FileStream file = File.Open(filePath, FileMode.Open); // Leitura do arquivo de save

			PlayerData data = (PlayerData)bf.Deserialize(file); // Recupera, do arquivo de save, os dados do personagem
			file.Close(); // Fecha o arquivo após ser lido

			// Atribui aos dados do personagem os valores dos dados do arquivo de save
			maxHP = data.maxHP; // Vida máxima
			maxMP = data.maxMP; // Mana máxima
			gold = data.gold; // Ouro
			positionX = data.positionX; // Posição em X
			positionY = data.positionY; // Posição em Y
			cameraMinX = data.cameraMinX; // Posição mínima da câmera em X
			cameraMaxX = data.cameraMaxX; // Posição máxima da câmera em X
			cameraMinY = data.cameraMinY; // Posição mínima da câmera em Y
			cameraMaxY = data.cameraMaxY; // Posição máxima da câmera em Y
			equipWepID = data.equipWepID; // Arma atual
			equipArmID = data.equipArmID; // Armadura atual
			jumpSkill = data.jumpSkill; // Se o jogador possui habilidade de pulo
			attackSkill = data.attackSkill; // Se o jogador possui habilidade de ataque
			deathSkill = data.deathSkill; // Se o jogador possui habilidade de morte
			playerSkills = data.playerSkills; // Habilidades
			playerConsumables = data.playerConsumables; // Consumíveis
			playerKeys = data.playerKeys; // Chaves
			shopSkills = data.shopSkills; // Habilidades da loja
		}
	}
}

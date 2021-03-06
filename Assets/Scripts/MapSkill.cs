﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSkill : MonoBehaviour {

	public Skill skill; // Referência a habilidade associada ao objeto no mapa

	private SpriteRenderer sprite; // Imagem utilizada pelo objeto no mapa

	// Inicialização
	void Start () {
		// Atribui ao objeto no mapa a imagem da habilidade associada a ele
		sprite = GetComponent<SpriteRenderer>();
		sprite.sprite = skill.image;
	}

	// Chamado ao personagem entrar em contato com o objeto no mapa
	private void OnTriggerEnter2D(Collider2D other){
		// Verifica se foi o personagem que entrou em contato com o objeto
		Player player = other.GetComponent<Player>();
		if (player != null) {
			PlayerInventory.playerInventory.AddSkill(skill); // Adiciona ao inventário do jogador a habilidade associada ao objeto cujo personagem entrou em contato 
			Destroy(gameObject); // Remove o objeto do mapa
		}
	}
}

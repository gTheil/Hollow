using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	private Animator anim; // Gerencia as animações
	private int damage; // Dano causado pelo ataque aos inimigos

	// Inicialização
	void Start () {
		anim = GetComponent<Animator>();
	}

	// Método chamado para executar uma animação
	public void PlayAnimation(AnimationClip clip){
		anim.Play(clip.name);
	}

	// Adiciona o ataque da arma ao dano total do ataque
	public void SetDamage(int weaponAttack){
		damage = weaponAttack;
	}
}

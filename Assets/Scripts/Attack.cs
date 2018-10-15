using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	private Animator anim; // Gerencia as animações de ataque
	private int damage; // Dano causado pelo ataque aos inimigos
	private Player player;

	// Inicialização
	void Start () {
		anim = GetComponent<Animator>(); // Inicializa o gerenciador de animações
		player = GetComponentInParent<Player>();
	}

	// Método chamado para executar uma animação
	public void PlayAnimation(AnimationClip clip){
		anim.Play(clip.name);
	}

	// Seta o dano do ataque
	public void SetDamage(int damageValue){
		damage = damageValue;
	}

	// Retorna o dano do ataque
	public int GetDamage() {
		return damage;
	}

	// Método que causa dano ao inimigo quando este entrar em contato com o ataque
	private void OnTriggerEnter2D(Collider2D other) {
		Enemy enemy = other.GetComponent<Enemy>();
		if (enemy != null) {
			if (player.redBuff)
				enemy.TakeDamage((damage) * 2);
			else
				enemy.TakeDamage(damage);
			if (enemy.redBuffOn && !player.redBuffSpell)
				player.SetPlayerSpell (player.database.GetSpell (2));
			else if (enemy.blueBuffOn && !player.blueBuffSpell)
				player.SetPlayerSpell (player.database.GetSpell (3));
		}
	}
}

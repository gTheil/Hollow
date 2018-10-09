using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panther : Enemy {
	// Atualizado a cada frame
	void FixedUpdate () {
		if (!isDead){ // Caso o inimigo esteja vivo
			playerDistance = player.transform.position - transform.position; // Atribui à variável de distância a diferença entre as posições do personagem e do inimigo

			if (Mathf.Abs (playerDistance.x) < wakeUpX && Mathf.Abs (playerDistance.y) < wakeUpY) { // Caso o personagem esteja a determinada distância do inimigo
				rb.velocity = new Vector2 (speed * (playerDistance.x / Mathf.Abs(playerDistance.x)), rb.velocity.y); // Seta a velocidade na qual o RigidBody do inimigo se moverá
			}

			anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // O inimigo se move, com animação, horizontalmente na determinada velocidade

			if (rb.velocity.x > 0 && !facingRight) // Se o inimigo estiver virado para a esquerda e se movimentar para a direita
				Flip(); // Vira sua imagem a direita
			else if (rb.velocity.x < 0 && facingRight) // Se o inimigo estiver virado para a direita e se movimentar para a esquerda
				Flip(); // Vira sua imagem a esquerda
		}
	}
}

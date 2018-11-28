using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPanther : Enemy {

	public int jumpForce;
	public float jumpRate;

	private float nextJump;

	// Atualizado a cada frame
	void FixedUpdate () {
		if (!isDead) { // Caso o inimigo esteja vivo
			playerDistance = player.transform.position - transform.position; // Atribui à variável de distância a diferença entre as posições do personagem e do inimigo

			if (playerDistance.x < 0 && facingRight)
				Flip ();
			else if (playerDistance.x > 0 && !facingRight)
				Flip ();

			if (Mathf.Abs (playerDistance.x) < wakeUpX && Mathf.Abs (playerDistance.y) < wakeUpY) { // Caso o personagem esteja a determinada distância do inimigo
				woke = true;
			} else {
				woke = false;
			}

			if (woke) {
				if (buffable && !isBuffed) {
					RedBuffUse ();
				}
				if (redBuffOn)
					rb.velocity = new Vector2 ((speed * 1.5f) * (playerDistance.x / Mathf.Abs (playerDistance.x)), rb.velocity.y); // Seta a velocidade na qual o RigidBody do inimigo se moverá
				else
					rb.velocity = new Vector2 (speed * (playerDistance.x / Mathf.Abs (playerDistance.x)), rb.velocity.y);
				if (Time.time > nextJump) {
					Jump ();
				}
				if (redBuffOn && Time.time > buffEnd) {
					audioBuff.Play();
					redBuffOn = false;
					FindObjectOfType<UIManager> ().SetMessage ("Inimigo Desativou Adrenalina!");
				}
				anim.SetFloat ("Speed", Mathf.Abs (rb.velocity.x)); // O inimigo se move, com animação, horizontalmente na determinada velocidade

				if (rb.velocity.x > 1 && !facingRight && playerDistance.x > 0) // Se o inimigo estiver virado para a esquerda e se movimentar para a direita
					Flip (); // Vira sua imagem a direita
				else if (rb.velocity.x < 1 && facingRight && playerDistance.x < 0) // Se o inimigo estiver virado para a direita e se movimentar para a esquerda
					Flip (); // Vira sua imagem a esquerda
			} else {
				rb.velocity = new Vector2 (0, 0);
				rb.AddForce (Vector2.down * (jumpForce / 2));
				anim.SetFloat ("Speed", 0);
			}
		}
	}

	void Jump() {
		rb.AddForce(Vector2.up * jumpForce);
		nextJump = Time.time + jumpRate;
	}

	void RedBuffUse() {
		audioBuff.Play();
		redBuffOn = true;
		FindObjectOfType<UIManager> ().SetMessage ("Inimigo Ativou Adrenalina!");
		isBuffed = true;
		buffEnd = Time.time + buffDuration;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkeleton : Enemy {

	public GameObject fireball;
	public float fireRate;

	private float nextFire;
	private Vector2 fireballPos;

	// Atualizado a cada frame
	void FixedUpdate () {
		if (!isDead) { // Caso o inimigo esteja vivo
			fireballPos = transform.position;
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
					BlueBuffUse ();
				}
				if (blueBuffOn)
					rb.velocity = new Vector2 ((speed / 2) * (playerDistance.x / Mathf.Abs (playerDistance.x)), rb.velocity.y); // Seta a velocidade na qual o RigidBody do inimigo se moverá
				else
					rb.velocity = new Vector2 (speed * (playerDistance.x / Mathf.Abs (playerDistance.x)), rb.velocity.y);
				if (Time.time > nextFire) {
					FireballUse ();
				}
				if (blueBuffOn && Time.time > buffEnd) {
					blueBuffOn = false;
					FindObjectOfType<UIManager> ().SetMessage ("Inimigo Desativou Fortaleza!");
				}
				anim.SetFloat ("Speed", Mathf.Abs (rb.velocity.x)); // O inimigo se move, com animação, horizontalmente na determinada velocidade

				if (rb.velocity.x > 1 && !facingRight) // Se o inimigo estiver virado para a esquerda e se movimentar para a direita
					Flip (); // Vira sua imagem a direita
				else if (rb.velocity.x < 1 && facingRight) // Se o inimigo estiver virado para a direita e se movimentar para a esquerda
					Flip (); // Vira sua imagem a esquerda
			} else {
				rb.velocity = new Vector2 (0, 0);
				anim.SetFloat ("Speed", 0);
			}
		}
	}

	void FireballUse() {
		if (facingRight)
			fireballPos.x += 0.3f;
		else
			fireballPos.x -= 0.3f;
		Instantiate(fireball, fireballPos, Quaternion.identity);
		nextFire = Time.time + fireRate;
	}

	void BlueBuffUse() {
		blueBuffOn = true;
		FindObjectOfType<UIManager> ().SetMessage ("Inimigo Ativou Fortaleza!");
		isBuffed = true;
		buffEnd = Time.time + buffDuration;
	}
}

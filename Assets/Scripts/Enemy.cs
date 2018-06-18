using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float wakeUpX; // Determina a distância em X que o personagem deve estar do inimigo para que o mesmo o persiga
	public float wakeUpY; // Determina a distância em Y que o personagem deve estar do inimigo para que o mesmo o persiga
	public float speed; // Determina a velocidade com que o inimigo se move

	private Transform player; // Referência ao personagem
	private Rigidbody2D rb; // Componente que adiciona física
	private Animator anim; // Gerenciador de animação
	private Vector3 playerDistance; // Determina a distãncia entre o inimigo e o personagem
	private bool facingRight = false; // Determina a direção em que o inimigo está virado

	// Inicialização
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform; // Lê a posição do personagem no cenário
		rb = GetComponent<Rigidbody2D>(); // Atribui à variável rb o RigidBody do inimigo
		anim = GetComponent<Animator>(); // Atribui à variável anim o Animator do inimigo
	}
	
	// Atualizado a cada frame
	void Update () {
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

	private void Flip () {
		facingRight = !facingRight; // Inverte o valor de facingRight, fazendo o inimigo virar a esquerda se estiver virado a direita e vice-versa
		Vector3 scale = transform.localScale; // Recebe o valor da escala do inimigo
		scale.x *= -1; // Inverte o valor x (horizontal) da escala entre valores positivos e negativos
		transform.localScale = scale; // Atualiza o valor da escala do inimigo com o novo valor positivo ou negativo pra virar sua imagem
	}
}

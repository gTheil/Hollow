﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float maxSpeed; // Velocidade na qual o personagem se move
	public Transform groundCheck; // Verifica se o personagem encontra-se no chão
	public float jumpForce; // Força vertical aplicada ao RigidBody do personagem durante o pulo
	public float fireRate; // Intervalo de tempo durante o qual um ataque é executado

	private Rigidbody2D rb; // RigidBody, componente que adiciona física
	private float speed; // Velocidade atual do personagem
	private bool facingRight = true; // Determina a direção em que o personagem está virado; ao inicializar, o personagem estará virado para a direita
	private bool onGround; // Determina se o personagem está no chão ou no ar
	private bool jump; // Determina se o personagem está em estado de pulo
	private Weapon weaponEquipped; // Referência a arma atualmente equipada no personagem
	private Animator anim; // Gerencia as animações do personagem
	private Attack attack; // Referência ao script de ataque para facilitar a chamada da animação da arma
	private float nextAttack; // Contagem para possibilitar o personagem a atacar novamente

	// Inicialização
	void Start () {
		rb = GetComponent<Rigidbody2D> (); // Atribui a rb o valor do componente RigidBody dado ao GameObject Player
		speed = maxSpeed; // Inicializa a velocidade do personagem com a maxSpeed definida no GameObject
		anim = GetComponent<Animator>(); // Inicializa o gerenciador de animações
		attack = GetComponentInChildren<Attack>(); // Inicializa o componente "Attack"
	}
	
	// Atualiza a cada frame
	void Update () {
		onGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")); // Dispara uma linha da posição atual do personagem até a posição do chão

		// Se o comando de pulo (barra de espaço) for executado enquanto o personagem estiver no chão
		if (Input.GetButtonDown ("Jump") && (onGround)) {
			Jump (); // O personagem realizará um pulo
			jump = true; // Ele será permitido realizar um segundo pulo
		}
		// Se o comando de pulo (barra de espaço) for executado no ar durante o estado de pulo
		if (Input.GetButtonDown ("Jump") && (!onGround) && jump == true) {
			Jump (); // O personagem realizará um segundo pulo
			jump = false; // Ele será impedido de executar pulos subsequentes
		}
		// Se o comando de ataque 1 (botão esquerdo do mouse) for executado enquanto o personagem tem uma arma equipada e sua contagem para o próximo ataque já foi encerrada
		if (Input.GetButtonDown ("Fire1") && Time.time > nextAttack && weaponEquipped != null) {
			Attack(); // O personagem realizará um ataque
		}
	}

	// Atualiza em um intervalo específico
	void FixedUpdate () {
		float h = Input.GetAxisRaw("Horizontal"); // Variável que controla a posição X (horizontal) do personagem
		rb.velocity = new Vector2(h * speed, rb.velocity.y); // Variável que controla o movimento na horizontal e vertical do personagem

		// Se a posição horizontal for maior que 0, ou seja, o personagem estiver se movendo para a direita, a função Flip é chamada para virar a imagem do personagem
		if (h > 0 && !facingRight) {
			Flip();
		}

		// Se a posição horizontal for menor que 0, ou seja, o personagem estiver se movendo para a esquerda, a função Flip é chamada para virar a imagem do personagem
		else if (h < 0 && facingRight) {
			Flip();
		}
	}

	// Efetua o pulo do personagem
	void Jump () {
		rb.velocity = Vector2.zero; // Zera a velocidade do personagem
		rb.AddForce(Vector2.up * jumpForce); // Aplica uma força vertical ao personagem
	}

	// Efetua a inversão da imagem do personagem
	void Flip () {
		facingRight = !facingRight; // Inverte o valor de facingRight, fazendo o personagem virar a esquerda se estiver virado a direita e vice-versa
		Vector3 scale = transform.localScale; // Recebe o valor da escala do personagem
		scale.x *= -1; // Inverte o valor x (horizontal) da escala entre valores positivos e negativos
		transform.localScale = scale; // Atualiza o valor da escala do personagem com o novo valor positivo ou negativo pra virar sua imagem
	}

	// Método chamado ao coletar uma arma
	public void AddWeapon (Weapon weapon) {
		weaponEquipped = weapon; // Equipa a arma no personagem
		attack.SetWeapon(weaponEquipped.damage); // Passa o valor de dano da arma equipada ao dano do ataque
	}

	// Efetua o ataque do personagem
	void Attack () {
		anim.SetTrigger("Attack"); // Roda a animação de ataque do personagem
		attack.PlayAnimation(weaponEquipped.animation); // Roda a animação de ataque da arma equipada
		nextAttack = Time.time + fireRate; // O personagem deverá esperar o tempo de ataque para poder atacar novamente
	}

}
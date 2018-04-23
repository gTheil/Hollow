using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float maxSpeed; // Velocidade na qual o personagem se move
	public Transform groundCheck; // Verifica se o personagem encontra-se no chão
	public float jumpForce; // Força vertical aplicada ao RigidBody do personagem durante o pulo

	private Rigidbody2D rb; // RigidBody, componente que adiciona física
	private float speed; // Velocidade atual do personagem
	private bool facingRight = true; // Determina a direção em que o personagem está virado; ao inicializar, o personagem estará virado para a direita
	private bool onGround; // Determina se o personagem está no chão ou no ar
	private bool jump; // Determina se o personagem está em estado de pulo
	private Weapon equippedWeapon; // Verifica a arma atualmente equipada no personagem

	// Inicialização
	void Start () {
		rb = GetComponent<Rigidbody2D> (); // Atribui a rb o valor do componente RigidBody dado ao GameObject Player
		speed = maxSpeed; // Inicializa a velocidade do personagem com a maxSpeed definida no GameObject
	}
	
	// Atualiza a cada frame
	void Update () {
		onGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")); // Dispara uma linha da posição atual do personagem até a posição do chão

		// Se o botão de pulo for pressionado enquanto o personagem estiver no chão, ele estará em estado de pulo
		if (Input.GetButtonDown ("Jump") && (onGround)) {
			Jump ();
			jump = true;
		}
		// Se o botão de pulo for pressionado no ar durante o estado de pulo, o personagem realizará um segundo pulo, e depois será impedido de realizar pulos subsequentes
		if (Input.GetButtonDown ("Jump") && (!onGround) && jump == true) {
			Jump ();
			jump = false;
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

	// Método chamado ao equipar uma arma
	public void AddWeapon (Weapon weapon) {
		equippedWeapon = weapon; // Equipa a arma no personagem
		GetComponentInChildren<Attack>().SetDamage(equippedWeapon.attack); // Passa o valor de ataque da arma ao dano total do ataque
	}

}

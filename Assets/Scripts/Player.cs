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
	private bool jump = false; // Determina se o personagem está num estado de pulo

	// Inicialização
	void Start () {
		rb = GetComponent<Rigidbody2D> (); // Atribui a rb o valor do componente RigidBody dado ao GameObject Player
		speed = maxSpeed; // Inicializa a velocidade do personagem com a maxSpeed definida no GameObject
	}
	
	// Atualiza a cada frame
	void Update () {
		onGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")); // Dispara uma linha da posição atual do personagem até a posição do chão

		// Se o botão de pulo for pressionado enquanto o personagem estiver no chão, ele estará em estado de pulo
		if (Input.GetButtonDown ("Jump") && (onGround))
			jump = true;
	}

	// Atualiza em um intervalo específico
	void FixedUpdate () {
		float h = Input.GetAxisRaw("Horizontal"); // Variável que controla a posição X (horizontal) do personagem
		rb.velocity = new Vector2(h * speed, rb.velocity.y); // A velocidade do RigidBody na horizontal é função da posição do personagem e sua velocidade atual

		// Se a posição horizontal for maior que 0, ou seja, o personagem estiver se movendo para a direita, a função Flip é chamada para virar a imagem do personagem
		if (h > 0 && !facingRight) {
			Flip();
		}

		// Se a posição horizontal for menor que 0, ou seja, o personagem estiver se movendo para a esquerda, a função Flip é chamada para virar a imagem do personagem
		else if (h < 0 && facingRight) {
			Flip();
		}

		// Ao personagem pular, sua velocidade é zerada e uma força vertical é aplica a seu RigidBody, logo em seguida acaba-se o estado de pulo
		if (jump) {
			rb.velocity = Vector2.zero;
			rb.AddForce(Vector2.up * jumpForce);
			jump = false;
		}
	}

	// Chamada ao personagem se virar da direita para a esquerda ou vice-versa
	void Flip () {
		facingRight = !facingRight; // Inverte o valor de facingRight, fazendo o personagem virar a esquerda se estiver virado a direita e vice-versa
		Vector3 scale = transform.localScale; // Recebe o valor da escala do personagem
		scale.x *= -1; // Inverte o valor x (horizontal) da escala entre valores positivos e negativos
		transform.localScale = scale; // Atualiza o valor da escala do personagem com o novo valor positivo ou negativo pra virar sua imagem
	}
}

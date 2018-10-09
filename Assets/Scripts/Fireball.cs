using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	public float speed; // Determina a velocidade com que a bola de fogo se move
	public int attack; // Valor de dano que a bola de fogo causará ao personagem quando se colidirem
	public float knockback; // Distância que a bola de fogo empurrará o personagem quando se colidirem

	private Transform player; // Referência ao personagem
	private Rigidbody2D rb; // Componente que adiciona física
	private Animator anim; // Gerenciador de animação
	private Vector3 playerDistance; // Determina a distãncia entre a bola de fogo e o personagem

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform; // Lê a posição do personagem no cenário
		rb = GetComponent<Rigidbody2D>(); // Inicializa a física da bola de fogo
		anim = GetComponent<Animator>(); // Inicializa o gerenciador de animação da bola de fogo
		playerDistance = player.transform.position - transform.position; // Atribui à variável de distância a diferença entre as posições do personagem e da bola de fogo

		if (playerDistance.x < 0) {
			Vector3 scale = transform.localScale; // Recebe o valor da escala da bola de fogo
			scale.x *= -1; // Inverte o valor x (horizontal) da escala
			transform.localScale = scale; // Atualiza o valor da escala da bola de fogo com o novo valor para virar sua imagem
			speed = speed * -1;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
			rb.velocity = new Vector2(speed, rb.velocity.y);
	}

	protected void OnCollisionEnter2D(Collision2D other) {
		Destroy(gameObject);
		Player player = other.gameObject.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage(attack); // Causa dano ao personagem
			player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * knockback * (playerDistance.x / Mathf.Abs(playerDistance.x)), ForceMode2D.Impulse); // Empurra o personagem uma determinada distância
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float wakeUpX; // Determina a distância em X que o personagem deve estar do inimigo para que o mesmo o persiga
	public float wakeUpY; // Determina a distância em Y que o personagem deve estar do inimigo para que o mesmo o persiga
	public float speed; // Determina a velocidade com que o inimigo se move
	public int hp; // Valor de vida do inimigo
	public GameObject drop; // Referência ao item que o inimigo deixará para trás ao morrer
	public Consumable consumable; // O item específico que será deixado para trás
	public int attack; // Valor de dano que o inimigo causará ao personagem quando se colidirem
	public float knockback; // Distância que o inimigo empurrará o personagem quando se colidirem

	private Transform player; // Referência ao personagem
	private Rigidbody2D rb; // Componente que adiciona física
	private Animator anim; // Gerenciador de animação
	private Vector3 playerDistance; // Determina a distãncia entre o inimigo e o personagem
	private bool facingRight = false; // Determina a direção em que o inimigo está virado
	private bool isDead = false; // Determina se o inimigo está morto
	private SpriteRenderer sprite; // Referência à imagem do inimigo

	// Inicialização
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform; // Lê a posição do personagem no cenário
		rb = GetComponent<Rigidbody2D>(); // Inicializa a física do inimigo
		anim = GetComponent<Animator>(); // Inicializa o gerenciador de animação do inimigo
		sprite = GetComponent<SpriteRenderer>(); // Inicializa a imagem do inimigo
	}
	
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

	// Método que vira a imagem do inimigo
	private void Flip () {
		facingRight = !facingRight; // Inverte o valor de facingRight, fazendo o inimigo virar a esquerda se estiver virado a direita e vice-versa
		Vector3 scale = transform.localScale; // Recebe o valor da escala do inimigo
		scale.x *= -1; // Inverte o valor x (horizontal) da escala entre valores positivos e negativos
		transform.localScale = scale; // Atualiza o valor da escala do inimigo com o novo valor positivo ou negativo pra virar sua imagem
	}

	// Método chamado ao inimigo entrar em contato com um ataque do personagem
	public void TakeDamage(int damage) {
		hp -= damage; // Diminui a vida do inimigo pelo dano do ataque
		if (hp <= 0) { // Caso sua vida chegue a zero
			isDead = true; // Colocado em estado de morte
			rb.velocity = Vector2.zero; // Seu corpo para de se mover
			anim.SetTrigger("Dead"); // Roda sua animação de morte
			if (consumable != null) { // Caso algum consumível tenha sido atribuído ao inimigo
				GameObject tempItem = Instantiate(drop, transform.position, transform.rotation); // Instancia um objeto no cenário
				tempItem.GetComponent<MapConsumable>().consumable = consumable; // Atribui ao objeto um consumível
			}
		} else { // Caso contrário
			StartCoroutine(DamageCoroutine()); // Roda a rotina de dano
		}
	}

	// Rotina de dano
	IEnumerator DamageCoroutine() {
		// Faz com que a imagem do inimigo torne-se vermelha brevemente
		for (float i = 0; i < 0.2f; i+= 0.2f) {
			sprite.color = Color.red;
			yield return new WaitForSeconds(0.1f);
			sprite.color = Color.white;
			yield return new WaitForSeconds(0.1f);
		}
	}

	// Método que remove o inimigo do cenário
	public void DestroyEnemy() {
		Destroy(gameObject);
	}

	// Método chamado ao inimigo colidir com o personagem
	private void OnCollisionEnter2D(Collision2D other) {
		Player player = other.gameObject.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage(attack); // Causa dano ao personagem
			player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * knockback * (playerDistance.x / Mathf.Abs(playerDistance.x)), ForceMode2D.Impulse); // Empurra o personagem uma determinada distância
		}
	}
}

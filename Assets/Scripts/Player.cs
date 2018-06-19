using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float maxSpeed; // Velocidade na qual o personagem se move
	public Transform groundCheck; // Verifica se o personagem encontra-se no chão
	public float jumpForce; // Força vertical aplicada ao RigidBody do personagem durante o pulo
	public float fireRate; // Intervalo de tempo durante o qual um ataque é executado
	public Consumable consumable; // Consumível que estará presente na barra de status do personagem para uso rápido ao ser selecionado
	public int maxHP; // Valor máximo de vida do personagem
	public int maxMP; // Valor máximo de mana do personagem
	public int def; // Valor de defesa do personagem
	public bool isPaused = false; // Determina se o menu está aberto
	public int gold; // Quantidade de ouro do personagem

	private Rigidbody2D rb; // RigidBody, componente que adiciona física
	private float speed; // Velocidade atual do personagem
	private bool facingRight = true; // Determina a direção em que o personagem está virado; ao inicializar, o personagem estará virado para a direita
	private bool onGround; // Determina se o personagem está no chão ou no ar
	private bool jump; // Determina se o personagem está em estado de pulo
	private Weapon weaponEquipped; // Referência a arma atualmente equipada no personagem
	private Armor armorEquipped; // Referência a armadura atualmente equipada no personagem
	private Animator anim; // Gerencia as animações do personagem
	private Attack attack; // Referência ao script de ataque para facilitar a chamada da animação da arma
	private float nextAttack; // Contagem para possibilitar o personagem a atacar novamente
	private int hp; // Valor atual de vida do personagem
	private int mp; // Valor atual de mana do personagem
	private bool canDamage = true; // Determina se o jogador pode receber dano
	private SpriteRenderer sprite; // Referência à imagem do jogador
	private bool isDead = false; // Determina se o personagem está morto

	// Inicialização
	void Start () {
		rb = GetComponent<Rigidbody2D> (); // Atribui a rb o valor do componente RigidBody dado ao GameObject Player
		speed = maxSpeed; // Inicializa a velocidade do personagem com a maxSpeed definida no GameObject
		anim = GetComponent<Animator>(); // Inicializa o gerenciador de animações
		attack = GetComponentInChildren<Attack>(); // Inicializa o componente "Attack"
		hp = maxHP; // Iguala a vida atual à total
		mp = maxMP; // Iguala a mana atual à total
		sprite = GetComponent<SpriteRenderer>(); // Inicializa a imagem do jogador
	}
	
	// Atualiza a cada frame
	void Update () {
		if (!isDead && !isPaused) { // Caso o personagem esteja vivo e o menu fechado 
			onGround = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")); // Dispara uma linha da posição atual do personagem até a posição do chão

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
				Attack (); // O personagem realizará um ataque
			}

			// Se o botão direito do mouse for pressionado e um consumível estiver equipado
			if (Input.GetButtonDown ("Fire2") && consumable != null) {
				QuickItem (consumable); // utiliza o item consumível equipado
			}
		}
	}

	// Atualiza em um intervalo específico
	void FixedUpdate () {
		if (!isDead && !isPaused) { // Caso o personagem esteja vivo e o menu fechado
			float h = Input.GetAxisRaw ("Horizontal"); // Variável que controla a posição X (horizontal) do personagem

			if (canDamage)
				rb.velocity = new Vector2 (h * speed, rb.velocity.y); // Variável que controla o movimento na horizontal e vertical do personagem

			// Se a posição horizontal for maior que 0, ou seja, o personagem estiver se movendo para a direita, a função Flip é chamada para virar a imagem do personagem
			if (h > 0 && !facingRight) {
				Flip ();
			}

		// Se a posição horizontal for menor que 0, ou seja, o personagem estiver se movendo para a esquerda, a função Flip é chamada para virar a imagem do personagem
		else if (h < 0 && facingRight) {
				Flip ();
			}
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
		attack.SetDamage(weaponEquipped.damage); // Passa o valor de dano da arma equipada ao dano do ataque
	}

	// Método chamado ao coletar uma armadura
	public void AddArmor (Armor armor) {
		armorEquipped = armor; // Equipa a armadura no personagem
		def = armorEquipped.defense; // Passa o valor de defesa da armadura equipada à defesa do jogador
	}

	// Efetua o ataque do personagem
	void Attack () {
		anim.SetTrigger("Attack"); // Roda a animação de ataque do personagem
		attack.PlayAnimation(weaponEquipped.animation); // Roda a animação de ataque da arma equipada
		nextAttack = Time.time + fireRate; // O personagem deverá esperar o tempo de ataque para poder atacar novamente
	}

	// Utiliza o consumível na barra de status
	public void QuickItem (Consumable consumable) {
		hp += consumable.hpGain; // Aumenta o valor de vida do personagem de acordo com o aumento de vida do consumível utilizado
		// Impede que o valor atual de vida do personagem ultrapasse o valor máximo
		if (hp >= maxHP) {
			hp = maxHP;
		}
		mp += consumable.mpGain; // Aumenta o valor de mana do personagem de acordo com o aumento de mana do consumível utilizado
		// Impede que o valor atual de mana do personagem ultrapasse o valor máximo
		if (mp >= maxMP) {
			mp = maxMP;
		}
		Inventory.inventory.RemoveConsumable(consumable); // Remove o consumível utilizado do inventário
	}

	// Método que retorna o valor de HP atual do personagem
	public int GetHP() {
		return hp;
	}

	// Método que retorna o valor de MP atual do personagem
	public int GetMP() {
		return mp;
	}

	// Método chamado ao personagem colidir com um inimigo
	public void TakeDamage(int damage) {
		if (canDamage) { // Caso o personagem possa receber dano
			canDamage = false; // É colocado em um estado de invencibilidade
			hp -= (damage - def); // Diminui a vida atual do personagem pela diferença entre o dano do inimigo e a defesa do personagem
			if (hp <= 0) { // Caso o dano recebido faça a vida atual do personagem ser menor ou igual a zero
				hp = 0; // Iguala sua vida a zero
				isDead = true; // O coloca em estado de morte
				anim.SetTrigger ("Dead"); // Roda a animação de morte do personagem
				Invoke ("ReloadScene", 3f); // Recarrega a cena após três segundos
			} else { // Caso contrário
				StartCoroutine(DamageCoroutine()); // Roda a rotina de dano
			}
		}
	}

	// Método chamado ao personagem morrer
	void ReloadScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex); // Recarrega a cena atual
	}

	// Rotina de dano
	IEnumerator DamageCoroutine() {
		// Faz com que a imagem do personagem pisque ao receber dano
		for (float i = 0; i < 0.6f; i+= 0.2f) {
			sprite.enabled = false;
			yield return new WaitForSeconds(0.1f);
			sprite.enabled = true;
			yield return new WaitForSeconds(0.1f);
		}

		canDamage = true; // Coloca o personagem em um estado no qual ele pode receber dano
	}
}

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
	public Spell spell; // Magia que estará presente na barra de status do personagem para uso rápido ao ser selecionada
	public int maxHP; // Valor máximo de vida do personagem
	public int maxMP; // Valor máximo de mana do personagem
	public int def; // Valor de defesa do personagem
	public int gold; // Quantidade de ouro do personagem
	public bool jumpSkill = false; // Determina se o personagem possui a habilidade Pulo
	public bool attackSkill = false; // Determina se o personagem possui a habilidade Ataque
	public bool deathSkill = false; // Determina se o personagem possui a habilidade Morte
	public bool doubleJumpSkill = false; // Determina se o personagem possui a habilidade Pulo Duplo
	public bool attackPlusSkill = false; // Determina se o personagem possui a habilidade Ataque Aprimorado
	public bool deathSaveSkill = false; // Determina se o personagem possui a habilidade Segunda Chance
	public bool fireballSpell = false;
	public bool redBuffSpell = false;
	public bool blueBuffSpell = false;
	public Skill skillJump; // Referência à habilidade Pulo que consta no inventário
	public Skill skillAttack; // Referência à habilidade Ataque que consta no inventário
	public Skill skillDeath; // Referência à habilidade Morte que consta no inventário
	public Skill skillDoubleJump; // Referência à habilidade Pulo Duplo que consta no inventário
	public Skill skillAttackPlus; // Referência à habilidade Ataque Aprimorado que consta no inventário
	public Skill skillDeathSave; // Referência à habilidade Segunda Chance que consta no inventário
	public Spell spellFireball;
	public Spell spellRedBuff;
	public Spell spellBlueBuff;
	public Weapon firstWeapon; // Referência a arma que será equipada automaticamente ao personagem desbloquear o Ataque
	public Weapon weaponEquipped; // Referência a arma atualmente equipada no personagem
	public Armor armorEquipped; // Referência a armadura atualmente equipada no personagem
	public Database database; // Referência ao script Database, que contém referências a todos os itens do jogo
	public GameObject fireball; // Referência à bola de fogo
	public bool facingRight = true; // Determina a direção em que o personagem está virado; ao inicializar, o personagem estará virado para a direita
	public bool redBuff = false;
	public GameManager gm; // Referência ao GameManager

	private Rigidbody2D rb; // RigidBody, componente que adiciona física
	private float speed; // Velocidade atual do personagem
	private bool onGround; // Determina se o personagem está no chão ou no ar
	private Animator anim; // Gerencia as animações do personagem
	private Attack attack; // Referência ao script de ataque para facilitar a chamada da animação da arma
	private float nextAttack; // Contagem para possibilitar o personagem a atacar novamente
	private int hp; // Valor atual de vida do personagem
	private int mp; // Valor atual de mana do personagem
	private bool canDamage = true; // Determina se o jogador pode receber dano
	private SpriteRenderer sprite; // Referência à imagem do jogador
	private bool isDead = false; // Determina se o personagem está morto
	private CameraFollow cameraFollow; // Referência ao script que controla a movimentação da câmera
	private bool jump = false;
	private bool deathSaveUsed; // Determina se a habilidade "Segunda Chance" foi ativada
	private bool blueBuff = false;

	// Inicialização
	void Start () {
		rb = GetComponent<Rigidbody2D> (); // Atribui a rb o valor do componente RigidBody dado ao GameObject Player
		anim = GetComponent<Animator>(); // Inicializa o gerenciador de animações
		attack = GetComponentInChildren<Attack>(); // Inicializa o componente "Attack"
		sprite = GetComponent<SpriteRenderer>(); // Inicializa a imagem do jogador
		cameraFollow = FindObjectOfType<CameraFollow>(); // Inicializa a posição da câmera
		SetPlayer();
	}
	
	// Atualiza a cada frame
	void Update () {
		if (!isDead && Time.time > nextAttack && !FindObjectOfType<UIManager>().pauseActive && !FindObjectOfType<UIManager>().shopActive) { // Caso o personagem esteja vivo e o menu fechado 
			onGround = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")); // Dispara uma linha da posição atual do personagem até a posição do chão

			if (onGround) {
				anim.SetBool ("Ground", true);
				jump = false;
			}

			// Concede ao personagem a habilidade Pulo e a adiciona ao seu inventário assim que ele sair do chão
			if (!jumpSkill && !onGround)
				SetPlayerSkill(database.GetSkill(1));

			// Se o comando de pulo (barra de espaço) for executado enquanto o personagem estiver no chão
			if (Input.GetKeyDown(KeyCode.UpArrow) && (onGround) && jumpSkill) {
				Jump (); // O personagem realizará um pulo
				jump = true; // Ele será permitido realizar um segundo pulo
			}


			// Se o comando de pulo (barra de espaço) for executado no ar durante o estado de pulo
			if (Input.GetKeyDown(KeyCode.UpArrow) && (!onGround) && doubleJumpSkill && jump) {
				Jump (); // O personagem realizará um segundo pulo
				jump = false; // Ele será impedido de executar pulos subsequentes
			}


			// Se o comando de ataque 1 (botão esquerdo do mouse) for executado enquanto o personagem tem uma arma equipada e sua contagem para o próximo ataque já foi encerrada
			if (Input.GetKeyDown(KeyCode.Space) && attackSkill && weaponEquipped != null) {
				Attack (); // O personagem realizará um ataque
			}

			if (Input.GetKeyDown (KeyCode.Z) && spell != null) {
				QuickSpell (spell);
			}

			// Se o botão direito do mouse for pressionado e um consumível estiver equipado
			if (Input.GetKeyDown(KeyCode.X) && consumable != null) {
				QuickItem (consumable); // utiliza o item consumível equipado
			}
		}
	}

	// Atualiza em um intervalo específico
	void FixedUpdate () {
		if (!isDead && Time.time > nextAttack) { // Caso o personagem esteja vivo e o menu fechado
			float h = Input.GetAxisRaw ("Horizontal"); // Variável que controla a posição X (horizontal) do personagem

			if (canDamage){
				if (redBuff)
					rb.velocity = new Vector2 (h * (speed * 2), rb.velocity.y); // Variável que controla o movimento na horizontal e vertical do personagem
				else if (blueBuff)
					rb.velocity = new Vector2 (h * (speed / 2), rb.velocity.y);
				else
					rb.velocity = new Vector2 (h * speed, rb.velocity.y);
			}

			if (onGround)
				anim.SetFloat("Speed", Mathf.Abs(h)); // Roda a animação de movimento enquanto o personagem estiver se movimentando

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
		if (!jump)
			anim.SetTrigger("Jump");
		else
			anim.SetTrigger("DoubleJump");
		anim.SetBool ("Ground", false);
		if (blueBuff)
			rb.AddForce(Vector2.up * (jumpForce / 1.5f));
		else
			rb.AddForce(Vector2.up * jumpForce);
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

	void FireballUse() {
		if (mp >= database.GetSpell(1).manaCost) {
			Instantiate (fireball, attack.transform.position, Quaternion.identity);
			anim.SetTrigger("Attack"); // Roda a animação de ataque do personagem
			nextAttack = Time.time + fireRate; // O personagem deverá esperar o tempo de ataque para poder atacar novamente
			mp -= database.GetSpell(1).manaCost;
		}
	}

	void RedBuffUse() {
		redBuff = !redBuff;
		if (redBuff) {
			mp -= database.GetSpell(2).manaCost;
			blueBuff = false;
			FindObjectOfType<UIManager> ().SetMessage ("Ativou Adrenalina!");
		} else
			FindObjectOfType<UIManager> ().SetMessage ("Desativou Adrenalina!");
	}

	void BlueBuffUse() {
		blueBuff = !blueBuff;
		if (blueBuff) {
			mp -= database.GetSpell(3).manaCost;
			redBuff = false;
			FindObjectOfType<UIManager> ().SetMessage ("Ativou Fortaleza!");
		} else
			FindObjectOfType<UIManager> ().SetMessage ("Desativou Fortaleza!");
	}

	// Utiliza o consumível na barra de status
	public void QuickItem (Consumable consumable) {
		if (Inventory.playerInventory.CountConsumable(consumable) > 0) {
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
		PlayerInventory.playerInventory.RemoveConsumable(consumable); // Remove o consumível utilizado do inventário
		}
	}

	public void QuickSpell (Spell spell) {
		if (spell.spellID == 1 && fireballSpell)
			FireballUse ();
		else if (spell.spellID == 2 && redBuffSpell)
			RedBuffUse ();
		else if (spell.spellID == 3 && blueBuffSpell)
			BlueBuffUse ();
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
		// Concede ao personagem a habilidade Ataque e a adiciona ao inventário assim que ele receber dano
		if (!attackSkill)
			SetPlayerSkill(database.GetSkill(2));
		if (canDamage) { // Caso o personagem possa receber dano
			canDamage = false; // É colocado em um estado de invencibilidade
			if (redBuff)
				hp -= ((damage - def) * 2);
			else if (blueBuff)
				hp -= ((damage - def) / 2);
			else
				hp -= (damage - def); // Diminui a vida atual do personagem pela diferença entre o dano do inimigo e a defesa do personagem
			if (hp <= 0) // Caso o dano recebido faça a vida atual do personagem ser menor ou igual a zero
			if (!deathSaveSkill || deathSaveUsed) {
				hp = 0; // Iguala sua vida a zero
			} else if (!deathSaveUsed) {
				hp = maxHP;
				deathSaveUsed = !deathSaveUsed;
			}
			if (deathSkill && hp == 0) {
					isDead = true; // O coloca em estado de morte
					cameraFollow.enabled = false; // Trava a câmera
					anim.SetTrigger ("Dead"); // Roda a animação de morte do personagem;
					Invoke ("ReloadScene", 3f); // Recarrega a cena após três segundos
				} else // Caso contrário
					StartCoroutine (DamageCoroutine ()); // Roda a rotina de dano
			}
		}

	// Método chamado ao personagem morrer
	void ReloadScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarrega a cena atual
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

	// Método que habilita as habilidades do personagem
	public void SetPlayerSkill(Skill skill) {
		if (skill.skillID == 1) {
			FindObjectOfType<UIManager> ().SetMessage (skillJump.message);
			jumpSkill = true;
			PlayerInventory.playerInventory.AddSkill (skillJump);
		} else if (skill.skillID == 2) {
			FindObjectOfType<UIManager> ().SetMessage (skillAttack.message);
			attackSkill = true;
			PlayerInventory.playerInventory.AddSkill (skillAttack);
			AddWeapon (firstWeapon);
		} else if (skill.skillID == 3) {
			FindObjectOfType<UIManager> ().SetMessage (skillDeath.message);
			deathSkill = true;
			PlayerInventory.playerInventory.AddSkill (skillDeath);
		} else if (skill.skillID == 4) {
			FindObjectOfType<UIManager> ().SetMessage (skillDoubleJump.message);
			doubleJumpSkill = true;
			PlayerInventory.playerInventory.AddSkill (skillDoubleJump);
		} else if (skill.skillID == 5) {
			FindObjectOfType<UIManager> ().SetMessage (skillAttackPlus.message);
			attackPlusSkill = true;
			PlayerInventory.playerInventory.AddSkill (skillAttackPlus);
			AddWeapon (database.GetWeapon (2));
		} else if (skill.skillID == 6) {
			FindObjectOfType<UIManager> ().SetMessage (skillDeathSave.message);
			deathSaveSkill = true;
			PlayerInventory.playerInventory.AddSkill (skillDeathSave);
		}
	}

	public void SetPlayerSpell(Spell spell) {
		if (spell.spellID == 1) {
			FindObjectOfType<UIManager> ().SetMessage (spellFireball.message);
			fireballSpell = true;
			PlayerInventory.playerInventory.AddSpell (spellFireball);
		} else if (spell.spellID == 2) {
			FindObjectOfType<UIManager> ().SetMessage (spellRedBuff.message);
			redBuffSpell = true;
			PlayerInventory.playerInventory.AddSpell (spellRedBuff);
		} else if (spell.spellID == 3) {
			FindObjectOfType<UIManager> ().SetMessage (spellBlueBuff.message);
			blueBuffSpell = true;
			PlayerInventory.playerInventory.AddSpell (spellBlueBuff);
		} 
	}

	public void SetPlayer() {
		Vector3 playerPosition = new Vector3(gm.positionX, gm.positionY, 0);
		transform.position = playerPosition;
		maxHP = gm.maxHP;
		maxMP = gm.maxMP;
		speed = maxSpeed;
		hp = maxHP;
		mp = maxMP;
		gold = gm.gold;
		jumpSkill = gm.jumpSkill;
		attackSkill = gm.attackSkill;
		deathSkill = gm.deathSkill;
		doubleJumpSkill = gm.doubleJumpSkill;
		attackPlusSkill = gm.attackPlusSkill;
		deathSaveSkill = gm.deathSaveSkill;
		fireballSpell = gm.fireballSpell;
		redBuffSpell = gm.redBuffSpell;
		blueBuffSpell = gm.blueBuffSpell;

		if (gm.equipWepID > 0)
			AddWeapon(PlayerInventory.playerInventory.database.GetWeapon(gm.equipWepID));
		if (gm.equipArmID > 0)
			AddArmor(PlayerInventory.playerInventory.database.GetArmor(gm.equipArmID));
	}
}

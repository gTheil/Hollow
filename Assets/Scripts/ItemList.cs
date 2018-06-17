using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour {

	public Image image; // Referência à imagem associada a um item
	public Text text; // Referência aos textos associados a um item
	public Weapon weapon; // Referência à classe de itens Armas
	public Key key; // Referência à classe de itens Chaves
	public Consumable consumable; // Referência à classe de itens Consumíveis

	// Método que gera um item do tipo Arma na Lista de Itens
	public void SetUpWeapon(Weapon menuWeapon) {
		weapon = menuWeapon;
		image.sprite = weapon.image;
		text.text = weapon.weaponName;
	}

	// Método que gera um item do tipo Chave na Lista de Itens
	public void SetUpKey(Key menuKey) {
		key = menuKey;
		image.sprite = key.image;
		text.text = key.keyName;
	}

	// Método que gera um item do tipo Consumível na Lista de Itens
	public void SetUpConsumable(Consumable menuConsumable) {
		consumable = menuConsumable;
		image.sprite = consumable.image;
		text.text = consumable.itemName;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ShootController : MonoBehaviour {
	public WeaponBase wea_InHand,wea_InPack;
	public Transform GunPoint;	
	private Pickable current_Pickable=null;
	private AudioSource c_audio;
	private AudioClip clip_medpack,clip_ammunation,clip_armor,clip_weapon;
	// Update is called once per frame
	void Start(){
		c_audio = GetComponent<AudioSource> ();
		clip_medpack = ResourceManager.Instance.GetAudio ("player/item_medpack");
		clip_ammunation = ResourceManager.Instance.GetAudio ("player/item_ammunation");
		clip_armor = ResourceManager.Instance.GetAudio ("player/item_armor");
		clip_weapon = ResourceManager.Instance.GetAudio ("player/item_weapon");
		WeaponBase[] weapons=GunPoint.GetComponentsInChildren<WeaponBase>();
		wea_InHand = weapons [0];
		if(weapons.Length>1)
		wea_InPack = weapons [1];
		if (wea_InPack != null)
			wea_InPack.gameObject.SetActive (false);	
	}
	void Update () {
		if (Input.GetMouseButton (0)) {
			Shoot ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			Reload ();
		}
		if (Input.GetMouseButtonDown (0)) {
			LeftClick ();
		}
		if (Input.GetMouseButtonDown (1)) {
			RightClick ();
		}
		if (Input.GetKeyDown (KeyCode.Tab)) {
			ChangeWeapon ();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			GetItem ();
		}
		if (Input.GetMouseButtonDown (2)) {
			MidClick ();
		}
	}
	public void Shoot(){
		wea_InHand.ContinousShoot();
	}
	public void Reload(){
		wea_InHand.Reload ();
	}
	public void LeftClick(){
		wea_InHand.EmptyClick ();
		wea_InHand.OneClickShoot ();
	}
	public void RightClick(){
		wea_InHand.WeaponSpecial ();
	}
	public void MidClick(){
		wea_InHand.LaserSwitch ();
	}
	void GetItem(){
		GetItem (current_Pickable);
		current_Pickable = null;
	}
	public void GetItem(Pickable _pickable){
		if (_pickable == null)
			return;
		AudioClip clip=null;
		bool b_pickWeapon = false;
		switch (_pickable.paType) {
		case PickableType.weapon:{
				b_pickWeapon = true;
				clip = clip_weapon;
				break;
			}
		case PickableType.ammunation:{
				wea_InHand.GetAmmo ();
				clip=clip_ammunation;
				break;}
		case PickableType.medpack:{
				GetComponent<PlayerHealthController> ().GainHP (50,0);
				clip=clip_medpack;
				break;
			}
		case PickableType.armor:{
				GetComponent<PlayerHealthController> ().GainHP (0,50);
				clip=clip_armor;
				break;
			}
		case PickableType.laser:{
				wea_InHand.GetAttachment ();
				clip = clip_weapon;
				break;
			}
		}
		if(b_pickWeapon)
			GetWeapon (_pickable);
		PlayAudio (clip);
		_pickable.PickUp ();
		_pickable = null;
		UpdateStatus ();
	}
	public void ChangeWeapon(){
		if (wea_InPack == null)
			return;

		WeaponBase temp = wea_InHand;
		wea_InHand = wea_InPack;
		wea_InPack = temp;

		wea_InHand.gameObject.SetActive (true);
		wea_InPack.gameObject.SetActive (false);
		wea_InHand.WeaponChangeTo ();
		wea_InPack.WeaponChangeBack ();
		UpdateStatus ();
	}
	void GetWeapon(Pickable _pickable){
		if (wea_InPack == null) {
			wea_InPack = _pickable.GetWeapon();
			wea_InPack.transform.SetParent (GunPoint);
			wea_InPack.transform.position = GunPoint.position;
			wea_InPack.transform.rotation = GunPoint.rotation;
			ChangeWeapon ();
		}else{
			wea_InHand.WeaponChangeBack ();
			Destroy (wea_InHand.gameObject);
			wea_InHand =_pickable.GetWeapon();
			wea_InHand.transform.position = GunPoint.position;
			wea_InHand.transform.rotation = GunPoint.rotation;
			wea_InHand.transform.SetParent (GunPoint);
			wea_InHand.WeaponChangeTo ();
			UpdateStatus ();
		}
		UIController.Instance.UpdateItem ("");
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "item") {
			UIController.Instance.UpdateItem (other.GetComponent<Pickable> ().GetItemName());
			current_Pickable= other.GetComponent<Pickable> ();
		}
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "item") {
			UIController.Instance.UpdateItem ("");
			current_Pickable = null;
		}
	}

	void UpdateStatus(){
		UIController.Instance.UpdateItem ("");
		UIController.Instance.UpdateAmmo (wea_InHand.InClipAmmo, wea_InHand.InPackAmmo);
	}
	void PlayAudio(AudioClip clip){
		c_audio.clip = clip;
		c_audio.Play ();
	}
}

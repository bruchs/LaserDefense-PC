using UnityEngine;
using System.Collections;

public class EnemyLaserDamage : MonoBehaviour {

	public float damage = 50f;
	
	public float GetDamage(){
		return damage;
	}
	
	public void HitHeroe(){
		Destroy(gameObject);
	}
}
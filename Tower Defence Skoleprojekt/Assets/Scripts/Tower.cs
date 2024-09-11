using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public List<GameObject> enemiesInRange = new List<GameObject>();            // En liste der skal indeholde fjender som t�rnet kan skyde p�
	public GameObject projectile;                                               // T�rnets projektil
	public float cooldown;                                                      // The time between each shot is fired
	float remainingCooldown;                                                    // Tiden f�r n�ste skud
	public Transform shootOrigin;                                               // Stedet projektilerne skal spawne
	public float projectileSpeed = 10f;                                         // Projektil hastighed

	void Start()
	{

	}

	void Update()
	{
		enemiesInRange.RemoveAll(enemy => enemy == null);                       // Ryd op i listen og fjern �delagte fjender (null-referencer)

		remainingCooldown = remainingCooldown - Time.deltaTime;                 // remaining cooldown f�r trukket lige s� meget fra sig som den tid sidste frame varede

		if (remainingCooldown <= 0)                                             // er tiden g�et?
		{
			if (enemiesInRange.Count > 0)                                       // er der fjender som t�rnet m� skyde p�?
			{
				Shoot();                                                        // skyd
			}
		}
	}

	private void OnTriggerEnter(Collider other)						// N�r noget r�r tower's trigger
	{
		if (other.gameObject.name == "Enemy(Clone)")                // Hedder objektet der r�r "Enemy(Clone)"?
		{
			enemiesInRange.Add(other.gameObject);                   // Tilf�j til listen enemiesInRange
		}
	}

	private void OnTriggerExit(Collider other)							// N�r noget forsvinder fra tower's trigger
	{
		if (other.gameObject.name == "Enemy(Clone)")					// Hedder objektet der r�r "Enemy(Clone)"?
		{
			enemiesInRange.Remove(other.gameObject);					// Fjern fra listen enemiesInRange
		}
	}

	void Shoot()
	{
		
		if (enemiesInRange.Count > 0)																		// S�rg for at der er en fjende at skyde p�
		{
			GameObject targetEnemy = enemiesInRange[0];														// Find den f�rste fjende i listen
			Vector3 direction = (targetEnemy.transform.position - shootOrigin.position).normalized;			// Beregn retningen mod fjenden

			GameObject newProjectile = Instantiate(projectile, shootOrigin.position, Quaternion.identity);  // Lav en ny projektil
			Rigidbody rb = newProjectile.GetComponent<Rigidbody>();											// F� fat i projektilens Rigidbody

			if (rb != null)
			{
				rb.velocity = direction * projectileSpeed;													// Skyd projektilen mod fjenden med den valgte hastighed
			}

			remainingCooldown = cooldown;                                                                   // Genstart cooldown

			StartCoroutine(DestroyProjectileAfterHalfASecond(newProjectile));								// Kald coroutine som �del�gger projektilet efter 500ms.
		}
	}

	IEnumerator DestroyProjectileAfterHalfASecond(GameObject projectileToDie)
	{
		yield return new WaitForSeconds(0.5f);					// Wait 500 ms.

		if (projectileToDie != null)							// Eksisterer projektilet stadigv�k?
		{
			Destroy(projectileToDie);							// �del�g projektilet
		}
	}
}

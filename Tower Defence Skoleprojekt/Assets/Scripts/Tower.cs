using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public List<GameObject> enemiesInRange = new List<GameObject>();            // En liste der skal indeholde fjender som tårnet kan skyde på
	public GameObject projectile;                                               // Tårnets projektil
	public float cooldown;                                                      // The time between each shot is fired
	float remainingCooldown;                                                    // Tiden før næste skud
	public Transform shootOrigin;                                               // Stedet projektilerne skal spawne
	public float projectileSpeed = 10f;                                         // Projektil hastighed

	void Start()
	{

	}

	void Update()
	{
		enemiesInRange.RemoveAll(enemy => enemy == null);                       // Ryd op i listen og fjern ødelagte fjender (null-referencer)

		remainingCooldown = remainingCooldown - Time.deltaTime;                 // remaining cooldown får trukket lige så meget fra sig som den tid sidste frame varede

		if (remainingCooldown <= 0)                                             // er tiden gået?
		{
			if (enemiesInRange.Count > 0)                                       // er der fjender som tårnet må skyde på?
			{
				Shoot();                                                        // skyd
			}
		}
	}

	private void OnTriggerEnter(Collider other)						// Når noget rør tower's trigger
	{
		if (other.gameObject.name == "Enemy(Clone)")                // Hedder objektet der rør "Enemy(Clone)"?
		{
			enemiesInRange.Add(other.gameObject);                   // Tilføj til listen enemiesInRange
		}
	}

	private void OnTriggerExit(Collider other)							// Når noget forsvinder fra tower's trigger
	{
		if (other.gameObject.name == "Enemy(Clone)")					// Hedder objektet der rør "Enemy(Clone)"?
		{
			enemiesInRange.Remove(other.gameObject);					// Fjern fra listen enemiesInRange
		}
	}

	void Shoot()
	{
		
		if (enemiesInRange.Count > 0)																		// Sørg for at der er en fjende at skyde på
		{
			GameObject targetEnemy = enemiesInRange[0];														// Find den første fjende i listen
			Vector3 direction = (targetEnemy.transform.position - shootOrigin.position).normalized;			// Beregn retningen mod fjenden

			GameObject newProjectile = Instantiate(projectile, shootOrigin.position, Quaternion.identity);  // Lav en ny projektil
			Rigidbody rb = newProjectile.GetComponent<Rigidbody>();											// Få fat i projektilens Rigidbody

			if (rb != null)
			{
				rb.velocity = direction * projectileSpeed;													// Skyd projektilen mod fjenden med den valgte hastighed
			}

			remainingCooldown = cooldown;                                                                   // Genstart cooldown

			StartCoroutine(DestroyProjectileAfterHalfASecond(newProjectile));								// Kald coroutine som ødelægger projektilet efter 500ms.
		}
	}

	IEnumerator DestroyProjectileAfterHalfASecond(GameObject projectileToDie)
	{
		yield return new WaitForSeconds(0.5f);					// Wait 500 ms.

		if (projectileToDie != null)							// Eksisterer projektilet stadigvæk?
		{
			Destroy(projectileToDie);							// Ødelæg projektilet
		}
	}
}

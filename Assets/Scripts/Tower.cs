using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class Tower : MonoBehaviour
// {
// 	public string projectilePath;
// 	public string shootOriginPath;
// 	public float projectileSpeed;
// 	public float cooldown;
// 	public int price;

// 	public List<GameObject> enemiesInRange = new List<GameObject>();
// 	public GameObject projectile;
// 	public Transform shootOrigin;
// 	protected float remainingCooldown;

// 	protected virtual void Start()
// 	{
// 		shootOrigin = GetShootOrigin(shootOriginPath);
// 		projectile = GetProjectile(projectilePath);
// 	}

// 	protected virtual void Update()
// 	{
// 		enemiesInRange.RemoveAll(enemy => enemy == null);
// 		remainingCooldown = remainingCooldown - Time.deltaTime;

// 		if (remainingCooldown <= 0)
// 		{
// 			if (enemiesInRange.Count > 0)
// 			{
// 				Shoot();
// 			}
// 		}
// 	}

// 	protected virtual void OnTriggerEnter(Collider other)
// 	{
// 		if (other.gameObject.name == "Enemy(Clone)")
// 		{
// 			enemiesInRange.Add(other.gameObject);
// 		}
// 	}

// 	protected virtual void OnTriggerExit(Collider other)
// 	{
// 		if (other.gameObject.name == "Enemy(Clone)")
// 		{
// 			enemiesInRange.Remove(other.gameObject);
// 		}
// 	}

// 	protected virtual void Shoot()
// 	{
// 		if (enemiesInRange.Count > 0)
// 		{
// 			GameObject targetEnemy = enemiesInRange[0];
// 			Vector3 direction = (targetEnemy.transform.position - shootOrigin.position).normalized;

// 			GameObject newProjectile = Instantiate(projectile, shootOrigin.position, Quaternion.identity);
// 			Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

// 			if (rb != null)
// 			{
// 				rb.velocity = direction * projectileSpeed;
// 			}

// 			remainingCooldown = cooldown;

// 			StartCoroutine(DestroyProjectileAfterHalfASecond(newProjectile));
// 		}
// 	}

// 	private Transform GetShootOrigin(string path)
// 	{
// 		GameObject parent = gameObject;
// 		return parent.transform.Find(path).transform;
// 	}

// 	private GameObject GetProjectile(string path)
// 	{
// 		return Resources.Load<GameObject>($"Prefabs/Projectiles/{path}");
// 	}

// 	protected IEnumerator DestroyProjectileAfterHalfASecond(GameObject projectileToDie)
// 	{
// 		yield return new WaitForSeconds(0.5f);

// 		if (projectileToDie != null)
// 		{
// 			Destroy(projectileToDie);
// 		}
// 	}
// }

public class Tower : MonoBehaviour
{
	public string shootOriginPath;
	public string particlePath;
	public float cooldown;
	public int price;
	public int damage;
	public GameObject beamEffectPrefab; // Reference to the particle system prefab

	public List<GameObject> enemiesInRange = new List<GameObject>();
	public Transform shootOrigin;
	protected float remainingCooldown;

	protected virtual void Start()
	{
		shootOrigin = GetShootOrigin(shootOriginPath);
		beamEffectPrefab = GetParticlePrefab(particlePath);
	}

	protected virtual void Update()
	{
		enemiesInRange.RemoveAll(enemy => enemy == null);
		remainingCooldown -= Time.deltaTime;

		if (remainingCooldown <= 0)
		{
			if (enemiesInRange.Count > 0)
			{
				Shoot();
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			enemiesInRange.Add(other.gameObject);
		}
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			enemiesInRange.Remove(other.gameObject);
		}
	}

	protected virtual void Shoot()
	{
		if (enemiesInRange.Count > 0)
		{
			GameObject targetEnemy = enemiesInRange[0];

			// Apply damage to the enemy
			Enemy enemyScript = targetEnemy.GetComponent<Enemy>();
			if (enemyScript != null)
			{
				enemyScript.TakeDamage(damage);
			}

			// Spawn visual effect
			SpawnBeamEffect(targetEnemy);

			remainingCooldown = cooldown;
		}
	}

	private Transform GetShootOrigin(string path)
	{
		return transform.Find(path).transform;
	}

	private GameObject GetParticlePrefab(string path)
	{
		return Resources.Load<GameObject>($"Prefabs/Projectiles/{path}");
	}

	protected void SpawnBeamEffect(GameObject targetEnemy)
	{
		// Instantiate the beam quad at the shoot origin
		GameObject beam = Instantiate(beamEffectPrefab, shootOrigin.position, Quaternion.identity);

		// Set the beam's parent to the tower (optional)
		beam.transform.SetParent(transform);

		// Calculate the direction and distance to the target enemy
		Vector3 startPoint = shootOrigin.position;
		Vector3 endPoint = targetEnemy.transform.position;
		Vector3 direction = (endPoint - startPoint).normalized;
		direction.x += 90;
		float distance = Vector3.Distance(startPoint, endPoint);

		// Position the beam at the midpoint between the tower and the enemy
		beam.transform.position = (startPoint + endPoint) / 2;

		// Rotate the beam to face the enemy
		beam.transform.rotation = Quaternion.LookRotation(direction);
		beam.transform.Rotate(Vector3.right, 0, 0);
		float y = beam.transform.rotation.y;
		float x = beam.transform.rotation.x;

		Debug.Log($"{beam.transform.rotation.normalized.x}");

		// Adjust the beam's scale to match the distance
		Vector3 beamScale = beam.transform.localScale;
		beamScale.x = 0.1f; // Thickness of the beam
		beamScale.y = distance; // Length of the beam
		beamScale.z = 1f; // Keep at 1 since the quad has no depth
		beam.transform.localScale = beamScale;

		// Destroy the beam after a short duration
		Destroy(beam, 0.1f);
	}

	// protected void SpawnBeamEffect(GameObject targetEnemy)
	// {
	// 	// Instantiate the beam quad at the shoot origin
	// 	GameObject beam = Instantiate(beamEffectPrefab, shootOrigin.position, Quaternion.identity);

	// 	// Set the beam's parent to the tower (optional)
	// 	beam.transform.SetParent(transform);

	// 	// Calculate the direction and distance to the target enemy
	// 	Vector3 startPoint = shootOrigin.position;
	// 	Vector3 endPoint = targetEnemy.transform.position;

	// 	// Since we're in a top-down game, we need to ignore the Y-axis
	// 	Vector3 direction = (new Vector3(endPoint.x, 0, endPoint.z) - new Vector3(startPoint.x, 0, startPoint.z)).normalized;
	// 	float distance = Vector3.Distance(new Vector3(startPoint.x, 0, startPoint.z), new Vector3(endPoint.x, 0, endPoint.z));

	// 	// Position the beam at the midpoint between the tower and the enemy
	// 	beam.transform.position = new Vector3((startPoint.x + endPoint.x) / 2, shootOrigin.position.y, (startPoint.z + endPoint.z) / 2);

	// 	// Rotate the beam to face the enemy
	// 	float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
	// 	beam.transform.rotation = Quaternion.Euler(90f, -angle, 0f); // Adjusted for top-down orientation

	// 	// Adjust the beam's scale to match the distance
	// 	Vector3 beamScale = beam.transform.localScale;
	// 	beamScale.x = distance; // Length of the beam in the XZ plane
	// 	beamScale.y = 1f;       // Thickness of the beam
	// 	beamScale.z = 1f;       // Thickness of the beam
	// 	beam.transform.localScale = beamScale;

	// 	// Destroy the beam after a short duration
	// 	Destroy(beam, 0.1f);
	// }

}

public class TowerBasic : Tower
{
	public TowerBasic()
	{
		// Set specific values for TowerBasic
		particlePath = "Beam Mesh";
		shootOriginPath = "Head";
		damage = 2;
		cooldown = 0.2f;
		price = 10;
	}

	// Removed `new` keyword; we will override the values in Start method
	protected override void Start()
	{
		// Call the base Start to ensure other initializations are done
		base.Start();
	}

	// No need to override Update if no changes are needed
	protected override void Update()
	{
		base.Update();
	}

	// No need to override OnTriggerEnter if no changes are needed
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}

	// No need to override OnTriggerExit if no changes are needed
	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
	}

	protected override void Shoot()
	{
		base.Shoot();
	}
}

public class TowerSniper : Tower
{
	public TowerSniper()
	{
		// Set specific values for TowerBasic
		particlePath = "Beam Mesh";
		shootOriginPath = "Head";
		damage = 10;
		cooldown = 0.8f;
		price = 20;
	}

	// Removed `new` keyword; we will override the values in Start method
	protected override void Start()
	{
		// Call the base Start to ensure other initializations are done
		base.Start();
	}

	// No need to override Update if no changes are needed
	protected override void Update()
	{
		base.Update();
	}

	// No need to override OnTriggerEnter if no changes are needed
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}

	// No need to override OnTriggerExit if no changes are needed
	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
	}

	protected override void Shoot()
	{
		base.Shoot();
	}
}
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		float distance = Vector3.Distance(startPoint, endPoint);

		// Position the beam at the midpoint between the tower and the enemy
		beam.transform.position = (startPoint + endPoint) / 2;

		// Rotate the beam to face the enemy (adjust for top-down view)
		// Look towards the enemy, aligning the Y axis (since top-down games often use Y as "up")
		beam.transform.rotation = Quaternion.FromToRotation(Vector3.down, direction);

		// Adjust the beam's scale to match the distance
		Vector3 beamScale = beam.transform.localScale;
		beamScale.x = 0.04f; // Thickness of the beam
		beamScale.y = distance; // Length of the beam
		beamScale.z = 1f; // Keep at 1 since the quad has no depth
		beam.transform.localScale = beamScale;

		// Destroy the beam after a short duration
		Destroy(beam, 0.05f);
	}
}

public class TowerBasic : Tower
{
	public TowerBasic()
	{
		particlePath = "Beam Mesh";
		shootOriginPath = "Head";
		damage = 5;
		cooldown = 0.75f;
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
		particlePath = "Beam Mesh";
		shootOriginPath = "Head";
		damage = 25;
		cooldown = 2f;
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
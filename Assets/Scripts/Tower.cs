using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public string projectilePath;
	public string shootOriginPath;
	public float projectileSpeed;
	public float cooldown;

	public List<GameObject> enemiesInRange = new List<GameObject>();
	public GameObject projectile;
	public Transform shootOrigin;
	protected float remainingCooldown;

	protected virtual void Start()
	{
		shootOrigin = GetShootOrigin(shootOriginPath);
		projectile = GetProjectile(projectilePath);
	}

	protected virtual void Update()
	{
		enemiesInRange.RemoveAll(enemy => enemy == null);
		remainingCooldown = remainingCooldown - Time.deltaTime;

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
		if (other.gameObject.name == "Enemy(Clone)")
		{
			enemiesInRange.Add(other.gameObject);
		}
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "Enemy(Clone)")
		{
			enemiesInRange.Remove(other.gameObject);
		}
	}

	protected virtual void Shoot()
	{
		if (enemiesInRange.Count > 0)
		{
			GameObject targetEnemy = enemiesInRange[0];
			Vector3 direction = (targetEnemy.transform.position - shootOrigin.position).normalized;

			GameObject newProjectile = Instantiate(projectile, shootOrigin.position, Quaternion.identity);
			Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

			if (rb != null)
			{
				rb.velocity = direction * projectileSpeed;
			}

			remainingCooldown = cooldown;

			StartCoroutine(DestroyProjectileAfterHalfASecond(newProjectile));
		}
	}

	private Transform GetShootOrigin(string path)
	{
		GameObject parent = gameObject;
		return parent.transform.Find(path).transform;
	}

	private GameObject GetProjectile(string path)
	{
		return Resources.Load<GameObject>($"Prefabs/Projectiles/{path}");
	}

	protected IEnumerator DestroyProjectileAfterHalfASecond(GameObject projectileToDie)
	{
		yield return new WaitForSeconds(0.5f);

		if (projectileToDie != null)
		{
			Destroy(projectileToDie);
		}
	}
}

public class TowerBasic : Tower
{
	// Removed `new` keyword; we will override the values in Start method
	protected override void Start()
	{
		// Set the desired values for the fields
		projectilePath = "Basic Projectile";
		shootOriginPath = "Head";
		cooldown = 0.2f;
		projectileSpeed = 25f;

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


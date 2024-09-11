using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public List<GameObject> enemiesInRange = new List<GameObject>();
	public GameObject projectile;
	public string projectilePath;
	public float cooldown;
	public Transform shootOrigin;
	public string shootOriginPath;
	public float projectileSpeed = 10f;
	protected float remainingCooldown;

	protected void Start()
	{
		shootOrigin = GetShootOrigin(shootOriginPath);
		projectile = GetProjectile(projectilePath);
	}

	protected void Update()
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

	protected void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "Enemy(Clone)")
		{
			enemiesInRange.Add(other.gameObject);
		}
	}

	protected void OnTriggerExit(Collider other)
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
	public new string projectilePath = "";
	public new string shootOriginPath = "";
	public new float cooldown = 0.2f;
	public new float projectileSpeed = 10f;

	new void Start()
	{
		base.Start();
	}

	new void Update()
	{
		base.Update();
	}

	new void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}

	new void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
	}

	protected override void Shoot()
	{
		base.Shoot();
	}
}

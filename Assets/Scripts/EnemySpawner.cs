using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
	public float cooldown;

	private void Start()
	{
		StartCoroutine("SpawnCoroutine");
	}

	public IEnumerator SpawnCoroutine()
	{
		while (true) 
		{ 
			Instantiate(enemy, this.transform.position, Quaternion.identity);
			yield return new WaitForSeconds(cooldown);
		}
	}
}

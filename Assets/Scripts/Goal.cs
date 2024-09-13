using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.name == "Enemy(Clone)")
        {
            GameController.Instance.LoseLife();
            Destroy(other.gameObject);
        }
	}
}

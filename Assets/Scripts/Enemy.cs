using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    Transform goal;
    public int maxHP;
    public int currentHP;
    public Image hpBar;
    public Image hpBarBackground;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        goal = GameObject.Find("Goal").transform;           // goal-variablen gives en v�rdi. Scriptet finder selv "Goal" i Unity-scenen n�r denne linje k�res.
        agent.SetDestination(goal.position);
    }

    public void TakeDamage(int damage)
    {
        currentHP = Mathf.RoundToInt(Math.Max(currentHP - damage, 0));
    }

    private void Update()
    {
        if (currentHP <= 0)                                 // Har fjenden 0 eller under 0 hp?
        {
            Destroy(gameObject);                            // Fjern fjenden fra spillet
            CurrencyHandler.Instance.GainMoney(1);
        }

        hpBar.fillAmount = (float)currentHP / maxHP;        // S�tter hp-barens v�rdi.

        hpBar.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);           // HP baren peger i retning af kameraet
        hpBarBackground.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up); // HP barens baggrundsbillede peger i retning af kameraet
    }

    private void OnCollisionEnter(Collision collision)                  // Fjenden rammer noget
    {
        if (collision.collider.tag == "Projectile")             // Hedder det fjenden rammer "Projectile(Clone)"?
        {
            currentHP = currentHP - 1;                                  // Der tr�kkes 1 fra currentHP;
            Destroy(collision.collider.gameObject);                     // Fjern projektilet fra spillet
        }
    }
}

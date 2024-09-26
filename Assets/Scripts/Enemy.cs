using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform goal;
    private Image hpBar;
    private Image hpBarBackground;
    private Camera mainCamera;

    public int maxHP;
    public float speed;
    public int moneyGivenOnDeath;

    [DoNotSerialize]
    public int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP;
        mainCamera = Camera.main;
        GetReferences();
        agent.speed = speed;
        agent.SetDestination(goal.position);
    }

    protected virtual void Update()
    {
        if (agent.hasPath)
        {
            agent.velocity = agent.desiredVelocity; // Force the agent to move at full speed
        }

        hpBar.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);           // HP baren peger i retning af kameraet
        hpBarBackground.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up); // HP barens baggrundsbillede peger i retning af kameraet
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP = Mathf.RoundToInt(Math.Max(currentHP - damage, 0));

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            CurrencyHandler.Instance.GainMoney(moneyGivenOnDeath);
        }
        hpBar.fillAmount = (float)currentHP / maxHP;
    }

    private void GetReferences()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        hpBar = transform.Find("Canvas/HPBar").GetComponent<Image>();
        hpBarBackground = transform.Find("Canvas/Image").GetComponent<Image>();
        goal = GameObject.Find("Goal").transform;
    }
}

public class EnemyBasic : Enemy
{
    public EnemyBasic()
    {
        maxHP = 10;
        speed = 4f;
        moneyGivenOnDeath = 1;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
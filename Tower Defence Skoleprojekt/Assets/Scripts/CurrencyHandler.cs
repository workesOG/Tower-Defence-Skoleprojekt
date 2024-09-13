using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyHandler : MonoBehaviour
{
    public static CurrencyHandler Instance { get; private set;}

    public int Money; // Antal penger
	System.Random random = new System.Random();

    void Awake()
    {
        Instance = this;
    }
  	public void GainMoney()
	{
		int randomMoney = random.Next(3,6);
		Money += randomMoney;
	}
}

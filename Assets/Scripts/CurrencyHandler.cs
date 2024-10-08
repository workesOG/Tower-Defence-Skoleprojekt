using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyHandler : MonoBehaviour
{
    public static CurrencyHandler Instance { get; private set; }

    [SerializeField]
    private int startingCapital;
    public TextMeshProUGUI moneyScreenText;
    System.Random random = new System.Random();

    private int money;
    public int Money
    {
        get => money;
        set
        {
            money = value;
            moneyScreenText.text = $"Money: {money}$";
        }
    }

    void Awake()
    {
        Instance = this;
        Money = startingCapital;
    }

    public void GainMoney(int amount)
    {
        //int randomMoney = random.Next(3, 6);
        Money += amount;
    }
}

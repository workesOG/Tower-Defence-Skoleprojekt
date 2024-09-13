using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
	public static GameController Instance;			// Laver �t objekt ud af GameController-klassen kaldet Instance
	
	[SerializeField]
	private int startingLives;
	
	private int lives;                               // Antal liv
	public int Lives{
		get => lives;
		set
		{
			lives = value;
			lifesScreenText.text = $"Lifes: {value}";   // S�tter v�rdien af teksten p� sk�rmen til at vise liv
		}
	}
	public TextMeshProUGUI lifesScreenText;			// Teksten p� sk�rmen

	private void Awake()							// Awake er ligesom Start-metoden, den k�rer bare tidligere
	{
		Instance = this;							// Instance = denne klasse (GameController)
		Lives = startingLives;
	}

	public void LoseLife()
	{
		Lives -= 1;                          // Man kan ogs� skrive "lifes--", det giver det samme

		if (Lives < 1)
		{
			print("Game Lost!");
			Time.timeScale = 0;						// S�tter tiden i st�. 1 = normaltid.
		}
	}


}

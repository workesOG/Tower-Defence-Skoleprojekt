using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
	public static GameController Instance;			// Laver ét objekt ud af GameController-klassen kaldet Instance
	public int lifes;                               // Antal liv
	public TextMeshProUGUI lifesScreenText;			// Teksten på skærmen

	private void Awake()							// Awake er ligesom Start-metoden, den kører bare tidligere
	{
		Instance = this;							// Instance = denne klasse (GameController)
	}

	public void LoseLife()
	{
		lifes = lifes - 1;                          // Man kan også skrive "lifes--", det giver det samme
		lifesScreenText.text = $"Lifes: {lifes}";   // Sætter værdien af teksten på skærmen til at vise liv

		if ( lifes < 1)
		{
			print("Game Lost!");
			Time.timeScale = 0;						// Sætter tiden i stå. 1 = normaltid.
		}
	}


}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MoleMaster : MonoBehaviour {

	public int minActivate;
	public int maxActivate;
	public float roundPauseTime;
	public Text letterLabel;

	Mole[] moles;
	float roundPause;
	char currentLetter;
	bool roundActive = false;

	const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	void Start() {
		roundPause = roundPauseTime;
		moles = FindObjectsOfType<Mole>();
		Debug.Log("Found " + moles.Length + " moles.");
		for ( int i = 0; i < moles.Length; i++ ) {
			moles[i].gameObject.SetActive(false);
		}
	}

	void Update() {
		CheckForRoundActivation();
		if ( roundActive ) {
			CheckForMoleHit();
		}
	}

	void CheckForRoundActivation() {
		if ( roundPause <= 0.0f && ! MolesActive() ) {
			roundPause = roundPauseTime;
		}
		if ( roundPause > 0.0f ) {
			roundPause -= Time.deltaTime;
			if ( roundPause <= 0.0f ) {
				ActivateMoles();
			}
		}
	}

	void CheckForMoleHit() {
		if ( Input.GetMouseButtonDown(0) ) {
			Mole mole = MoleUnderMouse();
			if ( mole != null ) {
				Debug.Log("Hit!");
				roundActive = false;
				if ( mole.Letter == currentLetter ) {
					mole.Hit();
				} else {
					mole.Miss();
				}
			}
		}
	}

	Mole MoleUnderMouse() {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		return (hit.collider != null) ? hit.collider.GetComponent<Mole>() : null;
	}

	void ActivateMoles() {
		Debug.Log("Go moles!");
		currentLetter = RandomLetter(' ');
		letterLabel.text = currentLetter.ToString();
		List<Mole> hat = new List<Mole>(moles);
		bool first = true;
		for ( int count = Random.Range(minActivate, maxActivate+1);
				count > 0; count-- ) {
			Mole m = hat[Random.Range(0, hat.Count)];
			hat.Remove(m);
			if ( first ) m.SetLetter(currentLetter);
			else m.SetLetter(RandomLetter(currentLetter));
			first = false;
			m.Activate();
		}
		roundActive = true;
	}

	char RandomLetter(char exclude) {
		char c;
		do {
			c = letters[Random.Range(0, letters.Length)];
		} while ( c == exclude );
		return c;
	}

	bool MolesActive() {
		for ( int i = 0; i < moles.Length; i++ ) {
			if ( moles[i].gameObject.activeSelf ) return true;
		}
		return false;
	}

}

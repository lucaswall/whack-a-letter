using UnityEngine;
using System.Collections;

public class Mole : MonoBehaviour {

	public float moveDistance;
	public float moveTime;
	public float waitTime;
	public TextMesh letterLabel;
	public float shakeAmplitude;
	public float scaleTo;

	public char Letter { get { return letterLabel.text[0]; } }

	float startY;
	float wait = 0.0f;
	float dir = 0.0f;

	void Start() {
		startY = transform.position.y;
	}

	public void Activate() {
		if ( gameObject.activeSelf ) {
			Debug.LogWarning("Mole double activation!");
		}
		gameObject.SetActive(true);
		dir = 1.0f;
		wait = 0.0f;
	}

	public void SetLetter(char letter) {
		letterLabel.text = letter.ToString();
	}

	public void Hit() {
		MoveDown();
		StartCoroutine(HitEffect());
	}

	public void Miss() {
		MoveDown();
		StartCoroutine(MissEffect());
	}

	void Update() {
		if ( wait > 0.0f ) {
			wait -= Time.deltaTime;
		} else {
			transform.Translate(Vector3.up * dir * (moveDistance/moveTime) * Time.deltaTime);
			if ( dir > 0.0f && transform.position.y >= startY + moveDistance ) {
				MoveDown();
			} else if ( dir < 0.0f && transform.position.y <= startY ) {
				gameObject.SetActive(false);
			}
		}
	}

	void MoveDown() {
		wait = waitTime;
		dir = -1.0f;
	}

	IEnumerator HitEffect() {
		float t = 0.0f;
		while ( t <= 1.0f ) {
			t += Time.deltaTime / waitTime;
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-shakeAmplitude, shakeAmplitude));
			yield return null;
		}
		transform.rotation = Quaternion.identity;
	}

	IEnumerator MissEffect() {
		float t = 0.0f;
		Vector3 scaleStart = transform.localScale;
		Vector3 scaleEnd = scaleStart * scaleTo;
		while ( t <= 1.0f ) {
			t += Time.deltaTime / (waitTime/2.0f);
			transform.localScale = Vector3.Lerp(scaleStart, scaleEnd, t);
			yield return null;
		}
		t = 0.0f;
		while ( t <= 1.0f ) {
			t += Time.deltaTime / (waitTime/2.0f);
			transform.localScale = Vector3.Lerp(scaleEnd, scaleStart, t);
			yield return null;
		}
	}

}

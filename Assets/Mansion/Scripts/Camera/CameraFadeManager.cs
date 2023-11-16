using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFadeManager : MonoBehaviour
{
	public static CameraFadeManager Instance;

	public bool ShouldBeBlack = false;
	public Image BlackImage;
	public float FadeDuration = 0.5f;

	public bool pushBlack = false;
	public bool pushClear = false;
	int previousCullingMask;

	private void Start()
	{
		if (Instance == null) Instance = this;
	
		previousCullingMask = Camera.main.cullingMask;
		BlackImage.material.color = Color.black;
		Camera.main.cullingMask = (1 << LayerMask.NameToLayer("ScreenFade"));
		FadeToClear();
	}

	public void SetTunnelObject(bool enabled)
	{
		transform.GetChild(2).gameObject.SetActive(enabled);
	}

	public void FadeToBlack(Action callback = null)
	{
		StopAllCoroutines();
		ShouldBeBlack = true;

		StartCoroutine(ToDark(callback));
	}

	public void FadeToClear(Action callback = null)
	{
		StopAllCoroutines();
		ShouldBeBlack = false;
		Camera.main.cullingMask = previousCullingMask;

		StartCoroutine(ToClear(callback));
	}


	IEnumerator ToClear(Action callback)
	{
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		float currentTime = 0;
		do
		{
			float LerpValue = currentTime / FadeDuration;
			currentTime += Time.deltaTime;
			BlackImage.material.color = new Color(0, 0, 0, Mathf.Lerp(BlackImage.material.color.a, 0f, LerpValue));
			yield return new WaitForEndOfFrame();
		} while (currentTime < FadeDuration);

		if (callback != null)
			callback.Invoke();
	}

	IEnumerator ToDark(Action callback)
	{
		float currentTime = 0;
		do
		{
			float LerpValue = currentTime / FadeDuration;
			currentTime += Time.deltaTime;
			BlackImage.material.color = new Color(0, 0, 0, Mathf.Lerp(BlackImage.material.color.a, 1f, LerpValue));
			yield return new WaitForEndOfFrame();
		} while (currentTime < FadeDuration);

		Camera.main.cullingMask = (1 << LayerMask.NameToLayer("LoadingObjects")) | (1 << LayerMask.NameToLayer("ScreenFade"));

		if (callback != null)
			callback.Invoke();
		Camera.main.clearFlags = CameraClearFlags.Color;
	}


	void Update()
	{
		if (pushBlack)
		{
			FadeToBlack(null);
			pushBlack = false;
		}

		if (pushClear)
		{
			FadeToClear(null);
			pushClear = false;
		}
	}
}

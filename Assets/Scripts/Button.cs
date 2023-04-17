using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public event Action<Button> buttonPressed;

    [SerializeField]
    Color buttonColor;

    Material material;
    public AudioSource audio;

    public float buttonSpeed = 0.1f;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().material;
        material.color = buttonColor;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Coroutine coroutine;
    internal void Activate()
    {
        if (isAnimating) return;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(ChangeObjColor(GetComponent<Renderer>().material));

        if (audio.isPlaying) audio.Stop();
        audio.Play();
        Invoke("AudioStop", 0.25f);

        if (buttonPressed != null) buttonPressed(this);
    }

    private void AudioStop()
    {
        audio.Stop();
    }

    bool isAnimating = false;
    private IEnumerator ChangeObjColor(Material material)
    {
        isAnimating = true;
        LeanTween.cancel(gameObject);
        // Tween our color change
        LeanTween.moveLocalZ(gameObject, 0.2f, buttonSpeed);

        yield return new WaitForSeconds(buttonSpeed);
        // Tween our color change
        LeanTween.moveLocalZ(gameObject, 0, buttonSpeed);

        isAnimating = false;
    }
}

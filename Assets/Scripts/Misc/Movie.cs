﻿using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {

	public MovieTexture OurMovie;
	public Transform CameraCloseTransform;

	Vector3 _cameraStartRotation;
	Vector3 _cameraStartPos;

	void Start() {

		renderer.material.mainTexture = OurMovie;

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && other.GetComponent<PhotonView>().isMine)// && Camera.main.GetComponent<CameraTrackObj>().enabled == true)
		{
			Debug.Log("starting");

			MoveCameraToScreen();
		}

	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" )// && LeanTween.isTweening(gameObject) == false && Camera.main.GetComponent<CameraTrackObj>().enabled == false)
		{
			LeanTween.cancel(gameObject);
			Debug.Log("stopping");
			MoveCameraAwayFromScreen();
		}
	}

	void MoveCameraToScreen()
	{
		LeanTween.cancel(gameObject);

		_cameraStartRotation = Camera.main.transform.rotation.eulerAngles;
		_cameraStartPos = Camera.main.transform.position;


		GameObject.FindWithTag("Player").GetComponent<ThirdPersonCamera>().Deactivate(); 
//		Camera.main.GetComponent<CameraTrackObj>().enabled = false;
//		Camera.main.GetComponent<SU_CameraFollow>().enabled = false;

		LeanTween.move(Camera.main.gameObject,CameraCloseTransform.position,2).setEase(LeanTweenType.easeInOutCubic).setOnComplete ( ()=> {
            OurMovie.Play();
            audio.Play();                                                                                                                          
		});
		LeanTween.rotate(Camera.main.gameObject,CameraCloseTransform.rotation.eulerAngles,2).setEase(LeanTweenType.easeInOutCubic);

	}

	void MoveCameraAwayFromScreen()
	{
		LeanTween.cancel(gameObject);
		
		LeanTween.move(Camera.main.gameObject,_cameraStartPos,2).setEase(LeanTweenType.easeInOutCubic);
		LeanTween.rotate(Camera.main.gameObject,_cameraStartRotation,2).setEase(LeanTweenType.easeInOutCubic).setOnComplete( () =>
		                                                                                                                    {
			GameObject.FindWithTag("Player").GetComponent<ThirdPersonCamera>().Activate();
//			Camera.main.GetComponent<CameraTrackObj>().enabled = true;
//			Camera.main.GetComponent<SU_CameraFollow>().enabled = true;
			OurMovie.Pause();
			audio.Pause();
		});
		
	}


//	void Update() {
//		MovieTexture m = renderer.material.mainTexture as MovieTexture;
//		if (!m.isPlaying && m.isReadyToPlay)
//		{
//			Debug.Log("playing");
//			m.Play();
//		}
//
//		
//	}
}
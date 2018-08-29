using UnityEngine;
using System;
using System.Collections;

public class CamLookAt : MonoBehaviour {
    [SerializeField]
    private GameObject cameraRig;
    [SerializeField]
    private GameObject mainCameraView;
    public GameObject currentView;
    BackButtonController[] backButton;
    private InkScript[] inks;

    void Awake()
    {
        Inspectable[] inspectables = FindObjectsOfType<Inspectable>();
        if (inspectables != null)

            for (int i = 0; i < inspectables.Length; i++) {
                inspectables[i].OnPlayerStartMovingObserveAngle += HandlePlayerStartMovingObserveAngle;
                inspectables[i].OnPlayerReachObserveAngle += HandlePlayerReachObserveAngle;
            }

        Ground[] grounds = FindObjectsOfType<Ground>();
        if (grounds != null)
            for (int i = 0; i < grounds.Length; i++)
            {
                grounds[i].OnPlayerClickOnGroundObserveAngle += HandlePlayerClickOnGroundObserveAngle;
            }

        backButton = FindObjectsOfType<BackButtonController>();
        for (int i = 0; i < backButton.Length; i++)
            backButton[i].OnCamBackButtonClick += HandleBackButtonClick;

        RotateFirstPiecePuzzleCam[] rotatePuzzleCams = FindObjectsOfType<RotateFirstPiecePuzzleCam>();
        if (rotatePuzzleCams != null) {
            for (int i=0; i<rotatePuzzleCams.Length; i++) {
                rotatePuzzleCams[i].OnCutsceneTriggered += HandleCutSceneCam;
            }
        }

        inks = FindObjectsOfType<InkScript>();
        if (inks != null) {
            for (int i=0; i<inks.Length; i++) {
                inks[i].GameEventFired += OnGameEventFired;
            }
        }
    }

    void HandlePlayerStartMovingObserveAngle(ObserveAngle observable)
    {
        StopAllCoroutines();
        cameraRig.transform.position = observable.playerMovingCameraTransform.position;
        cameraRig.transform.rotation = observable.playerMovingCameraTransform.rotation;
        currentView = observable.playerMovingCameraTransform.gameObject;

    }

    public event Action OnCameraChangeTriggerBackButton = delegate { };

    void HandlePlayerReachObserveAngle(ObserveAngle observable) {
        StopAllCoroutines();

        if (observable.playerReachCameraTransform.GetComponent<CamTriggerBackButtonTag>() != null) {
            OnCameraChangeTriggerBackButton();
        }
        cameraRig.transform.position = observable.playerReachCameraTransform.position;
        cameraRig.transform.rotation = observable.playerReachCameraTransform.rotation;
        currentView = observable.playerReachCameraTransform.gameObject;

        FloatObject floatObject = observable.GetComponent<FloatObject>();
        if (floatObject != null)
        {
            StartCoroutine(FollowCamCo(observable.playerReachCameraTransform));
        }
        
    }

    IEnumerator FollowCamCo(Transform reachCam) {
        while (true) {
            cameraRig.transform.position = reachCam.position;
            cameraRig.transform.rotation = reachCam.rotation;
            currentView = reachCam.gameObject;
            yield return null;
        }
        
    }

    void HandlePlayerClickOnGroundObserveAngle(ObserveAngle observable) {
        //HandlePlayerStartMovingObserveAngle(observable);
        StopAllCoroutines();

        cameraRig.transform.position = observable.playerMovingCameraTransform.position;
        cameraRig.transform.rotation = observable.playerMovingCameraTransform.rotation;
        currentView = observable.playerMovingCameraTransform.gameObject;
    }

    void HandleBackButtonClick() {
        StopAllCoroutines();

        cameraRig.transform.position = mainCameraView.transform.position;
        cameraRig.transform.rotation = mainCameraView.transform.rotation;
        currentView = mainCameraView;
    }


    Vector3 tempCamPos;
    Quaternion tempCamRot;
    GameObject tempCurrentView;
    bool cutsceneTimerEnd;

    //wait till dialouge ends to pan camera back but if not more than cutsceneMinDuration, don't pan back
    void OnGameEventFired(string gameEvent) {
        if (gameEvent == "cutsceneEnd") {
            StartCoroutine(EndCutSceneAfterMinDuration());
        }
    }

    IEnumerator EndCutSceneAfterMinDuration()
    {
        while (cutsceneTimerEnd != true)
        {
            yield return null;
        }

        //cutscenetimer ends here
        cameraRig.transform.position = tempCamPos;
        cameraRig.transform.rotation = tempCamRot;
        currentView = tempCurrentView;

        cutsceneTimerEnd = false;

        yield break;
    }

    void HandleCutSceneCam(GameObject observeCameraAngle, float cutsceneMinDuration) {
        StopAllCoroutines();

        tempCamPos = cameraRig.transform.position;
        tempCamRot = cameraRig.transform.rotation;
        tempCurrentView = currentView;

        //StartCoroutine(CutSceneCamCo(observeCameraAngle, cutsceneDuration));
        cameraRig.transform.position = observeCameraAngle.transform.position;
        cameraRig.transform.rotation = observeCameraAngle.transform.rotation;
        currentView = observeCameraAngle.gameObject;


        StartCoroutine(StartCutsceneTimer(cutsceneMinDuration));
    }

    IEnumerator StartCutsceneTimer(float cutsceneMinDuration) {
        yield return new WaitForSeconds(cutsceneMinDuration);
        cutsceneTimerEnd = true;
        yield break;
    }

    void OnDisable()
    {
        Inspectable[] inspectables = FindObjectsOfType<Inspectable>();
        if (inspectables != null)

            for (int i = 0; i < inspectables.Length; i++)
            {
                inspectables[i].OnPlayerStartMovingObserveAngle -= HandlePlayerStartMovingObserveAngle;
                inspectables[i].OnPlayerReachObserveAngle -= HandlePlayerReachObserveAngle;
            }

        Ground[] grounds = FindObjectsOfType<Ground>();
        if (grounds != null)

        for (int i = 0; i < grounds.Length; i++)
        {
            grounds[i].OnPlayerClickOnGroundObserveAngle -= HandlePlayerClickOnGroundObserveAngle;
        }

        for (int i = 0; i < backButton.Length; i++)
            backButton[i].OnCamBackButtonClick -= HandleBackButtonClick;


        RotateFirstPiecePuzzleCam[] rotatePuzzleCams = FindObjectsOfType<RotateFirstPiecePuzzleCam>();
        if (rotatePuzzleCams != null)
        {
            for (int i = 0; i < rotatePuzzleCams.Length; i++)
            {
                rotatePuzzleCams[i].OnCutsceneTriggered -= HandleCutSceneCam;
            }
        }

        if (inks != null)
        {
            for (int i = 0; i < inks.Length; i++)
            {
                inks[i].GameEventFired -= OnGameEventFired;
            }
        }
    }
}

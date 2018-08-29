using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickToMove : MonoBehaviour{

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private ThirdPersonCharacter character;

    private GameObject targetObject;
    //private IEnumerator OnClickCo;
    [HideInInspector] public bool reset;
    //[HideInInspector] public bool coroutineRunning = false;
    private bool UIOpened = false;
    private bool lockClickToMove = false;
    private InkScript[] inks;


    private void Awake()
    {
        //if UIs are opened, signal this class to stop mouse click
        ClueImageController clueImageController = FindObjectOfType<ClueImageController>();
        if (clueImageController != null) {
            clueImageController.OnOpenImageUI += HandleUIOpen;
            clueImageController.OnCloseImageUI += HandleUIClose;
        }

        RotateFirstPiecePuzzleCam[] rotatePuzzleCams = FindObjectsOfType<RotateFirstPiecePuzzleCam>();
        if (rotatePuzzleCams != null)
        {
            for (int i = 0; i < rotatePuzzleCams.Length; i++)
            {
                rotatePuzzleCams[i].OnCutsceneTriggered += HandleCutSceneCam;
            }
        }

        inks = FindObjectsOfType<InkScript>();
        if (inks != null)
        {
            for (int i = 0; i < inks.Length; i++)
                inks[i].GameEventFired += OnGameEventFired;
        }
    }

    private void Start()
    {
        agent.updateRotation = false;
    }

    //public event Action OnClickToActivate = delegate { };

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (eventSystem != null && !eventSystem.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hitInfo) && UIOpened == false && lockClickToMove == false)
            {


                ICanClick clickable = hitInfo.collider.GetComponent<ICanClick>();
                //if click on an object with the ICanClick component, do whatever implementations it has
                if (clickable != null)
                {

                    Debug.Log(hitInfo.collider.gameObject.name);


                    //while a coroutine is running, don't allow any click
                    //if (!coroutineRunning)
                    //{
                        //set move destination, dont change camera
                        clickable.OnClickMovementSetup(hitInfo, agent);

                        //a different object is click than last time, change camera
                        if (targetObject != hitInfo.collider.gameObject ||
                            targetObject.GetComponent<InspectableNoCam>() != null)
                        {
                            clickable.OnClickCameraSetup(agent);

                            if (targetObject != null)
                            {
                                //IsParentInteraction parentInteractable = targetObject.GetComponent<IsParentInteraction>();
                                //if (parentInteractable != null)
                                //{
                                //    parentInteractable.SetColliderState(false);
                                //}
                            }

                        }

                        targetObject = hitInfo.collider.gameObject;
                    //}

                }


                /////////////////////
                //Implementation of interacting with close object with animation
                /////////////////////

                ICanClickToActivate[] clickToActivatables = hitInfo.collider.GetComponents<ICanClickToActivate>();
                for (int i = 0; i < clickToActivatables.Length; i++)
                {
                    clickToActivatables[i].HandleClickToActivate();
                }
            }
        }
        
        

        //if there is a target object after clicking, execute the implementation of ICanClick in it
        if (targetObject != null) {
            targetObject.GetComponent<ICanClick>().OnClickMovementAction(agent, character);
        }
        
    }

    private void HandleUIOpen() {
        UIOpened = true;
    }

    private void HandleUIClose() {
        UIOpened = false;
    }

    bool cutsceneTimerEnd;

    //wait till dialouge ends to pan camera back but if not more than cutsceneMinDuration, don't pan back
    void OnGameEventFired(string gameEvent)
    {
        if (gameEvent == "cutsceneEnd")
        {
            StartCoroutine(EndCutSceneAfterMinDuration());
        }
    }

    IEnumerator EndCutSceneAfterMinDuration() {
        while (cutsceneTimerEnd != true) {
            yield return null;
        }

        //cutscenetimer ends here
        lockClickToMove = false;

        cutsceneTimerEnd = false;
        
        yield break;
    }

    void HandleCutSceneCam(GameObject observeCameraAngle, float cutsceneMinDuration)
    {
        StopAllCoroutines();

        lockClickToMove = true;


        StartCoroutine(StartCutsceneTimer(cutsceneMinDuration));
    }

    IEnumerator StartCutsceneTimer(float cutsceneMinDuration)
    {
        yield return new WaitForSeconds(cutsceneMinDuration);
        cutsceneTimerEnd = true;
        yield break;
    }

    void OnDestroy()
    {
        if (inks != null)
        {
            for (int i = 0; i < inks.Length; i++)
                inks[i].GameEventFired -= OnGameEventFired;
        }
    }

}

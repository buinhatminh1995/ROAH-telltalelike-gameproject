using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

    [SerializeField] private GameObject caseFiles;
    private InkScript ink;

    private void Awake()
    {
        ink = FindObjectOfType<InkScript>();
        ink.GameEventFired += OnGameEventFired;
    }

    private void OnGameEventFired(string gameEvent)
    {
        if (gameEvent == "readAirplaneFiles")
        {
            caseFiles.GetComponent<ClickOpenClueImageBlurryBackgroundUI>().FireOnClickObjectWithClue();
            caseFiles.GetComponent<ObjectClickTriggerBackButton>().FireOnObjectClickTriggerBackButton();

            FindObjectOfType<BackButtonController>().OnClueBackButtonClick += ResumeDialogue;
        }

        if (gameEvent == "sleepOnPlane")
        {
            // play plane noises
            ink.Delayed(12f, () =>
            {
                ResumeDialogue();
            });
        }
    }

    private void ResumeDialogue()
    {
        FindObjectOfType<BackButtonController>().OnClueBackButtonClick -= ResumeDialogue;
        ink.StartStory("sc2.story_resume", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour {
    
    public string startKnot;
    [SerializeField] private Puzzle puzzleRequirement;
    public bool activated = true;
    [SerializeField] private bool onceOnly, allowMovement, triggerAnimation;

    private void Awake()
    {
        var inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle += discardparam => StartInk();

        var inspectableNoCam = GetComponent<InspectableNoCam>();
        if (inspectableNoCam != null)
            inspectableNoCam.OnPlayerReachAndClickOnObject += StartInk;

        var clickOpenClue = GetComponent<ClickOpenClueImageBlurryBackgroundUI>();
        if (clickOpenClue != null)
            clickOpenClue.OnClickObjectWithClue += discardparam => StartInk();

        var inventoryClick = GetComponent<InventoryItem>();
        if (inventoryClick != null && clickOpenClue == null)
            inventoryClick.OnInventoryClick += StartInk;

        var rotatePuzzle = GetComponent<RotatePuzzleParent>();
        if (rotatePuzzle != null)
            rotatePuzzle.OnFirstPieceRotated += StartInk;

        // listen to puzzle solved event to activate dialogue trigger
        if (puzzleRequirement)
        {
            activated = false;
            puzzleRequirement.PuzzleSolved += RequirementsDone;
        }

        if (GetComponent<RotatePuzzleParent>() != null)
            activated = true;
    }

    // allow trigger when reqs done
    private void RequirementsDone(object sender, System.EventArgs e)
    {
        activated = true;
        var animator = GetComponent<Animator>();
        if (animator && triggerAnimation)
            animator.enabled = true;

        var sound = GetComponent<AudioSource>();
        if (sound && triggerAnimation)
            sound.PlayDelayed(10f);

        if (GetComponent<RotatePuzzleParent>() != null) {
            StartInk();
        }

    }
    
    public void StartInk() {
        if (activated) // some reqs fulfilled e.g. puzzle solved
        {
            print("trigger ink story at " + startKnot);

            var ink = FindObjectOfType<InkScript>();
            ink.Delayed(0.5f, () => {
                ink.StartStory(startKnot, allowMovement);
            });
            GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().Move(Vector3.zero, false, false);

            if (onceOnly)
                activated = false;
        }
        else if (puzzleRequirement)
        {
            Debug.LogWarning("Puzzle not solved yet, dialogue can't be triggered");
        }
    }
}
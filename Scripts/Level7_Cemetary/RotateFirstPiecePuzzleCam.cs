using System.Collections;
using UnityEngine;
using System;

public class RotateFirstPiecePuzzleCam : MonoBehaviour {

    //listen to OnfirstPuzzleRotated on the RotatePuzzleParent script
    //when triggered, start a coroutine for x seconds,
    //      change lockcam in click to move to true
    //      disable camlookat?, actually not need to cuz camlookat only works when clicktomove loop is active

    [SerializeField] private GameObject observeCameraAngle;
    [SerializeField] private float cutsceneMinDuration;

   
    private void Awake()
    {
        RotatePuzzleParent puzzleParent = GetComponent<RotatePuzzleParent>();
        if (puzzleParent != null) {
            puzzleParent.OnFirstPieceRotated += HandleFirstPuzzleRotated;
        }
        
    }

    public event Action<GameObject,float> OnCutsceneTriggered = delegate { };

    void HandleFirstPuzzleRotated() {
        OnCutsceneTriggered(observeCameraAngle,cutsceneMinDuration);
    }

    

}

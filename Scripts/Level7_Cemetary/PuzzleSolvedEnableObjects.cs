using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolvedEnableObjects : Puzzle {
    //this can be the PuzzleParentOfAnyPuzzle or RotatePuzzleParent
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private GameObject[] objectsToEnable;

    //if all puzzle solved, enable the object and also fire OnPuzzleSolved to let other classes that wanna listen to this to trigger sth else

    private void Awake()
    {
        if (puzzle != null)
            puzzle.PuzzleSolved += EnableObjects;

        // set objects inactive at runtime to attach handlers first
        if (objectsToEnable != null)
        {
            for (int i = 0; i < objectsToEnable.Length; i++)
            {
                objectsToEnable[i].SetActive(false);
            }
        }
    }

    void EnableObjects(object sender, System.EventArgs e) {
        if (objectsToEnable != null) {
            for (int i = 0; i < objectsToEnable.Length; i++) {
                objectsToEnable[i].SetActive(true);
            }
        }

        solved = true;
        OnPuzzleSolved(System.EventArgs.Empty);
        
    }
}

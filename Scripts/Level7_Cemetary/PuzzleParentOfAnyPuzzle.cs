using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzleParentOfAnyPuzzle : Puzzle {

    [SerializeField] Puzzle[] puzzles;
    //private bool solved;

    private void Awake()
    {
        if (puzzles != null) {
            for (int i=0; i<puzzles.Length; i++)
            {
                puzzles[i].PuzzleSolved += HandleChildrenPuzzleSolved;
            }
        }
    }

    //public event Action OnAllPuzzleSolved = delegate { };

    void HandleChildrenPuzzleSolved(object sender, System.EventArgs e) {
        solved = true;
        for (int i = 0; i < puzzles.Length; i++)
        {
            solved = solved && puzzles[i].solved;
            Debug.Log(((Puzzle)sender).solved);
        }

        if (solved)
        {
            OnPuzzleSolved(System.EventArgs.Empty);
            GetComponent<TriggerDialogue>().StartInk();
            print("all puzzles solved ahahah");
        }

    }
}

using UnityEngine;
using System;

public class RotatePuzzleParent : Puzzle {

    [SerializeField] private RotatePuzzlePiece[] puzzlePieces;
    //private bool solved;

    private void Awake()
    {
        for (int i=0; i < puzzlePieces.Length; i++) {
            puzzlePieces[i].OnRotate += HandlePuzzlePieceRotate;
        }
        solved = false;
    }

    private bool pieceRotated, firstPieceRotated;
    public event Action OnFirstPieceRotated = delegate { };

    void HandlePuzzlePieceRotate() {
        pieceRotated = true;
        solved = true;

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            solved = solved && puzzlePieces[i].CheckCorrectAngle();
               
        }

        if (!firstPieceRotated && pieceRotated)
        {
            //fire event for first piece rotated
            OnFirstPieceRotated();
            firstPieceRotated = true;
            print("RotatePuzzle first piece rotated, maybe switch camera here");
        }
            

        if (solved)
        {
            OnPuzzleSolved(System.EventArgs.Empty);
            print("RotatePuzzle solved");
        }

        
    }
}

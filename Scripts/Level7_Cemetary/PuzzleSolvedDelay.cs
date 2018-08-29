using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolvedDelay : Puzzle {
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private float delay;

    private void Awake()
    {
        if (puzzle != null)
        {
            puzzle.PuzzleSolved += DelayOnSolved;
        }
    }

    void DelayOnSolved(object sender, System.EventArgs e)
    {
        StartCoroutine(DelayCo(delay));
    }

    IEnumerator DelayCo(float time)
    {
        yield return new WaitForSeconds(time);
        solved = true;
        OnPuzzleSolved(System.EventArgs.Empty);    
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour {
    [HideInInspector]public bool solved;

    public delegate void PuzzleSolvedEventHandler(object sender, System.EventArgs e);
    public event PuzzleSolvedEventHandler PuzzleSolved = delegate { };

    protected void Delayed(float delay, System.Action action)
    {
        StartCoroutine(DelayedCo(delay, action));
    }

    protected IEnumerator DelayedCo(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action();
    }

    protected virtual void OnPuzzleSolved (System.EventArgs e)
    {
        PuzzleSolved(this, e);
    }
}

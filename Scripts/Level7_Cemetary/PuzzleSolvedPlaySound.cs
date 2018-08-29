using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolvedPlaySound : MonoBehaviour {
    /// <summary>
    /// play sound when puzzle solved, and possible allow other script to listen to event when done by inheriting puzzle?
    /// </summary>
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private AudioSource audSource;
    [SerializeField] private AudioClip puzzleSolvedSound;
    [SerializeField] float delay;

    private void Awake()
    {
        if (puzzle != null)
        {
            puzzle.PuzzleSolved += PlaySoundOnSolved;
        }
    }

    void PlaySoundOnSolved(object sender, System.EventArgs e) {
        StartCoroutine(PlayDelayCo(delay, puzzleSolvedSound));
    }

    IEnumerator PlayDelayCo(float delay, AudioClip clip)
    {
        yield return new WaitForSeconds(delay);
        if (clip != null)
        {
            audSource.PlayOneShot(clip);
        }
        yield break;
    }
    
}


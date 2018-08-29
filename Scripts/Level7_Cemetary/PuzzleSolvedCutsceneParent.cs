using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;

public class PuzzleSolvedCutsceneParent : MonoBehaviour {

    [SerializeField] private PuzzleSolvedCutsceneChild[] puzzleChildren;
    [SerializeField] GameObject cameraRig;
    [SerializeField] Puzzle puzzleToSolve;
    //yield return startcoroutine

    private void Awake()
    {
        //StartCoroutine(PuzzleSolvedCutsceneCo());
        //Puzzle puzzle = GetComponent<Puzzle>();
        if (puzzleToSolve != null) {
            puzzleToSolve.PuzzleSolved += HandlePuzzleSolvedCutscene;
        }
    }

    void HandlePuzzleSolvedCutscene(object sender, System.EventArgs e) {
        StartCoroutine(PuzzleSolvedCutsceneCo());
    }

    private Vector3 tempCamRigPos;
    private Quaternion tempCamRigRot;
    public event Action OnPuzzleSolvedCutsceneEnd = delegate { };

    IEnumerator PuzzleSolvedCutsceneCo() {
        tempCamRigPos = cameraRig.transform.position;
        tempCamRigRot = cameraRig.transform.rotation;

        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }

        if (puzzleChildren != null) {
            for (int i = 0; i < puzzleChildren.Length; i++)
            {
                if(puzzleChildren[i].cutsceneCam)
                {
                    cameraRig.transform.position = puzzleChildren[i].cutsceneCam.transform.position;
                    cameraRig.transform.rotation = puzzleChildren[i].cutsceneCam.transform.rotation;
                    yield return new WaitForSeconds(0.2f);
                }

                var audioSource = puzzleChildren[i].GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayDelayed(0.2f);
                }


                if (puzzleChildren[i].animationController != null && puzzleChildren[i].cutsceneAnimStateName != null) {
                    puzzleChildren[i].animationController.Play(puzzleChildren[i].cutsceneAnimStateName);
                }

                if (puzzleChildren[i].floatObjectScript != null) {
                    puzzleChildren[i].floatObjectScript.StopFloat();
                }

                puzzleChildren[i].transform.Rotate(new Vector3(UnityEngine.Random.Range(20f, 30f), UnityEngine.Random.Range(20f, 30f), UnityEngine.Random.Range(20f, 30f)), UnityEngine.Random.Range(20f, 30f));

                if (puzzleChildren[i].rigidbodyComponent != null)
                {
                    puzzleChildren[i].rigidbodyComponent.useGravity = true;
                }

                if (puzzleChildren[i].renderToMessWith != null)
                {
                    Material[] mats = puzzleChildren[i].renderToMessWith.materials;
                    if (mats != null) {
                        for (int j=0; j<mats.Length; j++) {
                            //Debug.Log(mats[j].GetColor("Color"));
                            //Debug.Log( ShaderUtil.GetPropertyName(mats[j].shader, 3) + " and " + ShaderUtil.GetPropertyType(mats[j].shader,3) );
                            //Debug.Log( mats[j].GetFloat("Vector1_BF2A8939"));

                            StartCoroutine(DisappearCo(mats[j], puzzleChildren[i].meshWithDuration));
                        }
                    }

                    yield return new WaitForSeconds(puzzleChildren[i].meshWithDuration);
                }

                

                yield return new WaitForSeconds(puzzleChildren[i].cutsceneAnimDuration);
            }
        }

        OnPuzzleSolvedCutsceneEnd();
        FindObjectOfType<SceneController>().OnGameEventFired("load_endgame");

        yield break;
    }

    IEnumerator DisappearCo(Material mat, float duration) {
        float currentValue = 1;
        while (mat.GetFloat("Vector1_BF2A8939") > 0)
        {
            currentValue -= currentValue / (duration * 60);
            mat.SetFloat("Vector1_BF2A8939", currentValue);
            yield return null;
        }
    }

    void ResetChildren() {
        cameraRig.transform.position = tempCamRigPos;
        cameraRig.transform.rotation = tempCamRigRot;
        for (int i = 0; i < puzzleChildren.Length; i++)
        {

            if (puzzleChildren[i].floatObjectScript != null)
            {
                puzzleChildren[i].floatObjectScript.enabled = true;
            }

            if (puzzleChildren[i].rigidbodyComponent != null)
            {
                puzzleChildren[i].rigidbodyComponent.useGravity = false;
            }

        }
    }

}

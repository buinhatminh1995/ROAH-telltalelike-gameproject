using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPuzzleManager : Puzzle {

    [SerializeField] private GameObject[] correctOrder; // set solution order in Unity Editor
    [SerializeField] private Canvas keypadCanvas; 
    [SerializeField] private Material defaultMat, wrongMat, correctMat;
    [SerializeField] private Animator safeAnimator;
    [SerializeField] private GameObject photograph;
    private GameObject[] buttonList;
    private Text keypadText;
    private MeshRenderer keypadMeshRenderer;

    private int progress = 0, attempts = 0;
    private bool solvedPuzzle;

    void Awake()
    {
        buttonList = transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject).ToArray();
        keypadText = keypadCanvas.GetComponentInChildren<Text>();
        keypadMeshRenderer = GetComponent<MeshRenderer>();
    }

    public void buttonPressed(GameObject button)
    {
        if (!solvedPuzzle)
        {
            keypadText.text += button.name;

            if (button == correctOrder[progress])
            {
                progress++;
            }

            if (++attempts >= correctOrder.Length)
            {
                StopAllCoroutines();
                StartCoroutine(CheckSolution());
            }
        }
    }

    private IEnumerator CheckSolution()
    {
        ToggleButtons(false);
        yield return new WaitForSeconds(1f);
        if (progress == correctOrder.Length)
        {
            print("Safe unlocked");
            solvedPuzzle = true;
            SetKeypadMats(correctMat);

            // DO WHATEVER, puzzle solved
            OnPuzzleSolved(System.EventArgs.Empty);
            safeAnimator.enabled = true;
            safeAnimator.GetComponent<Collider>().enabled = true;
            photograph.GetComponent<Collider>().enabled = true;

            yield break;
        }
        else
        {
            //animate incorrect combination
            SetKeypadMats(wrongMat);
            yield return new WaitForSeconds(3f);
            SetKeypadMats(defaultMat);
        }
        resetPuzzle();
    }

    private void resetPuzzle()
    {
        print("Wrong Order, Puzzle Reset");
        solvedPuzzle = false;
        progress = attempts = 0;
        keypadText.text = "";
        ToggleButtons(true);
        SetKeypadMats(defaultMat);
    }

    private void ToggleButtons(bool state)
    {
        foreach (GameObject b in buttonList)
        {
            b.GetComponent<Collider>().enabled = state;
        }
    }

    private void SetKeypadMats(Material newMat)
    {
        Material[] mats = keypadMeshRenderer.materials;
        mats[1] = newMat;
        keypadMeshRenderer.materials = mats;
    }

}

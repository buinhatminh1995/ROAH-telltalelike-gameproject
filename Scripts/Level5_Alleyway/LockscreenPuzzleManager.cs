using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockscreenPuzzleManager : Puzzle {

    [SerializeField] private GameObject[] correctOrder; // set solution order in Unity Editor
    [SerializeField] private Canvas keypadCanvas, messagesCanvas, jayCanvas, gabiCanvas; 
    [SerializeField] private Color defaultColor, wrongColor, correctColor;
    [SerializeField] private Material unlockedMat;

    private GameObject[] buttonList;
    private Text keypadText;
    private Color keypadColor;
    private MeshRenderer keypadMeshRenderer;

    private int progress = 0, attempts = 0;
    private bool solvedPuzzle;

    void Awake()
    {
        buttonList = transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject).ToArray();
        keypadText = keypadCanvas.GetComponentInChildren<Text>();
        keypadColor = keypadText.color;
        keypadMeshRenderer = GetComponentInParent<MeshRenderer>();

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
            SetPasscodeColor(correctColor);

            // DO WHATEVER, puzzle solved
            OnPuzzleSolved(System.EventArgs.Empty);
            yield return new WaitForSeconds(2f);

            SetMats(unlockedMat);
            ToggleButtons(false);
            foreach (GameObject b in buttonList)
            {
                b.SetActive(false);
            }
            keypadCanvas.gameObject.SetActive(false);
            messagesCanvas.gameObject.SetActive(true);

            yield break;
        }
        else
        {
            //animate incorrect combination
            SetPasscodeColor(wrongColor);
            yield return new WaitForSeconds(3f);
            SetPasscodeColor(defaultColor);
        }
        resetPuzzle();
    }

    private void resetPuzzle()
    {
        print("Wrong Order, Puzzle Reset");
        solvedPuzzle = false;
        progress = attempts = 0;
        keypadText.text = "";
        SetPasscodeColor(defaultColor);
        ToggleButtons(true);
    }

    private void ToggleButtons(bool state)
    {
        foreach (GameObject b in buttonList)
        {
            b.GetComponent<Collider>().enabled = state;
        }
    }

    private void SetPasscodeColor(Color newColor)
    {
        keypadText.color = newColor;
    }

    private void SetMats(Material newMat)
    {
        Material[] mats = keypadMeshRenderer.materials;
        mats[1] = newMat;
        keypadMeshRenderer.materials = mats;
    }

    public void OnPointerClick(BaseEventData e)
    {
        //Debug.Log("clicked phone");
        messagesCanvas.gameObject.SetActive(false);
        jayCanvas.gameObject.SetActive(false);
        gabiCanvas.gameObject.SetActive(false);

        var pe = e as PointerEventData;
        switch(pe.pointerPress.transform.parent.name)
        {
            case "Jay":
                jayCanvas.gameObject.SetActive(true);
                pe.pointerPress.GetComponent<TriggerDialogue>().StartInk();
                break;

            case "Gabi":
                gabiCanvas.gameObject.SetActive(true);
                break;

            case "DialJay":
                jayCanvas.gameObject.SetActive(true);
                var ink = FindObjectOfType<InkScript>();
                ink.Delayed(1f, () => {
                    ink.StartStory("sc3a.call_jay");
                });
                GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().Move(Vector3.zero, false, false);
                break;

            default:
                messagesCanvas.gameObject.SetActive(true);
                break;
        }
    }

}

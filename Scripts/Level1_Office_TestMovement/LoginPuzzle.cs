using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPuzzle : Puzzle {

    [SerializeField] private InputField pwField;
    [SerializeField] private Canvas filesCanvas;
    [SerializeField] private Text errorLabel;
    [SerializeField] private string correctPassword, welcomeText;

    private void Awake()
    {
        errorLabel = pwField.transform.Find("Error Label").GetComponent<Text>();
        errorLabel.enabled = false;
        filesCanvas.enabled = false;
    }

    public void CheckPassword(bool eventFromButton)
    {
        if (Input.GetButtonDown("Submit") || eventFromButton)
        {
            if (pwField.text.Equals(correctPassword))
            {
                pwField.transform.parent.Find("User Label").GetComponent<Text>().text = welcomeText;
                pwField.gameObject.SetActive(false);
                Delayed(5f, () =>
                {
                    pwField.GetComponentInParent<Canvas>().enabled = false;
                    filesCanvas.enabled = true;
                });

                OnPuzzleSolved(System.EventArgs.Empty);
            }
            else
            {
                errorLabel.enabled = true;
                Delayed(3f, () => {
                    errorLabel.enabled = false;
                    if (!pwField.isFocused)
                        pwField.text = "";
                    pwField.Select();
                    pwField.ActivateInputField();
                });
                Debug.Log("wrong pass");
            }
        }
    }
}

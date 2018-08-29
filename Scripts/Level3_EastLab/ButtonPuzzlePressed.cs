using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPuzzlePressed : MonoBehaviour {

    private void OnMouseDown()
    {
        handlePush();
    }

    private void handlePush()
    {
        //print("button pressed: " + gameObject.name);
        GetComponentInParent<ButtonPuzzleManager>().buttonPressed(gameObject);
    }

}

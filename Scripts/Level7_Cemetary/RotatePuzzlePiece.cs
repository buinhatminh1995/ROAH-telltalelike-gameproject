using System.Collections;
using UnityEngine;
using System;

public class RotatePuzzlePiece : MonoBehaviour{
    enum Axis
    {
        x,
        y,
        z
    };

    [SerializeField] private Axis rotateDirection;
    private bool rotating;

    private int currentRotateTimes = 0;
    [SerializeField] private int correctRotateTimes;
    [SerializeField] private float rotateAngle = 90;
    [SerializeField] private float totalRotateSecond = 1f;
    //[SerializeField] private Transform 

    private void OnMouseDown()
    {
        //can rotate only when not already rotating
        if(!rotating)
        {
            //when clicked on, rotate by rotateAngle, increament currentRotateTime by 1
            switch (rotateDirection)
            {
                case Axis.x:
                    StartCoroutine(StartRotation(new Vector3(rotateAngle, 0, 0)));
                    break;
                case Axis.y:
                    StartCoroutine(StartRotation(new Vector3(0, rotateAngle, 0)));
                    break;
                case Axis.z:
                    StartCoroutine(StartRotation(new Vector3(0, 0, rotateAngle)));
                    break;
            }
        }
        
        
    }

    public event Action OnRotate = delegate { };
    public event Action OnStartRotate = delegate { };

    IEnumerator StartRotation(Vector3 rotation) {
        OnStartRotate();
        float i = 0;
        float rotateAnglePerFrame = (rotateAngle / totalRotateSecond) / 60;
        while (i < rotateAngle) {
            i += rotateAnglePerFrame;
            gameObject.transform.Rotate(((rotation / totalRotateSecond) / 60 ), Space.World);

            rotating = true;
            yield return null;
            //Debug.Log(i + " + " + rotateAnglePerFrame);
        }

        rotating = false;
        currentRotateTimes++;
        OnRotate();
        yield break;
    }

    public bool CheckCorrectAngle() {
        int turnNumber = 360 / (int)rotateAngle;
        return currentRotateTimes % turnNumber == correctRotateTimes;
    }
}

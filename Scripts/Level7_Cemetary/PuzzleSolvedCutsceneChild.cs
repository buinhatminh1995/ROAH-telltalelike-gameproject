using System.Collections;
using UnityEngine;

public class PuzzleSolvedCutsceneChild : MonoBehaviour {

    [SerializeField] public GameObject cutsceneCam;
    [SerializeField] public Animator animationController;
    [SerializeField] public string cutsceneAnimStateName;
    [SerializeField] public float cutsceneAnimDuration;
    [SerializeField] public FloatObject floatObjectScript;
    [SerializeField] public Rigidbody rigidbodyComponent;

    [SerializeField] public Renderer renderToMessWith;
    [SerializeField] public float meshWithDuration;
}

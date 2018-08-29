using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogueAnim : MonoBehaviour {
    [SerializeField] string idleStateName = "Idle";
    [SerializeField] string[] animStateNames;
    [SerializeField] float[] animStateDuration;
    [SerializeField] Animator animController;

    private BackButtonController backButtonController;

    Inspectable inspectable;
    //trigger based on inspectable's event 
    /// <summary>
    /// TODO: stop this based on dialogue finishes
    /// </summary>
    private void Awake()
    {
        inspectable = GetComponent<Inspectable>();
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle += discardparam => AnimateNpc();
        if (animStateNames.Length != animStateDuration.Length) {
            print("NPCDialogue has unmatching animStateNames and animStateDuration");
        }

        backButtonController = FindObjectOfType<BackButtonController>();
        backButtonController.OnCamBackButtonClick += StopDialogueAnim;
    }

    private void OnDestroy()
    {
        if (inspectable != null)
            inspectable.OnPlayerReachObserveAngle -= discardparam => AnimateNpc();

        backButtonController.OnCamBackButtonClick -= StopDialogueAnim;
    }

    private bool handleByInk = false;

    public void SetAnimExternal(string stateName, float animDuration = 3f, float blendWeight = 0.2f) {
        handleByInk = true;
        StopAllCoroutines();
        StartCoroutine(ExternalAnimateNpcCo(stateName, animDuration, blendWeight));
    }

    IEnumerator ExternalAnimateNpcCo(string stateName, float animDuration, float blendWeight) {
        animController.CrossFade(stateName, blendWeight);
        yield return new WaitForSeconds(animDuration);
        animController.CrossFade(idleStateName, blendWeight);
    }

    public void ReturnDefaultControl()
    {
        handleByInk = false;
        AnimateNpc();
    }

    void AnimateNpc()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateNpcCo());
    }

    IEnumerator AnimateNpcCo() {

        //while hasn't reached the duration for dialogue, keep looping the foe loop
        while (!handleByInk) {
            for (int i = 0; i < animStateNames.Length; i++)
            {

                animController.CrossFade(animStateNames[i], 0.2f);
            
                yield return new WaitForSeconds(animStateDuration[i]);
                            
            }

            yield return null;
        }

    }

    void StopDialogueAnim() {

        StopAllCoroutines();
        animController.CrossFade(idleStateName, 0f);
       
    }
}

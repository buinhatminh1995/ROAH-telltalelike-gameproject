using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Ground : ICanClick {

    public delegate void PlayerClickOnGroundObserveAngle(ObserveAngle observable);
    public event PlayerClickOnGroundObserveAngle OnPlayerClickOnGroundObserveAngle;

    protected override IEnumerator InspectableOnClickCo(NavMeshAgent agent)
    {
        //change cameraAngleBeforeReach
        //////return sth here
        //clickToMove.coroutineRunning = true;
        print("ground coroutine");
        ObserveAngle observable = GetComponent<ObserveAngle>();
        int currentFrame = 0;

        if (observable != null)
        {

            while (currentFrame < 10)
            {        
                currentFrame++;
                yield return null;
            }

            OnPlayerClickOnGroundObserveAngle(observable);
        }
        print("ground coroutine own stop");
        //clickToMove.coroutineRunning = false;
        yield break;
    }

    //if the object clicked on is the ground, set destination as the click point
    public override void OnClickMovementSetup(RaycastHit hitInfo, NavMeshAgent agent)
    {
        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            agent.SetDestination(hitInfo.point);
            agent.stoppingDistance = 0.2f;
        }
    }

    //if it reaches destination, do this (how do i check if it finishes the check and allow camera and other things to trigger)
    //maybe once a cutscene is triggered, let the sequence happen.
    public override void OnClickMovementAction(NavMeshAgent agent, ThirdPersonCharacter character)
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
            //maybe let camera follow here?
        }
        else
        {
            character.Move(Vector3.zero, false, false);
            
        }
    }
}

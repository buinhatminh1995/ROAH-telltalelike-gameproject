using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;
using System;


/// <summary>
/// for objects far away, move to it. 
/// when reach destination or already near destination, 
/// fire an event when player click on this for other classes to listen to
/// </summary>
public class InspectableNoCam : ICanClick {

    [SerializeField] float distanceStopInfront = 0.6f;

    public event Action OnPlayerReachAndClickOnObject = delegate { };

    protected override IEnumerator InspectableOnClickCo(NavMeshAgent agent)
    {    
        //////return sth here

        int currentFrame = 0;
        
        //Unity AI doesn the path calculation for the first few frames, during these frames, agent.hasPath return false, and agent.remainingDistance return 0, so we bypass these frames with a coroutine
        while (currentFrame < 10)
        {             
            currentFrame++;
            yield return null;
        }

        //when the calculation is done, we can check for has path
        while (agent.remainingDistance > agent.stoppingDistance && agent.remainingDistance > distanceStopInfront)
        {
            //we do nothing here to let the agent move to this location

            yield return null;
        }

        //when the agent has reach the destination, fire an event 
        OnPlayerReachAndClickOnObject();

        yield return null;

        print("inspectablenocam coroutine own stop");
        yield break;
    }

    public override void OnClickMovementSetup(RaycastHit hitInfo, NavMeshAgent agent)
    {
        //for object far away, move to it
        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            agent.SetDestination(hitInfo.point);
            agent.stoppingDistance = distanceStopInfront;
        }
    }

    public override void OnClickMovementAction(NavMeshAgent agent, ThirdPersonCharacter character)
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }

    }
}

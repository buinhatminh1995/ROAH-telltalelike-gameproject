using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Inspectable : ICanClick {

    //IEnumerator myOnClickCo;
    ClickToMove clickToMove;

    protected override void Awake() {
        base.Awake();
        clickToMove = FindObjectOfType<ClickToMove>();
    }

    [SerializeField] float proximityDistanceToChangeCameraAngle = 0.3f;

    public delegate void PlayerStartMovingObserveAngleEvent(ObserveAngle observable);
    public event PlayerStartMovingObserveAngleEvent OnPlayerStartMovingObserveAngle;

    public delegate void PlayerReachObserveAngleEvent(ObserveAngle observable);
    public event PlayerReachObserveAngleEvent OnPlayerReachObserveAngle;

    protected override IEnumerator InspectableOnClickCo(NavMeshAgent agent)
    {
        //clickToMove.coroutineRunning = true;
        //change cameraAngleBeforeReach
        //////return sth here
        print("inspectable coroutine");
        ObserveAngle observable = GetComponent<ObserveAngle>();

        int currentFrame = 0;
        if (observable != null && clickToMove != null)
        {

            //Unity AI doesn the path calculation for the first few frames, during these frames, agent.hasPath return false, and agent.remainingDistance return 0, so we bypass these frames with a coroutine
            while (currentFrame < 10)
            {
                //if (clickToMove.reset)
                //{
                    
                //    clickToMove.reset = false;
                //    Debug.Log(clickToMove.reset);
                //    yield break;
                //}
                currentFrame++;
                yield return null;
            }

            //when the calculation is done, we can check for has path
            while (agent.remainingDistance > agent.stoppingDistance && agent.remainingDistance > 0.5f)
            {
                //anytime there is a click on the ClickToMove object, stop this coroutine
                //if (clickToMove.reset)
                //{
                    
                //    clickToMove.reset = false;
                //    Debug.Log(clickToMove.reset);
                //    yield break;
                //}
                
                //print("I'm still far away");

                OnPlayerStartMovingObserveAngle(observable);

                yield return null;
            }

            
            //if (clickToMove.reset)
            //{
                
            //    clickToMove.reset = false;
            //    Debug.Log(clickToMove.reset);
            //    yield break;
            //}
            
            //print("I'm here");
            //ObserveAngle observable = GetComponent<ObserveAngle>();

            OnPlayerReachObserveAngle(observable);

            yield return null;
        }
        //clickToMove.coroutineRunning = false;
        print("inspectable coroutine own stop");
        yield break;
    }

    public override void OnClickMovementSetup(RaycastHit hitInfo, NavMeshAgent agent)
    {
        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            agent.SetDestination(hitInfo.point);
            agent.stoppingDistance = proximityDistanceToChangeCameraAngle;
        }
    }

    public override void OnClickMovementAction(NavMeshAgent agent, ThirdPersonCharacter character)
    {
        //if the destination is reached, switch camera view based on the ObserveAngle component
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            //agent.ResetPath();
            character.Move(Vector3.zero, false, false);


            //on reaching this object, check if it has the ObserveObject component and apply the actions in it(probably switching camera at this point)

            //OnPlayerReachInspectable(this);

            //ObserveAngle observable = GetComponent<ObserveAngle>();

            //if (observable != null) {
            //    OnPlayerReachObserveAngle(observable);
            //}
        }
    }

    
}

using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;
using System;

public class InspectableAnywhere : ICanClick
{
    public event Action OnClickFromAnywhere = delegate { };

    private void OnMouseDown()
    {
        OnClickFromAnywhere();
    }

    public void FireOnClickFromAnywhere()
    {
        OnClickFromAnywhere();
    }

    protected override IEnumerator InspectableOnClickCo(NavMeshAgent agent)
    {
        yield break;
    }

    public override void OnClickMovementAction(NavMeshAgent agent, ThirdPersonCharacter character) {}
    public override void OnClickMovementSetup(RaycastHit hitInfo, NavMeshAgent agent) {}
}

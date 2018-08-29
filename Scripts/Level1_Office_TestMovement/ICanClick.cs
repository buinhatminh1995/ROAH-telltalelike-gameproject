using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections;

public abstract class ICanClick : MonoBehaviour {

    public Coroutine clickCommand;

    protected ICanClick[] clickables;

    protected virtual void Awake()
    {
        clickables = FindObjectsOfType<ICanClick>();
    }

    public virtual void OnClickCameraSetup(NavMeshAgent agent)
    {
        for (int i = 0; i < clickables.Length; i++)
        {
            if (clickables[i].clickCommand != null)
                clickables[i].StopCoroutine(clickables[i].clickCommand);

        }

        clickCommand = StartCoroutine(InspectableOnClickCo(agent));
    }

    protected abstract IEnumerator InspectableOnClickCo(NavMeshAgent agent);

    public abstract void OnClickMovementSetup(RaycastHit hitInfo, NavMeshAgent agent);

    public abstract void OnClickMovementAction(NavMeshAgent agent, ThirdPersonCharacter character);
}

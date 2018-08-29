using System.Collections;
using UnityEngine;

public class FloatObject : MonoBehaviour {

    private Vector3 pos;
    [SerializeField] bool floatFromBeginning = true;
    [SerializeField] float verticalOffset = 0.001f;
    [SerializeField] float slowSpeed = 0.2f;
    [SerializeField] float delayFloat;
     GameObject objectToFloat;
    

    private void Start()
    {
        if(objectToFloat == null) 
        objectToFloat = this.gameObject;

        pos = objectToFloat.transform.position;

        if (floatFromBeginning) {
            StartFloat(delayFloat);
        }
    }

    void StartFloat(float delay) {
        StartCoroutine(StartFloatCo(delay));
    }

    IEnumerator StartFloatCo(float delay) {
        yield return new WaitForSeconds(delay);
        while(true)
        {
            pos.y += verticalOffset * Mathf.Sin(slowSpeed * Time.time);
            objectToFloat.transform.position = pos;
            //Debug.Log(pos.y);
            yield return null;
        }

    }

    public void StopFloat() {
        StopAllCoroutines();
    }
}

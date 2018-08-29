using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendWeightController : MonoBehaviour {
	public SkinnedMeshRenderer frendMeshRenderer;
	public int blendWeightIndex = 4;
	public float blinkingInterval = 3f;
	public float startBlendWeight = 0f;
	public float endBlendWeight = 100f;
	public float startToEndDuration = 0.15f;
	public float endToStartDuration = 0.2f;
	IEnumerator BlendCo;

	// Use this for initialization
	void Start () {
		frendMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer> ();
		frendMeshRenderer.SetBlendShapeWeight(blendWeightIndex, startBlendWeight);
		//call this from the animation controller when switching state
		BlendCo = BShapeAnimate (blendWeightIndex);
		StartCoroutine (BlendCo);
	}

	IEnumerator BShapeAnimate(int bsSliderIndex, bool loop = true) {
		print ("Start BShapAnimate Coroutine");
		while (true) {
			///blink
			yield return new WaitForSeconds(blinkingInterval);
			//eyes close over time
			yield return LerpStartEnd(bsSliderIndex, startToEndDuration, startBlendWeight, endBlendWeight);
			yield return LerpStartEnd(bsSliderIndex, endToStartDuration, endBlendWeight, startBlendWeight);
			if (!loop)
				yield break;
		}
	}

	IEnumerator LerpStartEnd(int bsSliderIndex, float duration, float start, float finish) {
		float time = 0;
		while (time < duration) {
			time += Time.deltaTime;
			float blendShapeValue;
			blendShapeValue = Mathf.Lerp(start, finish, time / duration);
			frendMeshRenderer.SetBlendShapeWeight(bsSliderIndex, blendShapeValue);
			yield return null;
		}
	}

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			
			StopCoroutine(BlendCo);
//
//			if (blendWeightIndex < 4) {
//				blendWeightIndex++;
//			} else
//				blendWeightIndex = 0;
			BlendCo = BShapeAnimate (blendWeightIndex);
			StartCoroutine(BlendCo);
		}
	}
}

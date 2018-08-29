using UnityEngine;
using System.Collections;

public class InteractionSound : MonoBehaviour {

    [SerializeField] private AudioSource audSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] float delay;

    private void Awake()
    {
        ICanClickToActivate clickActivateable = GetComponent<ICanClickToActivate>();
        if (clickActivateable != null) {
            clickActivateable.OnOpen += HandleOpen;
            clickActivateable.OnClose += HandleClose;
        }
        

        RotatePuzzlePiece piece = GetComponent<RotatePuzzlePiece>();
        if (piece != null)
        {
            piece.OnStartRotate += HandleOpen;
        }

    }

    IEnumerator PlayDelayCo(float time, AudioClip clip) {
        yield return new WaitForSeconds(time);
        if (openSound != null)
        {
            audSource.PlayOneShot(clip);
        }
    }

    void HandleOpen() {
        StartCoroutine(PlayDelayCo(delay, openSound));
        
    }

    void HandleClose() {
        StartCoroutine(PlayDelayCo(delay, closeSound));
  
    }
}

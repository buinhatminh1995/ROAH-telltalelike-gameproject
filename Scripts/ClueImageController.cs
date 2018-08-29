using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.PostProcessing;

public class ClueImageController : MonoBehaviour {

    [SerializeField] public GameObject objectClue;
    [SerializeField] private Image clueImage;

    private ClickOpenClueImageBlurryBackgroundUI[] clickables;
    private BackButtonController backButtonController;

    private DepthOfField depthOfFieldLayer;
    private PostProcessVolume postProcessVolume;

    private static ClueImageController _instance;
    public static ClueImageController Instance;

    void OnEnable()
    {
        //if (_instance != null && _instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //_instance = this;
        //DontDestroyOnLoad(this.gameObject);
        //print("clueimage onenable");

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        clickables = FindObjectsOfType<ClickOpenClueImageBlurryBackgroundUI>();
        if (clickables != null) {
            for (int i = 0; i < clickables.Length; i++)
            {
                clickables[i].OnClickObjectWithClue += OpenImageUI;
            }
        }

        backButtonController = FindObjectOfType<BackButtonController>();
        if (backButtonController != null) {
            backButtonController.OnClueBackButtonClick += CloseImageUI;
        }

        // Get PostFX Profile
        postProcessVolume = Camera.main.GetComponentInChildren<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out depthOfFieldLayer);
    }

    public void AttachOpenImageUIListener(ClickOpenClueImageBlurryBackgroundUI clickable)
    {
        clickable.OnClickObjectWithClue += OpenImageUI;
    }

    public event Action OnOpenImageUI = delegate { };

    void OpenImageUI(Sprite sprite) {
        if (objectClue.activeSelf == false) {
            // enable PostFX Depth of Field
            depthOfFieldLayer.enabled.value = true;

            //switch the sprite to the one associated with this object
            clueImage.sprite = sprite;
            objectClue.SetActive(true);
            OnOpenImageUI();
        }
    }

    public event Action OnCloseImageUI = delegate { };

    void CloseImageUI() {
        if (objectClue.activeSelf == true) {
            // disable PostFX Depth of Field
            depthOfFieldLayer.enabled.value = false;

            objectClue.SetActive(false);
            OnCloseImageUI();
        }
    }

    void OnDisable()
    {
        //print("clueimage ondisable");

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;

        clickables = FindObjectsOfType<ClickOpenClueImageBlurryBackgroundUI>();
        if (clickables != null)
        {
            for (int i = 0; i < clickables.Length; i++)
            {
                clickables[i].OnClickObjectWithClue -= OpenImageUI;
            }
        }

        if (backButtonController != null)
        {
            backButtonController.OnClueBackButtonClick -= CloseImageUI;
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        OnEnable();
    }
}

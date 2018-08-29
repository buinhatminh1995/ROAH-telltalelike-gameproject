using UnityEngine;
using System;

public class BackButtonController : MonoBehaviour {

    [SerializeField] private GameObject camBackButton;
    [SerializeField] private GameObject clueBackButton;

    private ObjectClickTriggerBackButton[] clickTriggers;
    private CamLookAt camLookAt;
    private Ground ground;

    private static BackButtonController _instance;
    public static BackButtonController Instance;

    void OnEnable()
    {
        //print("backbutton onenable");
        //if (_instance != null && _instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //_instance = this;
        //DontDestroyOnLoad(this.gameObject);

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        clickTriggers = FindObjectsOfType<ObjectClickTriggerBackButton>();
        if (clickTriggers != null)
            for (int i = 0; i < clickTriggers.Length; i++)
            {
                clickTriggers[i].OnObjectClickTriggerBackButton += HandleTriggerClueBackButton;
            }

        camLookAt = FindObjectOfType<CamLookAt>();
        if (camLookAt != null)
            camLookAt.OnCameraChangeTriggerBackButton += HandleTriggerCamBackButton;

        ground = FindObjectOfType<Ground>();
        if (ground != null)
            ground.OnPlayerClickOnGroundObserveAngle += ClickCamBackButton;
    }

    public void AttachHandleClueBackButtonListener(ObjectClickTriggerBackButton clickTrigger)
    {
        clickTrigger.OnObjectClickTriggerBackButton += HandleTriggerClueBackButton;
    }

    void HandleTriggerCamBackButton() {
        if(camBackButton.activeSelf == false)
            camBackButton.SetActive(true);
    }

    void HandleTriggerClueBackButton()
    {
        if (clueBackButton.activeSelf == false)
            clueBackButton.SetActive(true);
    }

    public event Action OnCamBackButtonClick = delegate { };
    public event Action OnClueBackButtonClick = delegate { };

    public void ClickClueBackButton()
    {
        OnClueBackButtonClick();
        clueBackButton.SetActive(false);
    }

    public void ClickCamBackButton()
    {
        OnCamBackButtonClick();
        camBackButton.SetActive(false);
    }

    public void ClickCamBackButton(ObserveAngle observable)
    {
        OnCamBackButtonClick();
        camBackButton.SetActive(false);
    }
    
    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;

        if (clickTriggers != null)
            for (int i = 0; i < clickTriggers.Length; i++)
            {
                clickTriggers[i].OnObjectClickTriggerBackButton -= HandleTriggerClueBackButton;
            }
        
        if (camLookAt != null)
            camLookAt.OnCameraChangeTriggerBackButton -= HandleTriggerCamBackButton;

        if (ground != null)
            ground.OnPlayerClickOnGroundObserveAngle -= ClickCamBackButton;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        OnEnable();
    }
    
}

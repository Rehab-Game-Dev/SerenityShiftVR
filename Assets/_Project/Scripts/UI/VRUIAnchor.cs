using UnityEngine;

public class VRUIAnchor : MonoBehaviour
{
    [Header("PC Settings")]
    public RenderMode pcRenderMode = RenderMode.ScreenSpaceOverlay;

    [Header("VR Settings")]
    public Vector3 localPosition = new Vector3(0, 0, 2.0f);
    public Vector3 localRotation = Vector3.zero;
    public float uiScale = 0.001f;

    private Canvas canvas;
    private Camera targetCamera;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        UpdateUIMode();
    }

    void Update()
    {
        // Ensure UI stays tracked even if camera changes
        if (AuthManager.VR_ON && transform.parent == null)
        {
             UpdateUIMode();
        }
    }

    public void UpdateUIMode()
    {
        if (canvas == null) canvas = GetComponent<Canvas>();
        if (canvas == null) return;

        bool isVR = AuthManager.VR_ON;
        
        if (isVR)
        {
            targetCamera = Camera.main;
            if (targetCamera == null)
            {
                var camObj = GameObject.Find("Main Camera");
                if (camObj != null) targetCamera = camObj.GetComponent<Camera>();
            }

            if (targetCamera != null)
            {
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = targetCamera;
                
                transform.SetParent(targetCamera.transform);
                transform.localPosition = localPosition;
                transform.localRotation = Quaternion.Euler(localRotation);
                transform.localScale = new Vector3(uiScale, uiScale, uiScale);
                
                result_Log("UI Anchor: Canvas " + gameObject.name + " attached to VR Camera.");
            }
        }
        else
        {
            canvas.renderMode = pcRenderMode;
        }
    }

    private void result_Log(string msg) {
        // Simple internal log helper to avoid errors if I add too much logic
        Debug.Log(msg);
    }
}
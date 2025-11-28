using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    [Header("Camera")]
    public Camera mainCamera;
    public float zoomDuration = 0.6f;
    public float returnDuration = 0.6f;
    public AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float zoomFOV = 35f;

    [Header("UI (Simple click-to-win)")]
    public GameObject minigamePanel;   // A fullscreen panel with a button inside
    public Button winButton;           // This button is the "click to succeed"

    [Header("Player Control")]
    [Tooltip("Movement/look scripts to disable during minigame (e.g., PlayerMovement, MouseLook).")]
    public MonoBehaviour[] scriptsToDisableDuringMinigame;

    // Runtime state
    public bool IsOpen { get; private set; } = false;

    private Vector3 camStartPos;
    private Quaternion camStartRot;
    private float camStartFOV;

    private Transform targetZoomPoint;

    private Action _onSuccess;
    private Action _onCancel;
    private bool _succeeded;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (!mainCamera) mainCamera = Camera.main;
        if (minigamePanel) minigamePanel.SetActive(false);
        if (winButton) winButton.onClick.AddListener(OnWinClicked);
    }

    public void OpenMinigame(Transform zoomPoint, Action onSuccess, Action onCancel = null)
    {
        if (IsOpen) return;
        targetZoomPoint = zoomPoint;
        _onSuccess = onSuccess;
        _onCancel = onCancel;
        _succeeded = false;

        StartCoroutine(OpenRoutine());
    }

    public void CloseMinigame() => StartCoroutine(CloseRoutine());

    void OnWinClicked()
    {
        _succeeded = true; 
        CloseMinigame();
    }

    IEnumerator OpenRoutine()
    {
        IsOpen = true;

        // Save camera state
        camStartPos = mainCamera.transform.position;
        camStartRot = mainCamera.transform.rotation;
        camStartFOV = mainCamera.fieldOfView;

        // Disable player input
        SetPlayerControls(false);

        // Zoom to target
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, zoomDuration);
            float k = zoomCurve.Evaluate(Mathf.Clamp01(t));
            mainCamera.transform.position = Vector3.Lerp(camStartPos, targetZoomPoint.position, k);
            mainCamera.transform.rotation = Quaternion.Slerp(camStartRot, targetZoomPoint.rotation, k);
            mainCamera.fieldOfView = Mathf.Lerp(camStartFOV, zoomFOV, k);
            yield return null;
        }

        // Show minigame UI
        minigamePanel.SetActive(true);

        // (Cursor free for clicking)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator CloseRoutine()
    {
        // Hide UI first
        if (minigamePanel) minigamePanel.SetActive(false);

        // Zoom back
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;
        float startFOV = mainCamera.fieldOfView;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, returnDuration);
            float k = zoomCurve.Evaluate(Mathf.Clamp01(t));
            mainCamera.transform.position = Vector3.Lerp(startPos, camStartPos, k);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, camStartRot, k);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, camStartFOV, k);
            yield return null;
        }

        // Re-enable player input and lock cursor again
        SetPlayerControls(true);

        // Fire the appropriate callback
        if (_succeeded)
            _onSuccess?.Invoke();
        else
            _onCancel?.Invoke();

        // Clear state
        _onSuccess = null;
        _onCancel = null;
        targetZoomPoint = null;
        IsOpen = false;
    }

    void SetPlayerControls(bool enabled)
    {
        foreach (var m in scriptsToDisableDuringMinigame)
            if (m) m.enabled = enabled;

        // If your gameplay uses a locked cursor, refresh it here
        if (enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

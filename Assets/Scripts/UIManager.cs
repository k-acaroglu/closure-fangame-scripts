using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI remainingPingText;

    void Awake()
    {
        // Singleton pattern, destroy other instances of UIManager and make the original one the Manager
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetRemainingPingsText(int value)
    {
        if (remainingPingText)
        {
            remainingPingText.text = $"Remaining pings:  {value}";
        }
    }

    public void ShowInteractText(bool show, string message = "Press F to interact")
    {
        if (!interactText)
        {
            return;
        }
        interactText.text = message;
        interactText.gameObject.SetActive(show);
    }
}

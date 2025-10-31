using UnityEngine;

public class PingManager : MonoBehaviour
{
    public static PingManager Instance { get; private set; }
    public int Remaining { get; private set; }

    void Awake()
    {
        // Singleton pattern, destroy other instances of PingManager and make the original one the Manager
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        Remaining = GameObject.FindGameObjectsWithTag("isPing").Length;
        UIManager.Instance.SetRemainingPingsText(Remaining);
    }

    public void ReducePingByOne()
    {
        Remaining = Mathf.Max(0, Remaining - 1);
        UIManager.Instance.SetRemainingPingsText(Remaining);
        if (Remaining <= 0)
        {
            UIManager.Instance.SetStageClearedTextActive(true);
        }
    }
}

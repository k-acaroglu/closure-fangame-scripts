using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShowActionOnTrigger : MonoBehaviour
{
    [Header("Interacting Objects")]
    public GameObject interactableObject;
    
    // public TextMeshProUGUI interactText;
    // public TextMeshProUGUI remainingPingText;
    private bool playerInRange = false;

    void Start()
    {
        UIManager.Instance.SetRemainingPingsText(PingManager.Instance.Remaining);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("isPlayer"))
        {
            playerInRange = true;
            UIManager.Instance.ShowInteractText(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("isPlayer"))
        {
            playerInRange = false;
            UIManager.Instance.ShowInteractText(false);
        }
    }

    void Interact()
    {
        Debug.Log("Write Minigame logic here");
        //TODO: Start minigames via here!!!
        // remainingPingCount--;

        UIManager.Instance.ShowInteractText(false);
        PingManager.Instance.ReducePingByOne();
        
        Destroy(interactableObject);
        Destroy(gameObject);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }
}

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

    UIManager uiManager;
    private int remainingPingCount;
    private bool playerInRange = false;

    void Start()
    {
        remainingPingCount = CountObjectsWithTag("isPing");
        uiManager.SetRemainingPingsText(remainingPingCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("isPlayer"))
        {
            playerInRange = true;
            uiManager.ShowInteractText(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("isPlayer"))
        {
            playerInRange = false;
            uiManager.ShowInteractText(false);
        }
    }

    void Interact()
    {
        Debug.Log("Write Minigame logic here");
        //TODO: Start minigames via here!!!
        remainingPingCount--;

        uiManager.ShowInteractText(false);
        uiManager.SetRemainingPingsText(remainingPingCount);
        
        Destroy(interactableObject);
        Destroy(gameObject);
    }
    
    int CountObjectsWithTag(String tagName)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tagName);
        return taggedObjects.Length;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }
}

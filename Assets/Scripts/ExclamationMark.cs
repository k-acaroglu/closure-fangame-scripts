using System;
using Unity.VisualScripting;
using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    Camera cam;
    private Renderer rend;
    
    public float exclamationTurnSpeed = 90f;
    public Transform player;
    
    public float maxAlphaDistance;
    public float minAlphaDistance;

    [SerializeField] private Material exclamationMaterial;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (!cam) cam = Camera.main;
        if (!cam) return;
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * exclamationTurnSpeed, Space.World);
        
        float distance = Vector3.Distance(player.position, transform.position);
        float lerpedValue = Mathf.InverseLerp(minAlphaDistance, maxAlphaDistance, distance);

        Color color = rend.material.color;
        color.a = lerpedValue;
        rend.material.color = color;
    }
}

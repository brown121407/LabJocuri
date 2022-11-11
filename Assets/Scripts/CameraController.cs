using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField]
    private Transform sphereTransform;

    [SerializeField] 
    private Rigidbody sphereRigidbody;

    [SerializeField] 
    private PlayerController playerController;

    [SerializeField] 
    private Transform planeTransform;

    [SerializeField] private GameManager gameManager;
    
    private void Start()
    {
        _offset = transform.position - sphereTransform.position;
        transform.LookAt(sphereTransform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && _offset.magnitude > 3)
        {
            _offset *= 0.9f;
        }
        if (Input.GetKeyDown(KeyCode.P) && _offset.magnitude <= 6)
        {
            _offset *= 1.1f;
        }
        
        if (!gameManager.Won && Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform == planeTransform)
                {
                    sphereRigidbody.AddForce(Vector3.Normalize(hit.point + Vector3.up * 0.5f - sphereTransform.position));
                }
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = sphereTransform.position + _offset;
    }
}

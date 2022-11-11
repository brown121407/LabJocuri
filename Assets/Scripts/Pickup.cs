using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Regular, Red
    }
    
    [SerializeField]
    private float _rotationSpeed = 1.0f;

    public PickupType type;

    private void Update()
    {
        transform.Rotate(new Vector3(30, 60, 90) * Time.deltaTime * _rotationSpeed);
    }
}

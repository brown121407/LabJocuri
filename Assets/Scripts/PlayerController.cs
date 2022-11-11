using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;

    [SerializeField] private GameManager gameManager;

    private Rigidbody _rb;

    private GameObject _pickup;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        SpawnPickup();
    }

    private void FixedUpdate()
    {
        if (!gameManager.Won)
        {
            var oX = Input.GetAxis("Horizontal");
            var oZ = Input.GetAxis("Vertical");
            _rb.AddForce(new Vector3(oX, 0, oZ) * speed);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            speed = Mathf.Abs(speed);
            gameManager.UpdateScore();

            var pickup = other.gameObject.GetComponent<Pickup>();
            if (pickup.type == Pickup.PickupType.Red)
            {
                speed *= -1;
            }
            
            other.gameObject.SetActive(false);
            SpawnPickup();
        }
    }

    private void SpawnPickup()
    {
        var index = Random.Range(0, gameManager.pickups.Count);
        var pickupPosition = GameManager.RandomPosition(transform.position);
        var pickup = gameManager.pickups[index];
        pickup.transform.position = pickupPosition;
        pickup.SetActive(true);
    }
}

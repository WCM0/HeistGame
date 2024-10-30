using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberySystem : MonoBehaviour
{
    public float rewardAmount = 100.0f; // Amount of money the player earns when entering
    [SerializeField] private bool isRobbed = false; // Track if this target has been robbed

    public Car carController;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the robbery area: " + gameObject.name);
            carController = other.GetComponent<Car>();

            if (carController != null && !isRobbed)
            {
                carController.AddMoney(rewardAmount); // Add money to player
                isRobbed = true; // Mark this target as robbed
                Debug.Log("Awarded $" + rewardAmount + " to the player.");
                gameObject.SetActive(false); // Disable the target after robbing
            }
            else if (isRobbed)
            {
                Debug.Log("This target has already been robbed.");
            }
            else
            {
                Debug.LogError("PlayerController not found on Player GameObject.");
            }
        }
        else
        {
            Debug.Log("Non-player object entered the robbery area: " + other.name);
        }
    }
}


using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class MoneyUIController : MonoBehaviour
{
    public Car carController; // Reference to the player controller
    public TextMeshProUGUI moneyText; // Reference to the TextMeshPro UI element

    private void Update()
    {
        UpdateMoneyDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        // Check if playerController is assigned
        if (carController != null)
        {
            moneyText.text = "Money: $" + carController.money.ToString("F2"); // Display the money amount
        }
        else
        {
            Debug.LogWarning("PlayerController reference is not set in MoneyUIController.");
        }
    }
}

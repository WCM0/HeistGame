using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    public Transform centerOfMass;
    public float motorTorque = 1500f;
    public float maxSteer = 30f;
    public bool carActive;

    public float Steer { get; set; }
    public float Throttle { get; set; }
    public float money = 0.0f; // Starting money
    [SerializeField] private RobberySystem currentRobberyTarget; // Reference to the current robbery target

    private Rigidbody _rigidbody;
    private Wheel[] wheels;

    // Start is called before the first frame update
    void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
        GetComponent<RobberySystem>();
    }

    // Update is called once per frame
    void Update()
    {

        if (carActive == true)
        {
            Steer = GameManager.Instance.InputController.SteerInput;
            Throttle = GameManager.Instance.InputController.ThrottleInput;

            foreach (var wheel in wheels)
            {

                wheel.SteerAngle = Steer * maxSteer;
                wheel.Torque = Throttle * motorTorque;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered a robbery area
        if (other.CompareTag("RobberyTarget"))
        {
            currentRobberyTarget = other.GetComponent<RobberySystem>();
            if (currentRobberyTarget != null)
            {
                Debug.Log("Entered robbery target area: " + other.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the reference when exiting the robbery area
        if (other.CompareTag("RobberyTarget"))
        {
            currentRobberyTarget = null;
            Debug.Log("Exited robbery target area: " + other.name);
        }
    }

    public void AddMoney(float amount)
    {
        money += amount;
        Debug.Log("Player earned $" + amount + ". Total money: $" + money);
    }
}

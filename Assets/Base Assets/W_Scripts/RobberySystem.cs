using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberySystem : MonoBehaviour
{
    [System.Serializable]
    public class RobberyTarget
    {
        public string targetName;
        public int lootAmount;
        public float robberyTime;  // Directly customize time for each location
        public bool isRobbed = false;
        public bool ableToRob;
        public Collider targetCollider;
    }

    public List<RobberyTarget> robberyTargets;  // List of places to rob
    [SerializeField] private RobberyTarget currentTarget;
    public bool isRobbing = false;

    private void Start()
    {
        if (robberyTargets.Count > 0)
        {
            Debug.Log("Robbery system ready. Enter the area to start robbing.");
        }
        else
        {
            Debug.LogError("No robbery targets available.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered: " + other.gameObject.name);  // Log to see if anything is detected

        if (isRobbing) return;

        foreach (var target in robberyTargets)
        {
            if (other == target.targetCollider && !target.isRobbed)
            {
                Debug.Log("Target detected: " + target.targetName);
                currentTarget = target;
                StartRobbery(currentTarget);
                break;
            }
        }
    }

    private void StartRobbery(RobberyTarget target)
    {
        if (isRobbing)
        {
            Debug.Log("Already robbing a location.");
            return;
        }

        isRobbing = true;
        Debug.Log("Robbing: " + target.targetName + " | Robbery Time: " + target.robberyTime + "s");
        StartCoroutine(PerformRobbery(target, target.robberyTime));
    }

    private IEnumerator PerformRobbery(RobberyTarget target, float robberyTime)
    {
        yield return new WaitForSeconds(robberyTime);

        target.isRobbed = true;
        Debug.Log("Successfully robbed " + target.targetName + "! Loot collected: " + target.lootAmount);

        isRobbing = false;
    }
}


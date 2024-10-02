using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public Transform waypointParent;
    public float waypointDistance = 5f;
    public float range = 10f;
    public NavMeshAgent agent;

    public Transform[] points;
    public int currentWaypoint = 1;
    private float onMeshThreshold = 3;
    public Transform playerTransform;

    public bool isPatrolling;
    public bool isChasingPlayer;


    /*
     * int = 1, 2, 3
     * float = 0.125, .5f, 1.1f
     * bool = true, false
     * string = "Hello", "World"
     * char = 'C', 'O', 'O', 'L'
     */

    // Start is called before the first frame update
    void Start()
    {
        points = waypointParent.GetComponentsInChildren<Transform>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnDrawGizmos()
    {
        points = waypointParent.GetComponentsInChildren<Transform>();
        if (points != null)
        {
            Gizmos.color = Color.red;
            for (int i = 1; i < points.Length - 1; i++)
            {
                Transform pointA = points[i];
                Transform pointB = points[i + 1];
                Gizmos.DrawLine(pointA.position, pointB.position);
            }

            for (int i = 1; i < points.Length; i++)
            {
                Gizmos.DrawSphere(points[i].position, waypointDistance);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isPatrolling == true)
        {
            if (points.Length == 0)
            {
                return;
            }
            if(currentWaypoint >= points.Length)
            {
                currentWaypoint = 1;
            }
            print("Current Waypoint: " + currentWaypoint);
            // Get the current waypoint
            Transform currentPoint = points[currentWaypoint];
            // Move towards current waypoint
            // transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, Speed * Time.deltaTime);
            agent.SetDestination(currentPoint.position);
            // Check if distance between waypoint is close
            float distance = Vector3.Distance(transform.position, currentPoint.position);
            float playerDistance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < waypointDistance)
            {
                // Switch to next waypoint
                currentWaypoint = Random.Range(1, points.Length);
            }
            if(playerDistance <= range)
            {
                isPatrolling = false;
                isChasingPlayer = true;
            }
            // >>ERROR HANDLING<<
            // If currentWaypoint is outside array length
            // Reset back to 1
        }
    }

    public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        UnityEngine.AI.NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (UnityEngine.AI.NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, UnityEngine.AI.NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }
}

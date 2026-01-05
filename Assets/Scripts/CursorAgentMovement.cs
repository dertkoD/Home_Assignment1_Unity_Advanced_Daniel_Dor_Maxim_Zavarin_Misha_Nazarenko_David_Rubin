using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CursorAgentMovement : MonoBehaviour
{
    //Possibility of adding a large number of participants
    [SerializeField] private List<NavMeshAgent> agents = new List<NavMeshAgent>(); 
    [SerializeField] private Camera cam;

    private HashSet<NavMeshAgent> arrivedAgents = new(); // registry of arrived agents
    private bool hasClicked;
    private bool isGameActive = true; // Game activity flag

    private void Start()
    {
        if (!cam)
            Debug.Log("No camera was found");
        if (agents.Count==0)
            Debug.Log("No participants were added");
    }

    void Update()
    {
        // Check if game is active
        if (!isGameActive) return;
        
        if (CheckAgentsEnabled())
        {
            //Mouse destination setting
            if ( Mouse.current.leftButton.isPressed)
            {
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.value);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    SetAgentsMovement(hit.point);
                }
            } 
            
            if (hasClicked) CheckArrival();
        }
       
    }
    
    // Method to control game activity (called from GameMenuManager)
    public void SetGameActive(bool active)
    {
        isGameActive = active;
    }
    
    public bool IsGameActive()
    {
        return isGameActive;
    }

    //Set destination foreach agent
    private void SetAgentsMovement(Vector3 finish)
    {
        hasClicked = true; // flag to only display the arrival log after the user command
        
        arrivedAgents.Clear();
        
        foreach (var ag in agents)
        {
            ag.SetDestination(finish);
        }
    }

    //Check if all agents are enabled
    private bool CheckAgentsEnabled()
    {
        foreach (var ag in agents)
        {
            if (!ag.enabled || !ag.gameObject.activeInHierarchy)
            {
                Debug.Log($"Agent {ag.name} is disabled");
                return false;
            }
               
        }
        return true;
    }
    
    //Check if every agent arrived at destination and display a log message
    private void CheckArrival()
    {
        foreach (var ag in agents)
        {
            if (arrivedAgents.Contains(ag))
                continue;

            if (!ag.pathPending && ag.remainingDistance <= ag.stoppingDistance && 
                (!ag.hasPath || ag.velocity.sqrMagnitude == 0f))
            {
                arrivedAgents.Add(ag);
                Debug.Log($"{ag.name} arrived at destination");
            }
        }
    }
}

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

    private void Start()
    {
        if (!cam)
            Debug.Log("No camera was found");
        if (agents.Count==0)
            Debug.Log("No participants were added");
    }

    void Update()
    {
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
        }
       
    }

    //Set destination foreach agent
    private void SetAgentsMovement(Vector3 finish)
    {
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
}

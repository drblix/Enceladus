using System.Collections.Generic;
using UnityEngine;

public class ObjectiveChecker : MonoBehaviour
{
    [SerializeField]
    private List<Objective> objectiveList = new List<Objective>();

    private bool objectivesComplete = false;
    public bool ObjectivesComplete { get { return objectivesComplete; } }

    private void Awake()
    {
        foreach (Objective obj in FindObjectsOfType<Objective>())
        {
            objectiveList.Add(obj);
        }
    }

    public void CheckObjectives()
    {
        if (!objectivesComplete)
        {
            foreach (Objective objective in objectiveList)
            {
                if (objective.CorrectPosition)
                {
                    objective.ConnectedMapNode.GetComponent<MapNode>().ObjectiveCompleted();
                    objectiveList.Remove(objective);

                    if (objectiveList.Count == 0) { objectivesComplete = true; }

                    break;
                }
            }
        }
    }
}

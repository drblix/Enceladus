using System.Collections.Generic;
using UnityEngine;

public class ObjectiveChecker : MonoBehaviour
{
    private static ObjectiveChecker _instance;
    public static ObjectiveChecker Instance { get { return _instance; } }

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

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
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

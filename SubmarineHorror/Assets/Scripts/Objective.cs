using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField]
    private GameObject connectedMapNode;
    public GameObject ConnectedMapNode { get { return connectedMapNode; } }
    
    [SerializeField] [Range(0f, 360f)]
    private float requiredAngle = 0f;
    public float RequiredAngle { get { return requiredAngle; } }

    private bool correctPosition = false;
    public bool CorrectPosition { get { return correctPosition; } }

    // Angle can be off by 2 degrees
    private const float ANGLE_OFFSET = 2f;

    private void OnTriggerStay(Collider other) 
    {
        if (other.CompareTag("SubObj"))
        {
            float subY = other.transform.eulerAngles.y;

            if (subY > (requiredAngle - ANGLE_OFFSET) && subY < (requiredAngle + ANGLE_OFFSET))
            {
                correctPosition = true;
            }
            else
            {
                correctPosition = false;
            }
        }
        else
        {
            correctPosition = false;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        correctPosition = false;
    }
}

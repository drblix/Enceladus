using UnityEngine;

public class SubmarineYSetter : MonoBehaviour
{
    private const float OPTIMAL_DIST = 3.46f;
    private const float MAX_HEIGHT = 5f;

    private int terrainMask;

    private void Awake() 
    {
        terrainMask = LayerMask.GetMask("MapTerrain");
    }

    private void FixedUpdate() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 15f, terrainMask))
        {
            if (hitInfo.collider)
            {
                float hDistance = hitInfo.distance;
                float newY = Mathf.Clamp(transform.position.y + (OPTIMAL_DIST - hDistance), OPTIMAL_DIST, MAX_HEIGHT); 

                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
    }
}

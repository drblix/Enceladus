using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapNode : MonoBehaviour
{    
    [SerializeField]
    private GameObject representingObjective;
    [SerializeField]
    private Color triggeredColor;

    [SerializeField]
    private Sprite completedIcon;

    private GameObject infoContainer;
    private Image nodeImg;

    private TextMeshProUGUI xInfo;
    private TextMeshProUGUI yInfo;
    private TextMeshProUGUI aInfo;

    private bool completed = false;

    private void Awake() 
    {
        xInfo = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        yInfo = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        aInfo = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();

        infoContainer = transform.GetChild(0).gameObject;
        nodeImg = transform.GetComponent<Image>();

        Objective obj = representingObjective.GetComponent<Objective>();
        xInfo.SetText("X_POS: " + obj.transform.position.x.ToString());
        yInfo.SetText("Y_POS: " + obj.transform.position.z.ToString());
        aInfo.SetText("A_POS: " + obj.RequiredAngle.ToString());
    }

    public void MouseEntered()
    {
        if (!completed)
        {
            nodeImg.color = triggeredColor;
        }
    }

    public void MouseExited()
    {
        nodeImg.color = Color.white;
    }

    public void ObjectiveCompleted()
    {
        nodeImg.sprite = completedIcon;
        completed = true;
    }
}

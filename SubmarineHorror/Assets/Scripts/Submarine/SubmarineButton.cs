using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineButton : MonoBehaviour, IUsable
{
    public enum ButtonType
    {
        Forward,
        Backward,
        Right,
        Left
    }

    [SerializeField] private ButtonType _buttonType;

    public void Use()
    {

    }
}

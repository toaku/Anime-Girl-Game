using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour, IPlayerInput
{
    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private TouchButton attackButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetAttackInput()
    {
        return attackButton.isInput;
    }

    public Vector2 GetMovementInput()
    {
        return joystick.GetDirection();
    }
}

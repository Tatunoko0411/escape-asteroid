using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public int EventID;
    public int PlayerManagerID;
    // Start is called before the first frame update
    public enum Event_ID
    {
        Dice = 1,
        Card,
        Change_Direction,
        Look,
        Send,
        Oxygen,
        Goal,
        Round_End,
        Turn_End = 99,
    }
}

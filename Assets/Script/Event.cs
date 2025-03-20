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
        Game_End,
        Clear,
        Exchange_End,
        Next_Round,
        Turn_End = 99,
        Quit_Player,
        Set_Target,
        Conect,
    }
}

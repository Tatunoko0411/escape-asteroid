using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public int id;

    public int hand;

    public int handCard_id;

    public int handCard_id_2;

    public int handCard_id_3;


    public Player(int id,int hand ,int handCard_1,int handCard_2,int handCard_3)
    {
        this.id = id;
        this.hand = hand;
        this.handCard_id = handCard_1;
        this.handCard_id_2 = handCard_2;
        this.handCard_id_3 = handCard_3;
    }



}

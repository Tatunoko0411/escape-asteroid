using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public int Dice1Max;
    public int Dice2Max;
    public int Dice1Min;
    public int Dice2Min;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DiceRoll(GameObject player)
    {
        int dice1,dice2,roll;
        dice1 = Random.Range(Dice1Min, Dice1Max);
        dice2 = Random.Range(Dice2Min, Dice2Max);
        roll = dice1 + dice2;

        Debug.Log($"ダイス１{dice1},ダイス２{dice2},合計:{roll}");



        GameObject.Find($"Player").GetComponent<Player>().MovePlayer(roll);
        


    }


}

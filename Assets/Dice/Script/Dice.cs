using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Animator Animation;
    [SerializeField] PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        Animation = GetComponent<Animator>();
    }

    public void isResult()
    {
        Animation.SetBool("isResult", true);
    }

    public void notisResult()
    {
        Animation.SetBool("isResult", false);
    }

    public void DiceAnimation(int dice1, int dice2 , int RollResult)
    {
        Animation.SetInteger("dice1", dice1);
        Animation.SetInteger("dice2", dice2);
        Animation.SetInteger("RollResult", RollResult);
        notisResult();
    }

    public void endDice()
    {
        playerManager.isDiceRoll = true;
    }
}

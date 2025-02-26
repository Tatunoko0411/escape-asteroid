using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int trout = 0;
    private int Move = 0;
    public int direction = 1;
    private int waitTime = 0;
    private Vector3 TargetPos;
    private int troutTier = 0;
    private bool isStart = false;
    private bool isGoal = false;
   [SerializeField] public  List<List<GameObject>> items =new List<List<GameObject>>()
   {
       new List<GameObject>(), new List<GameObject>(),new List<GameObject>(),new List<GameObject>()
   };
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの移動
        if (Move > 0)
        {
            if (waitTime <= 0)
            {

                if ((GameObject.Find("GameManager").GetComponent<GameManager>().trouts.Count <= trout + direction) ||
                    (0 > trout + direction))
                {
                    Move = 0;
                        //素材取得ターンに入る
                        ChoiceGetItem();
                    
                    return;
                }
                else
                {
                    TargetPos = (Vector3)(GameObject.Find("GameManager").GetComponent<GameManager>().trouts[trout + direction].transform.position);
                    TargetPos.y = 0.6f;
                }
                
                transform.position = Vector3.MoveTowards
                      (transform.position,
                      TargetPos,
                      0.01f + Time.deltaTime
                      );


                if (transform.position == TargetPos)
                {
                    Move--;
                    trout += direction;
                    waitTime = 30;
                    if (Move == 0)
                    {
                        //素材取得ターンに入る
                        ChoiceGetItem();
                    }
                }
            }
            else
            {
                waitTime--;
            }

        }


    }

    public void MovePlayer(int roll)
    {
        Move = roll;
    }

    public void ChoiceGetItem()
    {
        //ダイスロール系のボタンを無効にする
        GameObject gameObject = GameObject.Find("GameManager");
        gameObject.GetComponent<GameManager>().DiceRollButton.SetActive(false);
        gameObject.GetComponent<GameManager>().BackButton.SetActive(false);
        gameObject.GetComponent<GameManager>().GetItemUI.SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "based")
        {
            if (isStart)
            {
                isGoal = true;
                Debug.Log("ゴールしました");
            }
        }
        else
        {
            isStart = true;
            Debug.Log("スタートしました");
        }
    }
}

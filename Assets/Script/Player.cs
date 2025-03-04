using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int trout = 0;
    private int Move = 0;
    public List<int> probability = new List<int>()
    {0,0,0,0};
    public int direction = 1;
    private int waitTime = 0;
    private Vector3 TargetPos;
    private int troutTier = 0;
    private bool isStart = false;
    private bool isGoal = false;
    public int id;

    public int hand;

    public int handCard_id;

    public int handCard_id_2;

    public int handCard_id_3;
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().ChangeDirection();
                        //素材取得ターンに入る
                        ChoiceGetItem();
                    
                    return;
                }
                else
                {
                    TargetPos = (Vector3)(GameObject.Find("GameManager").GetComponent<GameManager>().trouts[trout + direction].transform.position);
                    TargetPos.y = 0.3f;
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

    public void SetHaveItem()
    {//
        GameObject[] Items = GameObject.FindGameObjectsWithTag("Item");
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        foreach (GameObject r in Items)
        {
            Destroy(r);
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                foreach (GameObject item in items[i])
                {
                    GameObject textObject = Instantiate(
                                             item,
                                             gameManager.transform.position,
                                             Quaternion.identity,
                                             gameManager.parentGameObject.transform
                                             );
                }
            }
        }
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
        else if (!isStart)
        {
            isStart = true;
            Debug.Log("スタートしました");
        }

        if (other.gameObject.tag == "Level0")
        {
            probability[0] = 70;
            probability[1] = 30;
            probability[2] = 0;
            probability[3] = 0;
        }

        if (other.gameObject.tag == "Level1")
        {
            probability[0] = 30;
            probability[1] = 60;
            probability[2] = 10;
            probability[3] = 0;
        }

        if (other.gameObject.tag == "Level2")
        {
            probability[0] = 0;
            probability[1] = 30;
            probability[2] = 60;
            probability[3] = 10;
        }

        if (other.gameObject.tag == "Level3")
        {
            probability[0] = 0;
            probability[1] = 0;
            probability[2] = 30;
            probability[3] = 70;
        }
    }
}

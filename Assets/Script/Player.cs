using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int trout = 0;
    private int Move = 0;
    public int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
       // MovePlayer(4);
    }

    // Update is called once per frame
    void Update()
    {

        if (Move > 0)
        {
            if (( GameObject.Find("GameManager").GetComponent<GameManager>().trouts.Count <= trout + direction)||
                (0 > trout + direction))
            {
                Move = 0;
                return;
            }
            transform.position = Vector3.MoveTowards
                  (transform.position,
                  GameObject.Find("GameManager").GetComponent<GameManager>().trouts[trout+direction].transform.position,
                  0.01f + Time.deltaTime
                  );


            if (transform.position == GameObject.Find("GameManager").GetComponent<GameManager>().trouts[trout + direction].transform.position)
            {
                Move--;
                trout += direction;
            }

        }


    }

    public void MovePlayer(int roll)
    {
        Move = roll;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameManager gameManager;
    [SerializeField] List<GameObject> IsExchangeUI;
    [SerializeField] List<GameObject> IsPlayingUI;
    [SerializeField] List<GameObject> IsDiceRollUI;
    [SerializeField] List<GameObject> IsGetItemUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isStart)
        {
            foreach (GameObject r in IsPlayingUI)
            {
                r.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject r in IsPlayingUI)
            {
                r.SetActive(false);
            }
        }
        if (!gameManager.isDiceRoll)
        {
            foreach (GameObject r in IsDiceRollUI)
            {
                r.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject r in IsDiceRollUI)
            {
                r.SetActive(false);
            }
        }
        if (gameManager.isGetItem)
        {
            foreach (GameObject r in IsGetItemUI)
            {
                r.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject r in IsGetItemUI)
            {
                r.SetActive(false);
            }
        }
        if (player.isGoal)
        {
            foreach (GameObject r in IsExchangeUI)
            {
                r.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject r in IsExchangeUI)
            {
                r.SetActive(false);
            }
        }
    }
}

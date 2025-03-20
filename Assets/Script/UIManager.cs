using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerManager PlayerManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] ExchangeManager exchangeManager;

    [SerializeField] List<GameObject> IsExchangeUI;
    [SerializeField] List<GameObject> IsSelectUI;
    [SerializeField] List<GameObject> IsPlayingUI;
    [SerializeField] List<GameObject> IsDiceRollUI;
    [SerializeField] List<GameObject> IsGetItemManagerUI;
    GameObject[] Cards;
    void Start()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isStart)
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
        if (PlayerManager.isDiceRollUI)
        {
            foreach (GameObject r in IsDiceRollUI)
            {
                r.SetActive(true);
            }

            foreach (GameObject r in Cards)
            {
                r.transform.position = new Vector3(r.transform.position.x, 150.0f, r.transform.position.z);
            }
        }
        else
        {
            foreach (GameObject r in IsDiceRollUI)
            {
                r.SetActive(false);
            }

            foreach (GameObject r in Cards)
            {
                r.transform.position =new Vector3(r.transform.position.x,-750.0f,r.transform.position.z);
            }
        }
        if (PlayerManager.isGetItemManager)
        {
            foreach (GameObject r in IsGetItemManagerUI)
            {
                r.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject r in IsGetItemManagerUI)
            {
                r.SetActive(false);
            }
        }
        if (PlayerManager.isSetExchange)
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
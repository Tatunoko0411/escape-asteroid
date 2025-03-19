using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConectManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Player1;
    [SerializeField] List<GameObject> Player2;
    [SerializeField] List<GameObject> Player3;
    [SerializeField] List<GameObject> Player4;
    public bool PlayerActive1;
    public bool PlayerActive2;
    public bool PlayerActive3;
    public bool PlayerActive4;
    bool startCountDown = false;

    int remainingTime = 1;

    [SerializeField] GameObject remainingText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerActive1)
        {
            foreach (GameObject player in Player1)
            {
                player.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject player in Player1)
            {
                player.SetActive(false);
            }
        }

        if (PlayerActive2)
        {
            foreach (GameObject player in Player2)
            {
                player.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject player in Player2)
            {
                player.SetActive(false);
            }
        }
        if (PlayerActive3)
        {
            foreach (GameObject player in Player3)
            {
                player.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject player in Player3)
            {
                player.SetActive(false);
            }
        }

        if (PlayerActive4)
        {
            foreach (GameObject player in Player4)
            {
                player.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject player in Player4)
            {
                player.SetActive(false);
            }
        }

        if(PlayerActive1 && !startCountDown)
        {
            StartCoroutine("CountDownTimer");
            startCountDown = true;
        }
    }
    public void SetConectedPlayer(int MainPlayerId)
    {
        for (int i = 0; i < MainPlayerId; i++)
        {
            switch(i)
            {
                case 0:
                    PlayerActive1 = true;
                    break;
                    case 1:
                    PlayerActive2 = true;
                    break;
                    case 2:
                    PlayerActive3 = true;
                    break;
                    case 3:
                    PlayerActive4 = true;
                    break;
            }
        }
    }

    IEnumerator CountDownTimer()
    {
        remainingText.SetActive(true);
        Text text = remainingText.GetComponent<Text>();
        Debug.Log("カウントダウン開始");
        while (true)
        {
            Debug.Log("待機前");
            //1秒待つ
            yield return new WaitForSeconds(1.0f);

            //カウントダウンしてタイマーのテキストに秒数を設定
            remainingTime--;
            text.text = remainingTime.ToString();
            Debug.Log("待機後");
            //0になったら終了
            if (remainingTime <= 0)
            {
                Initiate.Fade("TatunokoScene", Color.black, 1.0f);
                break;
            }
        }
        Debug.Log("カウントダウン終了");
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    //インスペクターから設定するか、初期化時にGetComponentして、Textへの参照を取得しておく。
    [SerializeField]
    Text txt;

    //一ループの長さ(秒数)。
    [Header("1ループの長さ(秒単位)")]
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float duration = 1.0f;
    float startDuration = 0.2f;


    [Header("ループ開始時の色")]
    [SerializeField]
    Color32 startColor = new Color32(255, 255, 255, 255);

    [Header("ループ終了時の色")]
    [SerializeField]
    Color32 endColor = new Color32(255, 255, 255, 40);

    public float _anglePerFrame = 0.1f;    // 1フレームに何度回すか[unit : deg]
    float _rot = 0.0f;

    //ゲームスタートの判定
    bool gameStart =  false;

    //移行先のシーン名
    private string fadeScene = "ConectScene";

    //フェード時の色
    Color32 fadeColor = new Color32(255, 255, 255, 255);

    [SerializeField] GameObject howTo;
    [SerializeField] GameObject howToButton;
    void Update()
    {
        //GameStartの点滅
        txt.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));

        _rot += _anglePerFrame;
        if (_rot >= 360.0f)
        {    // 0～360°の範囲におさめたい
            _rot -= 360.0f;
        }
        RenderSettings.skybox.SetFloat("_Rotation", _rot);    // 回す

        if (gameStart == true)
        {

            //ゲーム開始時の点滅
            txt.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / startDuration, 1.0f));
        
        }

    }

    public void gameStartSet()
    {
        gameStart = true;

        //シーン移行のフェード処理
        Initiate.Fade(fadeScene, fadeColor, 0.5f);
    }

    public void SetHoeTo()
    {
            howTo.SetActive(true);
         howToButton.SetActive(true);   
    }

    public void backHow()
    {
        howTo.SetActive(false);
        howToButton.SetActive(false);
    }
}

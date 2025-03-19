using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class resultManager : MonoBehaviour
{

    [SerializeField] GameObject resultTextPrefab;
    [SerializeField] GameObject parentGameObject;

    //移行先のシーン名
    private string fadeScene = "TitleScenes";

    //フェード時の色
    Color32 fadeColor = new Color32(255, 255, 255, 255);

    public float _anglePerFrame = 0.1f;    // 1フレームに何度回すか[unit : deg]
    float _rot = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //スカイボックスの回転
        _rot += _anglePerFrame;
        if (_rot >= 360.0f)
        {    // 0～360°の範囲におさめたい
            _rot -= 360.0f;
        }
        RenderSettings.skybox.SetFloat("_Rotation", _rot);    // 回す
    }

    public void titleSceneFade()
    {
        //シーン移行のフェード処理
        Initiate.Fade(fadeScene, fadeColor, 0.5f);
    }
}

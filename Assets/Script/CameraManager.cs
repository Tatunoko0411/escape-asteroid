using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public int activePlayer = 0;
    int select = 0;
    [SerializeField] List<GameObject> cameraList;
    [SerializeField] GameObject OverallCamera;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cameraList.Count; i++)
        {
            cameraList[i].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        { select--;
            if(select<0)
            {
                select = 3;
            }
            ChangeCamera(select);
        }
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            select++;
            if (select > 3)
            {
                select = 0;
            }
            ChangeCamera(select);
        }
        cameraList[activePlayer].SetActive(true);
    }

    private void Update()
    {
        for (int i = 0; i < cameraList.Count; i++)
        {
            cameraList[i].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            select--;
            if (select < 0)
            {
                select = 3;
            }
            ChangeCamera(select);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            select++;
            if (select > 3)
            {
                select = 0;
            }
            ChangeCamera(select);
        }
        cameraList[activePlayer].SetActive(true);
    }
    public void ChangeCamera(int player)
    {
        activePlayer = player;
        for (int i = 0; i < cameraList.Count; i++)
        {
            cameraList[i].SetActive(false);
        }
        cameraList[activePlayer].SetActive(true);
    }

    public void WholeCameraSwitching()
    {
        if (OverallCamera.activeSelf)
        {
            OverallCamera.SetActive(false);
            for (int i = 0; i < cameraList.Count; i++)
            {
                cameraList[i].SetActive(false);
            }
            cameraList[activePlayer].SetActive(true);
        }
        else
        {
            for (int i = 0; i < cameraList.Count; i++)
            {
                cameraList[i].SetActive(false);
            }
            OverallCamera.SetActive(true);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public RFIBManager rFIBManager;
    public GameParameter gameParameter;

    public int lightValue;

    // Start is called before the first frame update
    void Start()
    {
        lightValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SenceID();
        SetLight();
    }

    public void SenceID()
    {
        foreach (var dic in gameParameter.unitDic)
        {
            if (rFIBManager.tagSensing[dic.Key])
            {
                lightValue = dic.Value.GetComponent<Unit>().lightValue;
            }
        }
    }

    public void SetLight()
    {
        foreach (var dic in gameParameter.unitDic)
        {
            if (dic.Value.GetComponent<Unit>().lightValue <= lightValue)
                dic.Value.SetActive(true);
            else
                dic.Value.SetActive(false);
        }
    }
}

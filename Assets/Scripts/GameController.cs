using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public RFIBManager rFIBManager;
    public GameParameter gameParameter;

    public int lightValue;
    public Dictionary<string, bool> lightControl;

    public string lightMode;
    public Dictionary<string, bool> modeControl;

    // Start is called before the first frame update
    void Start()
    {
        lightControl = new Dictionary<string, bool>();
        foreach (var dic in gameParameter.unitDic)
        {
            lightControl.Add(dic.Key, false);
        }

        modeControl = new Dictionary<string, bool>();
        foreach (var dic in gameParameter.modeDic)
        {
            modeControl.Add(dic.Key, false);
        }
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

            if (rFIBManager.tagSensing[dic.Key] && !lightControl[dic.Key])
            {
                lightValue = dic.Value.GetComponent<Unit>().lightValue;
                lightControl[dic.Key] = true;
            }
            else if (!rFIBManager.tagSensing[dic.Key])
            {
                lightControl[dic.Key] = false;
            }
        }

        foreach (var dic in gameParameter.modeDic)
        {
            if (rFIBManager.tagSensing[dic.Key] && !modeControl[dic.Key])
            {
                lightMode = dic.Value.GetComponent<Mode>().modeName;
                modeControl[dic.Key] = true;
            }
            else if (!rFIBManager.tagSensing[dic.Key])
            {
                modeControl[dic.Key] = false;
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

        foreach (var dic in gameParameter.modeDic)
        {
            if (dic.Value.GetComponent<Mode>().modeName == lightMode)
            {
                dic.Value.GetComponent<SpriteRenderer>().color = dic.Value.GetComponent<Mode>().activeColor;
            }
            else
            {
                dic.Value.GetComponent<SpriteRenderer>().color = new Color32(60, 60, 60, 255);
            }
        }
    }
}

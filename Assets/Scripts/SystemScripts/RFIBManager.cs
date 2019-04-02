using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// 編號規則:
// 系統編號 此欄空白 方塊種類 編號+上下 方向

public class RFIBManager : MonoBehaviour
{
    RFIBricks_Cores RFIB;
    public GameParameter gameParameter;

    public Dictionary<string, bool> tagSensing;
    public Dictionary<string, int> tagMissTime;

    #region RFIB parameter
    readonly short[] EnableAntenna = { 1 };       // reader port
    readonly string ReaderIP = "192.168.1.94";           // 到時再說
    readonly double ReaderPower = 30, Sensitive = -70;   // 功率, 敏感度
    readonly bool Flag_ToConnectTheReade = true;        // false就不會連reader

    readonly bool showSysMesg = true;
    readonly bool showReceiveTag = true;
    readonly bool showDebugMesg = true;

    readonly string sysTagBased = "8940 0000";           // 允許的系統編號

    readonly int refreshTime = 600;                      // clear beffer
    readonly int disappearTime = 400;                    // id 消失多久才會的消失
    readonly int delayForReceivingTime = 200;            // 清空之後停多久才收id

    #endregion

    void Start()
    {
        #region Set RFIB Parameter
        RFIB = new RFIBricks_Cores(ReaderIP, ReaderPower, Sensitive, EnableAntenna, Flag_ToConnectTheReade);
        RFIB.setShowSysMesg(showSysMesg);
        RFIB.setShowReceiveTag(showReceiveTag);
        RFIB.setShowDebugMesg(showDebugMesg);

        RFIB.setSysTagBased(sysTagBased);
        RFIB.setAllowBlockType(RFIBParameter.AllowBlockType);

        RFIB.setRefreshTime(refreshTime);
        RFIB.setDisappearTime(disappearTime);
        RFIB.setDelayForReceivingTime(delayForReceivingTime);

        // 開始接收ID前要將地板配對
        BoardMapping();

        RFIB.startReceive();
        RFIB.startToBuild();
        RFIB.printNoiseIDs();

        #endregion

        tagSensing = new Dictionary<string, bool>();
        tagMissTime = new Dictionary<string, int>();

        foreach (var dic in gameParameter.unitDic)
        {
            tagSensing.Add(dic.Key, false);
            tagMissTime.Add(dic.Key, 0);
        }
        foreach (var dic in gameParameter.modeDic)
        {
            tagSensing.Add(dic.Key, false);
            tagMissTime.Add(dic.Key, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RFIB.statesUpdate();
        CountTagTime();
        SenseID();
        KeyPressed();
    }

    // 在開始接收ID前，這邊要將接收到的地板ID進行配對編號。
    private void BoardMapping()
    {
        //  [04]   | 0004 0104  ..   ..   ..   ..   ..  0704 0804
        //  [03]   | 0003 0103  ..   ..   ..   ..   ..  0703 0803
        //  [02]   | 0002 0102  ..   ..   ..   ..   ..  0702 0802
        //  [01]   | 0001 0101  ..   ..   ..   ..   ..  0701 0801
        //  [00]   | 0000 0100  ..   ..   ..   ..   ..  0700 0800
        //-------／-----------------------------------------------
        //   y ／x | [00] [01] [02] [03] [04] [05] [06] [07] [08] 

        for (int i = 0; i < RFIBParameter.blockNum; i++)
        {
            string pos = "0" + (i % RFIBParameter.stageCol).ToString() + "0" + (i / RFIBParameter.stageCol).ToString();
            RFIB.setBoardBlockMappingArray(i, pos);
        }
    }

    public void CountTagTime()
    {
        foreach (var dic in gameParameter.unitDic)
        {
            if (RFIB.IfContainTag(dic.Key))
            {
                tagSensing[dic.Key] = true;
                tagMissTime[dic.Key] = 0;
            }
            else
            {
                tagMissTime[dic.Key] += 1;
            }
        }
        foreach (var dic in gameParameter.modeDic)
        {
            if (RFIB.IfContainTag(dic.Key))
            {
                tagSensing[dic.Key] = true;
                tagMissTime[dic.Key] = 0;
            }
            else
            {
                tagMissTime[dic.Key] += 1;
            }
        }
    }

    public void SenseID()
    {
        foreach (var dic in gameParameter.unitDic)
        {
            if (tagMissTime[dic.Key] > 10)
            {
                tagSensing[dic.Key] = false;
            }
        }
        foreach (var dic in gameParameter.modeDic)
        {
            if (tagMissTime[dic.Key] > 10)
            {
                tagSensing[dic.Key] = false;
            }
        }
    }

    public void KeyPressed()
    {
        GetKey("1", "8940 0000 3333 0000 0001");
        GetKey("2", "8940 0000 3333 0001 0001");
        GetKey("3", "8940 0000 3333 0002 0001");
        GetKey("4", "8940 0000 3333 0003 0001");
        GetKey("5", "8940 0000 3333 0004 0001");
        GetKey("6", "8940 0000 3333 0005 0001");
        GetKey("7", "8940 0000 3333 0006 0001");
        GetKey("8", "8940 0000 3333 0007 0001");
        GetKey("9", "8940 0000 3333 0008 0001");

        GetKey("q", "8940 0000 3333 0011 0001");
        GetKey("w", "8940 0000 3333 0012 0001");
        GetKey("e", "8940 0000 3333 0013 0001");

        #region Information
        if (Input.GetKeyUp(";"))
        {
            string[] tags = RFIB.GetTags();
            for (int i = 0; i < tags.Length; i++)
                Debug.Log(tags[i]);
        }
        if (Input.GetKeyUp("."))
        {
            RFIB.printAllReceivedIDs();
            RFIB.printNoiseIDs();
        }

        #endregion
    }

    public void ChangeTestTag(string tag, bool TorF)
    {
        if (TorF)
            RFIB._Testing_AddHoldingTag(tag);
        else
            RFIB._Testing_RemoveHoldingTag(tag);
    }

    public void GetKey(string key, string tag)
    {
        if (Input.GetKeyDown(key))
            ChangeTestTag(tag, true);
        if (Input.GetKeyUp(key))
            ChangeTestTag(tag, false);
    }
}

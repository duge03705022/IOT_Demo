using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameParameter gameParameter;
    public string[] RFIDTag;
    public int lightValue;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < RFIDTag.Length; i++)
        {
            gameParameter.unitDic.Add(RFIDTag[i], gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

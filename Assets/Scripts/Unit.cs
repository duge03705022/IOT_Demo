using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameParameter gameParameter;
    public string RFIDTag;
    public int lightValue;

    // Start is called before the first frame update
    void Start()
    {
        gameParameter.unitDic.Add(RFIDTag, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

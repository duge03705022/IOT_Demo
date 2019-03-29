using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParameter : MonoBehaviour
{
    public Dictionary<string, GameObject> unitDic;
    public Dictionary<string, GameObject> modeDic;

    // Start is called before the first frame update
    void Start()
    {
        unitDic = new Dictionary<string, GameObject>();
        modeDic = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

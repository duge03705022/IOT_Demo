using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Binding : MonoBehaviour
{
    public GameObject[] tagText;
    public GameObject[] Unit;
    public Text newTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NewTag("1", "0004");
        NewTag("2", "0104");
        NewTag("3", "0204");
        NewTag("4", "0304");
        NewTag("5", "0404");
        NewTag("6", "0504");
        NewTag("7", "0604");
        NewTag("8", "0704");
        NewTag("9", "0804");
        NewTag("0", "");

        BindingText("z", 0);
        BindingText("x", 1);
        BindingText("c", 2);
        BindingText("v", 3);
        BindingText("b", 4);
        BindingText("n", 5);
        BindingText("m", 6);
        BindingText(",", 7);
        BindingText(".", 8);
    }

    public void NewTag(string key, string tag)
    {
        if (Input.GetKeyDown(key))
        {
            newTag.text = "New tag : " + tag;
        }
    }

    public void BindingText(string key, int id)
    {
        if (Input.GetKey(key))
        {
            tagText[id].SetActive(true);
            Unit[id].GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        }
        else
        {
            Unit[id].GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
            tagText[id].SetActive(false);
        }
    }
}

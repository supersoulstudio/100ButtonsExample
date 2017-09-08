using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    private class UIData
    {
        public string Name;
        public GameObject Obj;
    }

    [SerializeField]
    private UIData[] Data;

    private void Awake()
    {
        for (int i = 0; i < Data.Length; i++)
        {
            Data[i].Obj.SetActive(false);
        }
    }

    public GameObject Show(string name)
    {
        GameObject obj = null;

        for (int i = 0; i < Data.Length; i++)
        {
            if (Data[i].Name == name)
            {
                Data[i].Obj.SetActive(true);
                obj = Data[i].Obj;
            }
            else
            {
                Data[i].Obj.SetActive(false);
            }
        }

        return obj;
    }

    public void Show(string name, string message)
    {
        GameObject obj = Show(name);
        Transform t = obj.transform.Find("Text");
        t.GetComponent<Text>().text = message;
    }
}

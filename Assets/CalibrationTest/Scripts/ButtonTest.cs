using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ButtonTest : MonoBehaviour
{
    public GameObject prefab;

    ParticleSystem[] parts;
    Renderer[] rends;

    int[] buttons = new int[100];
    int buttonInd;

    private bool Calibrating = false;

    void Start()
    {
        float height = Camera.main.orthographicSize / 2f;
        float width = height * Camera.main.aspect;
        parts = new ParticleSystem[100];
        rends = new Renderer[100];
        for (int i = 0; i < 51; i++)
        {
            int y = i / 7;
            int x = i % 7;
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.position = new Vector3(x * 1.1f - width * 2f + 1f, -y * 1.1f + height + 1f, 0f);
            parts[i] = obj.GetComponent<ParticleSystem>();
            rends[i] = obj.GetComponent<Renderer>();
        }
        for (int i = 0; i < 49; i++)
        {
            int y = i / 7;
            int x = i % 7 + 7;
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.position = new Vector3(x * 1.1f - width * 2f + 1f, -y * 1.1f + height + 1f, 0f);
            parts[51 + i] = obj.GetComponent<ParticleSystem>();
            rends[51 + i] = obj.GetComponent<Renderer>();
        }
        /*if (File.Exists(Application.dataPath + "/cal.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(int[]));
            using (StreamReader stream = new StreamReader(Application.dataPath + "/cal.xml"))
            {
                buttons = (int[])serializer.Deserialize(stream);
                buttonInd = buttons.Length;
            }
        }*/
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonInd = 0;
            if (Calibrating)
            {
                Debug.Log("Calibration Restarting");
            }
            else
            {
                Debug.Log("Calibration Starting");
                Calibrating = true;
            }
        }
        
        if (Calibrating)
        {
            for (int d = 0; d < 2; d++)
            {
                for (int i = 0; i < 54; i++)
                {
                    if (SerialController.Instance.WasPressed(d, i))
                    {
                        Debug.Log(buttonInd + " " + d + " " + i);
                        buttons[buttonInd] = i;
                        buttonInd++;
                        if (buttonInd == buttons.Length)
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(int[]));
                            using (StreamWriter stream = new StreamWriter(Application.dataPath + "/cal.xml"))
                            {
                                serializer.Serialize(stream, buttons);
                            }
                            Debug.Log("Calibration Done");
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 49; i++)
            {
                if (SerialController.Instance.IsPressed(0, i))
                {
                    //parts[i].Emit(30);
                    rends[i].material.color = Color.red;
                }
                else
                {
                    rends[i].material.color = Color.white;
                }

                if (SerialController.Instance.WasPressed(0, i))
                {
                    Debug.Log("0 " + i);
                }
            }
            for (int i = 0; i < 51; i++)
            {
                if (SerialController.Instance.IsPressed(1, i))
                {
                    //parts[i].Emit(30);
                    rends[i + 49].material.color = Color.red;
                }
                else
                {
                    rends[i + 49].material.color = Color.white;
                }

                if (SerialController.Instance.WasPressed(1, i))
                {
                    Debug.Log("1 " + i);
                }
            }


        }
    }
}
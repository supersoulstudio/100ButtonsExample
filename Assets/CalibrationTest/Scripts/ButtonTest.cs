using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ButtonTest : MonoBehaviour
{
    public Text DeviceLabel;
    public Toggle CalibToggle;
    public GameObject prefab;
    public int numButtons = 51;

    ParticleSystem[] parts;
    Renderer[] rends;

    List<int> buttons = new List<int>();
    int buttonInd;
    int deviceInd;

    private bool Calibrating = true;

    void Start()
    {
        float height = Camera.main.orthographicSize / 2f;
        float width = height * Camera.main.aspect;
        parts = new ParticleSystem[numButtons];
        rends = new Renderer[numButtons];
        for (int i = 0; i < numButtons; i++)
        {
            int y = i / 7;
            int x = i % 7;
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.position = new Vector3(x * 1.1f - width * 2f + 1f, -y * 1.1f + height + 1f, 0f);
            parts[i] = obj.GetComponent<ParticleSystem>();
            rends[i] = obj.GetComponent<Renderer>();
        }
        CalibToggle.isOn = Calibrating;
        CalibToggle.onValueChanged.AddListener(ToggleCalib);
        UpdateDevice();
    }

    public void ToggleCalib(bool value)
    {
        Calibrating = value;

        if (Calibrating)
        {
            buttonInd = 0;
            buttons.Clear();

            for (int i = 0; i < numButtons; i++)
                rends[i].material.color = Color.red;
        }
    }

    void UpdateDevice()
    {
        DeviceLabel.text = (deviceInd + 1).ToString();
        if (Calibrating)
        {
            buttonInd = 0;
            buttons.Clear();

            for (int i = 0; i < numButtons; i++)
                rends[i].material.color = Color.red;
        }
        else
        {
            buttons.Clear();
            buttons.AddRange(SerialController.Instance.GetCalib(deviceInd));
        }
    }

    public void PrevDevice()
    {
        deviceInd--;
        if (deviceInd < 0)
            deviceInd = 0;
        UpdateDevice();
    }

    public void NextDevice()
    {
        deviceInd++;
        if (deviceInd > SerialController.Instance.readers.Count - 1)
            deviceInd = SerialController.Instance.readers.Count - 1;
        UpdateDevice();
    }

    public void SaveCalib()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(int[]));
        using (StreamWriter stream = new StreamWriter(Application.dataPath + "/cal" + deviceInd + ".xml"))
        {
            serializer.Serialize(stream, buttons.ToArray());
        }
        SerialController.Instance.SetCalib(deviceInd, buttons.ToArray());
    }

    void Update()
    {
        if (Calibrating)
        {
            if (buttonInd < numButtons)
            {
                for (int i = 0; i < numButtons; i++)
                {
                    if (SerialController.Instance.WasPressedRaw(deviceInd, i))
                    {
                        Debug.Log(buttonInd + " " + deviceInd + " " + i);
                        while (buttons.Count <= buttonInd)
                            buttons.Add(-1);
                        buttons[buttonInd] = i;
                        rends[buttonInd].material.color = Color.white;
                        buttonInd++;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < numButtons; i++)
            {
                if (SerialController.Instance.IsPressed(deviceInd, i))
                {
                    //parts[i].Emit(30);
                    rends[i].material.color = Color.green;
                }
                else
                {
                    rends[i].material.color = Color.white;
                }

                if (SerialController.Instance.WasPressed(deviceInd, i))
                {
                    Debug.Log(deviceInd + " " + i);
                }
            }
        }
    }
}
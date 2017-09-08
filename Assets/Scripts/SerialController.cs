using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SerialController : MonoBehaviour
{
    public static SerialController Instance;
    int[][] buttons;

    public List<ReadSerial> readers = new List<ReadSerial>();
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        using (StreamReader sr = new StreamReader(Application.dataPath + "\\config.txt"))
        {
            string portName = sr.ReadLine();
            while (!string.IsNullOrEmpty(portName))
            {
                readers.Add(new ReadSerial(portName));
                portName = sr.ReadLine();
            }
        }

        XmlSerializer serializer = new XmlSerializer(typeof(int[]));
        buttons = new int[2][];
        using (StreamReader stream = new StreamReader(Application.dataPath + "/cal0.xml"))
        {
            buttons[0] = (int[])serializer.Deserialize(stream);
        }
        using (StreamReader stream = new StreamReader(Application.dataPath + "/cal1.xml"))
        {
            buttons[1] = (int[])serializer.Deserialize(stream);
        }
    }

    void Start()
    {
        for (int i = 0; i < readers.Count; i++)
        {
            readers[i].Start();
        }
    }

    void Update()
    {
        for (int i = 0; i < readers.Count; i++)
        {
            readers[i].Update();
        }
    }

    public bool IsPressed(int device, int button)
    {
        try
        {
            return readers[device].IsPressed(buttons[device][button]);
        }
        catch
        {
            return false;
        }
    }

    public bool WasPressed(int device, int button)
    {
        if (button >= buttons[device].Length)
        {
            return false;
        }
        return readers[device].WasPressed(buttons[device][button]);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < readers.Count; i++)
        {
            readers[i].Close();
        }
    }
}
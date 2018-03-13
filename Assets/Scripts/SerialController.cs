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
        buttons = new int[readers.Count][];
        for (int i = 0; i < readers.Count; i++)
        {
            if (File.Exists(Application.dataPath + "/cal" + i + ".xml"))
            {
                using (StreamReader stream = new StreamReader(Application.dataPath + "/cal" + i + ".xml"))
                {
                    buttons[i] = (int[])serializer.Deserialize(stream);
                }
            }
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

    public int[] GetCalib(int device)
    {
        if (device >= 0 && device < buttons.Length && buttons[device] != null)
            return buttons[device];
        else
            return new int[0];
    }

    public void SetCalib(int device, int[] calib)
    {
        if (device < buttons.Length)
            buttons[device] = calib;
    }

    public bool IsPressedRaw(int device, int button)
    {
        try
        {
            return readers[device].IsPressed(button);
        }
        catch
        {
            return false;
        }
    }

    public bool WasPressedRaw(int device, int button)
    {
        try
        {
            return readers[device].WasPressed(button);
        }
        catch
        {
            return false;
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
        try
        {
            return readers[device].WasPressed(buttons[device][button]);
        }
        catch
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < readers.Count; i++)
        {
            readers[i].Close();
        }
    }
}
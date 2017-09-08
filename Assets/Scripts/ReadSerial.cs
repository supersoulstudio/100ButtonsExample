using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

[System.Serializable]
public class ReadSerial
{
    public string portName = "COM5";
    SerialPort port;
    List<byte> input = new List<byte>();
    byte[] states = new byte[8];
    byte[] oldStates = new byte[8];

    public ReadSerial(string name)
    {
        portName = name;
    }

    public void Start()
    {
        port = new SerialPort(portName, 9600);
        try
        {
            port.Open();
            port.ReadTimeout = 1;
            Debug.Log("open");
        }
        catch
        {
            Debug.Log("failed");
        }
    }

    public void Update()
    {
        for (int i = 0; i < states.Length; i++)
        {
            oldStates[i] = states[i];
        }
        if (port.IsOpen)
        {
            try
            {
                while (true)
                {
                    byte read = (byte)port.ReadByte();
                    if ((read & 1) == 1)
                    {
                        if (input.Count >= states.Length)
                        {
                            for (int i = 0; i < states.Length; i++)
                            {
                                states[i] = input[input.Count - 8 + i];
                            }
                        }
                        input.Clear();
                    }
                    else
                    {
                        input.Add(read);
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
        else
        {
        }
    }

    public bool IsPressed(int i)
    {
        return (states[i / 7] & (1 << (i % 7 + 1))) > 0;
    }

    public bool WasPressed(int i)
    {
        return ((states[i / 7] & (1 << (i % 7 + 1))) > 0)
            && !((oldStates[i / 7] & (1 << (i % 7 + 1))) > 0);
    }

    public void Close()
    {
        port.Close();
    }
}
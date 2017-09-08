using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTest : MonoBehaviour
{
    public GameObject prefab;
    string[] keys = {   "1234567890-=",
                        "qwertyuiop[]",
                        "asdfghjkl;'",
                        "zxcvbnm,./"};

    ParticleSystem[][] parts;
    
    void Start()
    {
        float height = Camera.main.orthographicSize / 2f;
        float width = height * Camera.main.aspect;
        parts = new ParticleSystem[keys.Length][];
        for (int i = 0; i < keys.Length; i++)
        {
            parts[i] = new ParticleSystem[keys[i].Length];
            for (int j = 0; j < keys[i].Length; j++)
            {
                GameObject obj = GameObject.Instantiate(prefab);
                obj.transform.position = new Vector3(j + i * 0.5f - width, -i + height, 0f);
                parts[i][j] = obj.GetComponent<ParticleSystem>();
            }
        }
    }
    
    void Update()
    {
        string input = Input.inputString;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < keys.Length; j++)
            {
                int ind = keys[j].IndexOf(input[i]);
                if (ind != -1)
                {
                    parts[j][ind].Emit(30);
                    break;
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject Prefab;

    public int Width = 14;
    public int Height = 7;
    public float ButtonRadius = 0.3f;
    public Color ButtonOff;

    [System.NonSerialized]
    public ButtonData[,] Buttons;

    public enum ButtonColor { Red, Green, Blue, Yellow, Black, White }

    public class ButtonData
    {
        public ButtonColor BColor;
        public Color MColor;
        public GameObject Obj;
        private Material Material;
        public bool Active = true;
        private Animator Anim;

        public ButtonData(ButtonColor BColor)
        {
            this.BColor = BColor;
            switch (BColor)
            {
                case ButtonColor.Red:
                    this.MColor = new Color(1, 0, 0);
                    break;
                case ButtonColor.Green:
                    this.MColor = new Color(0, 1, 0);
                    break;
                case ButtonColor.Blue:
                    this.MColor = new Color(0, 0, 1);
                    break;
                case ButtonColor.Yellow:
                    this.MColor = new Color(1, 1, 0);
                    break;
                case ButtonColor.Black:
                    this.MColor = new Color(0, 0, 0);
                    break;
                case ButtonColor.White:
                    this.MColor = new Color(1, 1, 1);
                    break;
                default:
                    break;
            }            
        }

        public void CreateObj(int x, int y, float Radius, GameObject Prefab)
        {
            Obj = GameObject.Instantiate(Prefab, new Vector3(x * Radius, 0, y * Radius), Quaternion.identity);
            Obj.name = x.ToString() + "_" + y.ToString();
            Material = Obj.transform.Find("Button").transform.Find("Ring").GetComponent<Renderer>().material;
            Material.color = this.MColor;
            Obj.transform.Find("Button").transform.Find("Face").GetComponent<Renderer>().material = Material;
            Anim = Obj.GetComponent<Animator>();
        }

        public void SetActive(bool Active, bool HideInactive, bool ShowColor)
        {
            this.Active = Active;
            if (HideInactive && !Active)
            {
                Obj.SetActive(false);
            }
            else
            {
                Obj.SetActive(true);
                if (Active && HideInactive && ShowColor || Active && !HideInactive)
                {
                    Material.color = MColor;
                }
                else
                {
                    Material.color = SimpleGame.Board.ButtonOff;
                }
            }
        }

        public void Press()
        {
            Anim.SetTrigger("Pressed");
        }
    }

    // Use this for initialization
    void Awake()
    {
        CreateButtonData();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Buttons[x, y].CreateObj(x, y, ButtonRadius, Prefab);
            }
        }

        //TODO for testing
        //Width = 10;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowAll(false, false, true);
                ShowColor(ButtonColor.Red, false, true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShowAll(false, false, true);
                ShowColor(ButtonColor.Green, false, true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ShowAll(false, false, false);
                ShowColor(ButtonColor.White, false, false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ShowAll(false, false, false);
                ShowColor(ButtonColor.Blue, false, false);
                ShowColor(ButtonColor.Black, false, false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ShowAll(false, true, true);
                ShowColor(ButtonColor.Red, true, true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ShowAll(false, true, true);
                ShowColor(ButtonColor.Green, true, true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ShowAll(false, true, false);
                ShowColor(ButtonColor.Red, true, false);
                ShowColor(ButtonColor.White, true, false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                ShowAll(false, false, false);
                ShowRandom(3, false, false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                ShowAll(false, true, true);
                ShowRandom(3, true, true);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                ShowAll(true, true, true);
            }
        }
    }

    public void ShowAll(bool Active, bool HideInactive, bool ShowColor)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Buttons[x, y].SetActive(Active, HideInactive, ShowColor);
            }
        }
    }

    public void ShowRandom(int Count, bool HideInactive, bool ShowColor)
    {
        int total = 0;
        int randx, randy;

        do
        {
            randx = Random.Range(0, Width);
            randy = Random.Range(0, Height);
            if (!Buttons[randx, randy].Active)
            {
                Buttons[randx, randy].SetActive(true, HideInactive, ShowColor);
                total++;
            }
        } while (total != Count);
    }

    public void ShowColor(ButtonColor color, bool HideInactive, bool ShowColor)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Buttons[x, y].BColor == color)
                {
                    Buttons[x, y].SetActive(true, HideInactive, ShowColor);
                }
            }
        }
    }

    public int CountActive()
    {
        int count = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Buttons[x, y].Active)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private void CreateButtonData()
    {
        Buttons = new ButtonData[Width, Height];

        Buttons[0, 0] = new ButtonData(ButtonColor.Green);
        Buttons[1, 0] = new ButtonData(ButtonColor.Yellow);
        Buttons[2, 0] = new ButtonData(ButtonColor.Yellow);
        Buttons[3, 0] = new ButtonData(ButtonColor.Green);
        Buttons[4, 0] = new ButtonData(ButtonColor.Red);
        Buttons[5, 0] = new ButtonData(ButtonColor.Yellow);
        Buttons[6, 0] = new ButtonData(ButtonColor.Blue);
        Buttons[7, 0] = new ButtonData(ButtonColor.Blue);
        Buttons[8, 0] = new ButtonData(ButtonColor.White);
        Buttons[9, 0] = new ButtonData(ButtonColor.Yellow);
        Buttons[10, 0] = new ButtonData(ButtonColor.Red);
        Buttons[11, 0] = new ButtonData(ButtonColor.Green);
        Buttons[12, 0] = new ButtonData(ButtonColor.Blue);
        Buttons[13, 0] = new ButtonData(ButtonColor.Green);

        Buttons[0, 1] = new ButtonData(ButtonColor.Red);
        Buttons[1, 1] = new ButtonData(ButtonColor.Black);
        Buttons[2, 1] = new ButtonData(ButtonColor.White);
        Buttons[3, 1] = new ButtonData(ButtonColor.Blue);
        Buttons[4, 1] = new ButtonData(ButtonColor.White);
        Buttons[5, 1] = new ButtonData(ButtonColor.Green);
        Buttons[6, 1] = new ButtonData(ButtonColor.Yellow);
        Buttons[7, 1] = new ButtonData(ButtonColor.Green);
        Buttons[8, 1] = new ButtonData(ButtonColor.Green);
        Buttons[9, 1] = new ButtonData(ButtonColor.White);
        Buttons[10, 1] = new ButtonData(ButtonColor.Green);
        Buttons[11, 1] = new ButtonData(ButtonColor.Blue);
        Buttons[12, 1] = new ButtonData(ButtonColor.Red);
        Buttons[13, 1] = new ButtonData(ButtonColor.Blue);

        Buttons[0, 2] = new ButtonData(ButtonColor.Blue);
        Buttons[1, 2] = new ButtonData(ButtonColor.White);
        Buttons[2, 2] = new ButtonData(ButtonColor.White);
        Buttons[3, 2] = new ButtonData(ButtonColor.Green);
        Buttons[4, 2] = new ButtonData(ButtonColor.Blue);
        Buttons[5, 2] = new ButtonData(ButtonColor.Green);
        Buttons[6, 2] = new ButtonData(ButtonColor.Green);
        Buttons[7, 2] = new ButtonData(ButtonColor.Green);
        Buttons[8, 2] = new ButtonData(ButtonColor.Red);
        Buttons[9, 2] = new ButtonData(ButtonColor.Green);
        Buttons[10, 2] = new ButtonData(ButtonColor.Green);
        Buttons[11, 2] = new ButtonData(ButtonColor.Blue);
        Buttons[12, 2] = new ButtonData(ButtonColor.Blue);
        Buttons[13, 2] = new ButtonData(ButtonColor.Red);

        Buttons[0, 3] = new ButtonData(ButtonColor.Green);
        Buttons[1, 3] = new ButtonData(ButtonColor.Green);
        Buttons[2, 3] = new ButtonData(ButtonColor.White);
        Buttons[3, 3] = new ButtonData(ButtonColor.Yellow);
        Buttons[4, 3] = new ButtonData(ButtonColor.Yellow);
        Buttons[5, 3] = new ButtonData(ButtonColor.Blue);
        Buttons[6, 3] = new ButtonData(ButtonColor.White);
        Buttons[7, 3] = new ButtonData(ButtonColor.Yellow);
        Buttons[8, 3] = new ButtonData(ButtonColor.White);
        Buttons[9, 3] = new ButtonData(ButtonColor.Blue);
        Buttons[10, 3] = new ButtonData(ButtonColor.Yellow);
        Buttons[11, 3] = new ButtonData(ButtonColor.Blue);
        Buttons[12, 3] = new ButtonData(ButtonColor.Green);
        Buttons[13, 3] = new ButtonData(ButtonColor.Red);

        Buttons[0, 4] = new ButtonData(ButtonColor.White);
        Buttons[1, 4] = new ButtonData(ButtonColor.Blue);
        Buttons[2, 4] = new ButtonData(ButtonColor.Green);
        Buttons[3, 4] = new ButtonData(ButtonColor.Yellow);
        Buttons[4, 4] = new ButtonData(ButtonColor.Blue);
        Buttons[5, 4] = new ButtonData(ButtonColor.Green);
        Buttons[6, 4] = new ButtonData(ButtonColor.Green);
        Buttons[7, 4] = new ButtonData(ButtonColor.Black);
        Buttons[8, 4] = new ButtonData(ButtonColor.Blue);
        Buttons[9, 4] = new ButtonData(ButtonColor.Red);
        Buttons[10, 4] = new ButtonData(ButtonColor.Red);
        Buttons[11, 4] = new ButtonData(ButtonColor.Yellow);
        Buttons[12, 4] = new ButtonData(ButtonColor.Green);
        Buttons[13, 4] = new ButtonData(ButtonColor.Yellow);

        Buttons[0, 5] = new ButtonData(ButtonColor.White);
        Buttons[1, 5] = new ButtonData(ButtonColor.Green);
        Buttons[2, 5] = new ButtonData(ButtonColor.Blue);
        Buttons[3, 5] = new ButtonData(ButtonColor.Red);
        Buttons[4, 5] = new ButtonData(ButtonColor.Yellow);
        Buttons[5, 5] = new ButtonData(ButtonColor.Blue);
        Buttons[6, 5] = new ButtonData(ButtonColor.Green);
        Buttons[7, 5] = new ButtonData(ButtonColor.Yellow);
        Buttons[8, 5] = new ButtonData(ButtonColor.Red);
        Buttons[9, 5] = new ButtonData(ButtonColor.Yellow);
        Buttons[10, 5] = new ButtonData(ButtonColor.Green);
        Buttons[11, 5] = new ButtonData(ButtonColor.Blue);
        Buttons[12, 5] = new ButtonData(ButtonColor.White);
        Buttons[13, 5] = new ButtonData(ButtonColor.Red);

        Buttons[0, 6] = new ButtonData(ButtonColor.Red);
        Buttons[1, 6] = new ButtonData(ButtonColor.Blue);
        Buttons[2, 6] = new ButtonData(ButtonColor.White);
        Buttons[3, 6] = new ButtonData(ButtonColor.Red);
        Buttons[4, 6] = new ButtonData(ButtonColor.Green);
        Buttons[5, 6] = new ButtonData(ButtonColor.White);
        Buttons[6, 6] = new ButtonData(ButtonColor.Red);
        Buttons[7, 6] = new ButtonData(ButtonColor.Blue);
        Buttons[8, 6] = new ButtonData(ButtonColor.Blue);
        Buttons[9, 6] = new ButtonData(ButtonColor.Green);
        Buttons[10, 6] = new ButtonData(ButtonColor.White);
        Buttons[11, 6] = new ButtonData(ButtonColor.Blue);
        Buttons[12, 6] = new ButtonData(ButtonColor.Blue);
        Buttons[13, 6] = new ButtonData(ButtonColor.Green);
    }
}

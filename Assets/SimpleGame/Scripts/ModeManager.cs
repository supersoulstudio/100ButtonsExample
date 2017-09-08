using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    public Text Timer;
    public Text IntroTimer;
    public Text ResultsMessage;

    public KeyCode[,] Keys;

    protected ModeBase Mode;

    public bool IsRunning = false;

    // Use this for initialization
    void Start ()
    {
        CreateKeys();

        Mode = new ModeRandom();        
    }
    
    public void Intro()
    {
        IsRunning = true;

        StartCoroutine(IntroDo());
    }

    private IEnumerator IntroDo()
    {
        Mode.Board = SimpleGame.Board;
        Mode.Setup(this);

        SimpleGame.UI.Show("Intro");

        IntroTimer.text = "Ready";
        yield return new WaitForSeconds(1f);

        IntroTimer.text = "Set";
        yield return new WaitForSeconds(1f);

        IntroTimer.text = "Go!";
        yield return new WaitForSeconds(0.5f);

        SimpleGame.UI.Show("HUD");
        Mode.Start();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!IsRunning || !Mode.IsRunning)
        {
            return;
        }

        //Keyboard input for testing
        int posx = 0;
        int posy = 0;
        for (int x = 0; x < Keys.GetLength(0); x++)
        {
            for (int y = 0; y < Keys.GetLength(1); y++)
            {
                if (Input.GetKeyDown(Keys[x, y]))
                {
                    posx = x;
                    posy = y;

                    if (Input.GetKey(KeyCode.BackQuote))
                    {
                        posx += 10;
                    }
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        posy += 4;
                    }
                    PressCheck(posx, posy);
                }
            }
        }

        //Test board
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 51; j++)
            {
                if (SerialController.Instance.WasPressed(i, j))
                {
                    posx = (j % 7) + (i * 7);
                    posy = (j / 7);
                    PressCheck(posx, posy);
                }
            }
        }

        Mode.Update();
    }

    private void PressCheck(int x, int y)
    {
        //TODO don't hardcode
        if (y < 7)
        {
            Debug.Log(x.ToString() + "," + y.ToString());
            StartCoroutine(Mode.PressCheck(x, y));
        }
    }

    public void Finish()
    {
        SimpleGame.UI.Show("Results");

        StartCoroutine(FinishDo());
    }

    private IEnumerator FinishDo()
    {
        yield return StartCoroutine(Mode.Finish());

        yield return new WaitForSeconds(1);

        IsRunning = false;
    }

    private void CreateKeys()
    {
        Keys = new KeyCode[10, 4];

        Keys[0, 0] = KeyCode.Z;
        Keys[1, 0] = KeyCode.X;
        Keys[2, 0] = KeyCode.C;
        Keys[3, 0] = KeyCode.V;
        Keys[4, 0] = KeyCode.B;
        Keys[5, 0] = KeyCode.N;
        Keys[6, 0] = KeyCode.M;
        Keys[7, 0] = KeyCode.Comma;
        Keys[8, 0] = KeyCode.Period;
        Keys[9, 0] = KeyCode.Slash;

        Keys[0, 1] = KeyCode.A;
        Keys[1, 1] = KeyCode.S;
        Keys[2, 1] = KeyCode.D;
        Keys[3, 1] = KeyCode.F;
        Keys[4, 1] = KeyCode.G;
        Keys[5, 1] = KeyCode.H;
        Keys[6, 1] = KeyCode.J;
        Keys[7, 1] = KeyCode.K;
        Keys[8, 1] = KeyCode.L;
        Keys[9, 1] = KeyCode.Semicolon;

        Keys[0, 2] = KeyCode.Q;
        Keys[1, 2] = KeyCode.W;
        Keys[2, 2] = KeyCode.E;
        Keys[3, 2] = KeyCode.R;
        Keys[4, 2] = KeyCode.T;
        Keys[5, 2] = KeyCode.Y;
        Keys[6, 2] = KeyCode.U;
        Keys[7, 2] = KeyCode.I;
        Keys[8, 2] = KeyCode.O;
        Keys[9, 2] = KeyCode.P;

        Keys[0, 3] = KeyCode.Alpha1;
        Keys[1, 3] = KeyCode.Alpha2;
        Keys[2, 3] = KeyCode.Alpha3;
        Keys[3, 3] = KeyCode.Alpha4;
        Keys[4, 3] = KeyCode.Alpha5;
        Keys[5, 3] = KeyCode.Alpha6;
        Keys[6, 3] = KeyCode.Alpha7;
        Keys[7, 3] = KeyCode.Alpha8;
        Keys[8, 3] = KeyCode.Alpha9;
        Keys[9, 3] = KeyCode.Alpha0;

    }

}

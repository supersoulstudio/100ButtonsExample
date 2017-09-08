using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeRandom : ModeBase
{
    private float StartTime;

    protected ModeManager Manager;

    public override void Setup(ModeManager Manager)
    {
        this.Manager = Manager;

        Board.ShowAll(false, false, true);
    }

    public override void Start()
    {
        IsRunning = true;
        StartTime = Time.time;

        Board.ShowRandom(3, false, true);
    }

    public override void Update()
    {
        float timeelapsed = Time.time - StartTime;
        float timeleft = 30 - timeelapsed;
        if (timeleft <= 0)
        {
            timeleft = 0;

            Manager.Finish();
        }

        Manager.Timer.text = String.Format("{0:0.0}", timeleft);
    }

    public override IEnumerator PressCheck(int x, int y)
    {
        if (Board.Buttons[x, y].Active)
        {
            Board.Buttons[x, y].Press();
            yield return new WaitForSeconds(0.2f);
            Board.Buttons[x, y].SetActive(false, false, true);
        }

        int count = Board.CountActive();

        if (count == 0)
        {
            Manager.Finish();
        }
    }

    public override IEnumerator Finish()
    {
        IsRunning = false;

        int count = Board.CountActive();

        if (count == 0)
        {
            Manager.ResultsMessage.text = "Good Job!";
        }            
        else
        {
            Manager.ResultsMessage.text = "Better Luck Next Time";
        }

        yield return new WaitForSeconds(3f);

        Manager.ResultsMessage.text = "";
    }
}

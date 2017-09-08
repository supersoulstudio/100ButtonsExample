using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModeBase
{
    //TODO remove
    public BoardManager Board;

    public bool IsRunning = false;

    public abstract void Setup(ModeManager Manager);

    public abstract void Start();

    public abstract void Update();

    public abstract IEnumerator PressCheck(int x, int y);

    public abstract IEnumerator Finish();
}

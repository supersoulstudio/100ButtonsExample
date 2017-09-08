using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGameManager : MonoBehaviour
{
    private void Awake()
    {
        SimpleGame.Mode = GetComponent<ModeManager>();
        SimpleGame.UI = GetComponent<UIManager>();
        SimpleGame.Board = GetComponent<BoardManager>();
    }

    private IEnumerator Start ()
    {
        //	Press any key
        while (true)
        {
            SimpleGame.UI.Show("Message", "Press Space");

            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }

            SimpleGame.UI.Show("Message", "Round 1");

            yield return new WaitForSeconds(2f);

            SimpleGame.Mode.Intro();

            while (SimpleGame.Mode.IsRunning)
            {
                yield return null;
            }

            SimpleGame.UI.Show("Message", "Round 2");

            yield return new WaitForSeconds(2f);

            SimpleGame.Mode.Intro();

            while (SimpleGame.Mode.IsRunning)
            {
                yield return null;
            }

            SimpleGame.Board.ShowAll(true, false, true);
        }
    }
}

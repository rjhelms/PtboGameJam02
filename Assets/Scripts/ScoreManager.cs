using UnityEngine;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager>
{
    protected ScoreManager() { }

    public int Lives = 3;
    public int Level = 1;

    public void Reset()
    {
        Lives = 3;
        Level = 1;
    }
}
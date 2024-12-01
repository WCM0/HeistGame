using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FPSLimiter : MonoBehaviour

{

    public int FPS = 60;

    void Start()

    {

        QualitySettings.vSyncCount = 1;

        Application.targetFrameRate = FPS;

    }

}


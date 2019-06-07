using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAndroidPlugin : MonoBehaviour{

    public void CallNativePlugin(){
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject unityActivity = unityPlayerClass.GetStatic("currentActivity");

        AndroidJavaObject earphone = new AndroidJavaObject("com.example.phonemodule.Earphone");

        bool hasClicked = false;
        earphone.Call("ClickEarphoneButton");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CallNativePlugin();
    }
}

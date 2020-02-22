#if false//UNITY_STANDALONE_WIN && !UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class TapInputStandaloneWin : Singleton<TapInputStandaloneWin>, ITapInput
{


    [DllImport("TAPWin")]
    extern static void TAPWinUnityBridgeStart();

    [DllImport("TAPWin")]
    extern static bool TAPWinUnityBridgeGetTapConnected(out string identifier, out string name, out int fw);

    [DllImport("TAPWin")]
    extern static bool TAPWinUnityBridgeGetTapDisconnected(out string identifier);

    [DllImport("TAPWin")]
    extern static bool TAPWinUnityBridgeGetTapped(out string identifier, out int tapcode);

    [DllImport("TAPWin")]
    extern static bool TAPWinUnityBridgeGetMoused(out string identifier, out int vx, out int vy, out bool isMouse);

    [DllImport("TAPWin")]
    extern static void TAPWinUnityBridgeSetActivated(bool enabled);

    private const Char ARGS_SEPERATOR = '|';

    public event Action OnBluetoothTurnedOn;
    public event Action OnBluetoothTurnedOff;
    public event Action<string, string, int> OnTapConnected;
    public event Action<string> OnTapDisconnected;
    public event Action<string> OnControllerModeStarted;
    public event Action<string> OnTextModeStarted;
    public event Action<string, int> OnTapInputReceived;
    public event Action<string, int, int, bool> OnMouseInputReceived;
    public event Action<string[]> OnConnectedTapsReceived;
    public event Action<string, int> OnModeReceived;
    public event Action<string, TapAirGesture> OnAirGestureInputReceived;
    public event Action<string, bool> OnTapChangedAirGestureState;
    public event Action<string, RawSensorData> OnRawSensorDataReceived;

    public void DisableDebug()
    {
       
    }

    public void EnableDebug()
    {
       
    }

    public void StartControllerMode(string tapIdentifier)
    {
       
    }

    public void StartTextMode(string tapIdentifier)
    {
        
    }

    void FixedUpdate()
    {
    
        sendTapConnected(5);
        sendTapDisconnected(5);
        sendTapped(20);
        sendMoused(20);

    }


    private void sendTapConnected(int limit)
    {
        if (OnTapConnected != null)
        {
            string identifier;
            string name;
            int fw;
            if (TAPWinUnityBridgeGetTapConnected(out identifier, out name, out fw) && limit > 0)
            {
                OnTapConnected(identifier, name, fw);
                limit--;
            }
        }


    }

    private void sendTapDisconnected(int limit)
    {
        if (OnTapDisconnected != null)
        {
            string identifier;
            if (TAPWinUnityBridgeGetTapDisconnected(out identifier) && limit > 0)
            {
                OnTapDisconnected(identifier);
                limit--;
            }
        }

    }

    private void sendTapped(int limit)
    {
        if (OnTapInputReceived != null)
        {
            string identifier;
            int tapcode;
            if (TAPWinUnityBridgeGetTapped(out identifier, out tapcode) && limit > 0)
            {
                OnTapInputReceived(identifier, tapcode);
                limit--;
            }
        }
       

    }

    private void sendMoused(int limit)
    {
        if (OnMouseInputReceived != null)
        {
            string identifier;
            int vx;
            int vy;
            bool isMouse;
            if (TAPWinUnityBridgeGetMoused(out identifier, out vx, out vy, out isMouse) && limit > 0)
            {
                OnMouseInputReceived(identifier, vx, vy, isMouse);
                limit--;
            }
        }
      

    }

    void OnApplicationQuit()
    {
        TAPWinUnityBridgeSetActivated(false);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        TAPWinUnityBridgeSetActivated(hasFocus);
        
    }

    public override void OnInit()
    {
        TAPWinUnityBridgeStart();
    }

    void SetMouseHIDEnabledInRawModeForAllTaps(bool enable)
    {

    }

    bool IsAnyTapInAirMouseState()
    {
        return false;
    }

    void readAllTapsState()
    {
        
    }


    private void onAirGestureInputReceived(String args)
    {
        if (OnAirGestureInputReceived != null)
        {
            string[] argParts = args.Split(ARGS_SEPERATOR);
            if (argParts.Length >= 2)
            {
                int gesture = 0;
                if (Int32.TryParse(argParts[1], out gesture)) 
                {
                    if (Enum.IsDefined(typeof(TapAirGesture), gesture))
                    {
                        OnAirGestureInputReceived(argParts[0], (TapAirGesture)gesture);
                    }
                }
            }
            
        }
    }

    private void onTapChangedAirGestureState(String args)
    {
        if (OnTapChangedAirGestureState != null)
        {
            string[] argParts = args.Split(ARGS_SEPERATOR);
            if (argParts.Length >= 2)
            {
                int state = 0;
                
                if (Int32.TryParse(argParts[1], out state)) 
                {
                    OnTapChangedAirGestureState(argParts[0], state == 1);
                }
            }

        }
    }

    private void onRawSensorDataReceived(String rsArg)
    {
        if (OnRawSensorDataReceived != null)
        {
            string[] argParts = rsArg.Split(ARGS_SEPERATOR);
            if (argParts.Length == 2)
            {
                RawSensorData data = RawSensorData.makeFromString(argParts[1], "^");
                if (data != null)
                {
                   OnRawSensorDataReceived(argParts[0], data);
                }
            }
        }
    }

    public void StartControllerWithMouseHIDMode(string tapIdentifier)
    {
        tapUnityAdapter.Call("startControllerWithMouseHIDMode", tapIdentifier);
    }

    public void StartRawSensorMode(string tapIdentifier, int deviceAccelerometerSensitivity, int imuGyroSensitivity, int imuAccelerometerSensitivity)
    {
        tapUnityAdapter.Call("startRawSensorMode", tapIdentifier, deviceAccelerometerSensitivity, imuGyroSensitivity, imuAccelerometerSensitivity);
    }

    public void Vibrate(string tapIdentifier, int[] durations)
    {
        string durationsString = "";
        string delimeter = "^";
        for (int i = 0; i < durations.Length; i++)
        {
            durationsString = durationsString + durations[i].ToString();
            if (i < durations.Length - 1)
            {
                durationsString = durationsString + delimeter;
            }
        }
        tapUnityAdapter.Call("vibrate", tapIdentifier, durationsString, delimeter);

    }

    public bool IsAnyTapSupportsAirMouse()
    {
        return false;
    }
}

#endif

using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PID
{
    private float _p, _i, _d;
    private float _kp, _ki, _kd;
    private float _prevError;

    /// <summary>
    /// Constant proportion
    /// </summary>
    public float Kp
    {
        get
        {
            return _kp;
        }
        set
        {
            _kp = value;
        }
    }

    /// <summary>
    /// Constant integral
    /// </summary>
    public float Ki
    {
        get
        {
            return _ki;
        }
        set
        {
            _ki = value;
        }
    }

    /// <summary>
    /// Constant derivative
    /// </summary>
    public float Kd
    {
        get
        {
            return _kd;
        }
        set
        {
            _kd = value;
        }
    }

    public PID(float p, float i, float d)
    {
        _kp = p;
        _ki = i;
        _kd = d;
    }

    /// <summary>
    /// Based on the code from Brian-Stone on the Unity forums
    /// https://forum.unity.com/threads/rigidbody-lookat-torque.146625/#post-1005645
    /// </summary>
    /// <param name="currentError"></param>
    /// <param name="deltaTime"></param>
    /// <returns></returns>
    public float GetOutput(float currentError, float deltaTime)
    {
        _p = currentError;
        _i += _p * deltaTime;
        _d = (_p - _prevError) / deltaTime;
        _prevError = currentError;
        
        return _p * Kp + _i * Ki + _d * Kd;
    }
}
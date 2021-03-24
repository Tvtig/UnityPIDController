using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PID
{
	private float _kp;
	private float _ki;
	private float _kd;

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
	
	private float _p, _i, _d;
	private float _prevError;
	
	public PID(float p, float i, float d)
    {
		_kp = p;
		_ki = i;
		_kd = d;
    }

	public float GetOutput(float currentError, float deltaTime)
	{
		_p = currentError;
		_i += _p * deltaTime;
		_d = (_p - _prevError) / deltaTime;
		_prevError = currentError;

		return _p * Kp + _i * Ki + _d * Kd;
	}

}

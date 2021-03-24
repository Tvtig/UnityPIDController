using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 5000)]
    private float _thrust = 1000f;
    [SerializeField]
    private GameObject _target = null;
    [SerializeField]
    private float _maxAngularVelocity = 20;


    [SerializeField]
    [Range(-10, 10)]
    private float _xAxisP, _xAxisI, _xAxisD;

    [SerializeField]
    [Range(-10, 10)]
    private float _yAxisP, _yAxisI, _yAxisD;

    [SerializeField]
    [Range(-10, 10)]
    private float _zAxisP, _zAxisI, _zAxisD;

    private PID _xAxisPIDController;
    private PID _yAxisPIDController;
    private PID _zAxisPIDController;

    private Rigidbody _rb;

    void Start()
    {
        //_pidController = gameObject.GetComponents<PID>()[0];
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = _maxAngularVelocity;
        _xAxisPIDController = new PID(_xAxisP, _xAxisI, _xAxisD);
        _yAxisPIDController = new PID(_yAxisP, _yAxisI, _yAxisD);
        _zAxisPIDController = new PID(_zAxisP, _zAxisI, _zAxisD);
    }

    private void Update()
    {
        _xAxisPIDController.Kp = _xAxisP;
        _xAxisPIDController.Ki = _xAxisI;
        _xAxisPIDController.Kd = _xAxisD;

        _yAxisPIDController.Kp = _yAxisP;
        _yAxisPIDController.Ki = _yAxisI;
        _yAxisPIDController.Kd = _yAxisD;

        _zAxisPIDController.Kp = _zAxisP;
        _zAxisPIDController.Ki = _zAxisI;
        _zAxisPIDController.Kd = _zAxisD;
    }

    void FixedUpdate()
    {
        //Get the required rotation based on the target position - we can do this by getting the direction
        //from the current position to the target. Then use rotate towards and look rotation, to get a quaternion thingy.
        var targetDirection = transform.position - _target.transform.position;
        Vector3 rotationDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360, 0.00f);
        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);

        //Figure out the error for each asix
        float xAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.x);
        float xTorqueCorrection = _xAxisPIDController.GetOutput(xAngleError, Time.fixedDeltaTime);

        float yAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y);
        float yTorqueCorrection = _yAxisPIDController.GetOutput(yAngleError, Time.fixedDeltaTime);

        float zAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetRotation.eulerAngles.z);
        float zTorqueCorrection = _zAxisPIDController.GetOutput(zAngleError, Time.fixedDeltaTime);

        _rb.AddRelativeTorque((xTorqueCorrection * Vector3.right) + (yTorqueCorrection * Vector3.up) + (zTorqueCorrection * Vector3.forward));
        _rb.AddRelativeForce((-Vector3.forward) * _thrust * Time.fixedDeltaTime);
    }
}

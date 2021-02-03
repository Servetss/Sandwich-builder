using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDirection : MonoBehaviour
{
    public static ThrowDirection instance;

    [Header("Transforms")]
    [SerializeField] private Transform _arrow;
    private const int _speedMax = 2;
    private const float _degreeMax = 0.6f;

    [Space]
    [Header("Throw parametrs")]
    [Range(0, _speedMax)]
    [SerializeField] private float _speedArrow;
    [Range(0, _degreeMax)]
    [SerializeField] private float _degreeArrow;
    [SerializeField] private float _throwDiapason;
    [SerializeField] private float _verticalAngle;
    private float _newDegreeArrow;

    private float _aimAngle;
    private float _horizontalAngle;

    private float _saveDegreeArrow;


    [Space]
    [SerializeField] private bool _isRotationEnabled;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _saveDegreeArrow = _degreeArrow;
        _degreeArrow = 0;
    }

    private void FixedUpdate()
    {
        _aimAngle += Time.fixedDeltaTime * _speedArrow;

        _arrow.transform.position = new Vector3(Mathf.Sin(_aimAngle) * _degreeArrow, _arrow.transform.position.y, _arrow.transform.position.z);

        _horizontalAngle = _throwDiapason * Mathf.Sin(_aimAngle) * _degreeArrow;
    }


    public void FirstShoot()
    {
        _degreeArrow = _saveDegreeArrow;
        _aimAngle = 0;
    }

    public Vector3 GetDirectionShoot()
    {
        return new Vector3(_verticalAngle, _isRotationEnabled ? _horizontalAngle : 0, 0);
    }


    public void ChangeSpeed(float addValue)
    {
        _speedArrow = Mathf.Clamp(_speedArrow + addValue, 0, _speedMax);
    }

    public void ChangeThrowDiapasone(float addValue)
    {
        _newDegreeArrow = _degreeArrow + addValue;
        _speedArrow = Mathf.Clamp(_speedArrow + addValue, 0, _speedMax);

        if (_degreeArrow < _degreeMax) InvokeRepeating("SmoothChangeDegreeArrow", 0, Time.deltaTime * 2);
    }

    private void SmoothChangeDegreeArrow()
    {
        _degreeArrow += Time.deltaTime;

        if (_degreeArrow >= _newDegreeArrow) CancelInvoke("SmoothChangeDegreeArrow");
    }

    public float GetArrowSpeed()
    {
        return _speedArrow;
    }


    public void ResetData()
    {
        _speedArrow = 1;
        _degreeArrow = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAngle : MonoBehaviour
{
    [SerializeField] private Transform _pointToTarget;
    [SerializeField] private Transform _pointTargeted;

    private float _angle;
    private void FixedUpdate()
    {
        _angle = Vector3.SignedAngle(_pointToTarget.up, ((_pointToTarget.position + _pointToTarget.up) - _pointTargeted.position).normalized, Vector3.forward);
        print(_angle);

        Debug.DrawLine(_pointToTarget.up, ((_pointToTarget.position + _pointToTarget.up) - _pointTargeted.position).normalized, Color.red, Time.fixedDeltaTime);



        Debug.DrawLine(Vector3.zero, _pointToTarget.up, Color.green, Time.fixedDeltaTime);

        Debug.DrawLine(_pointToTarget.position + (_pointToTarget.up * 2), _pointToTarget.position, Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(_pointToTarget.position + (_pointToTarget.up * 2), _pointTargeted.position, Color.green, Time.fixedDeltaTime);
    }
}

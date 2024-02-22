using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraFacer : MonoBehaviour
{
    private Transform _camTrans;
    private Transform _myTrans;
    private Quaternion _lookRot;
    
    private void Start()
    {
        _camTrans = Camera.main.transform;
        _myTrans = transform;
    }
    
    private void LateUpdate()
    {
        _lookRot = Quaternion.LookRotation(_myTrans.position - _camTrans.position);
        _lookRot.z = 0;
        _myTrans.rotation = _lookRot;
    }
}
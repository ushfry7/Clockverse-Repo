using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class ThirdPersonCmControl : MonoBehaviour
{
    CinemachineOrbitalFollow _cameraCM;
    float _verticalDown = 2f, _verticalUp = 50f, _mouseY, _verticalClamp = 10f;
    public float _sensitivity = 300f;

    private void Start()
    {
        _sensitivity = FindFirstObjectByType<GameUIManager>().CameraSensitivity();
        _cameraCM = GetComponent<CinemachineOrbitalFollow>();
    }

    void Update()
    {
        _cameraCM.HorizontalAxis.Value += Input.GetAxisRaw("Mouse X") * Time.deltaTime * (_sensitivity / 2f);
        _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (_sensitivity / 2f);
        if (_cameraCM.VerticalAxis.Value < _verticalDown && _cameraCM.VerticalAxis.Value > _verticalUp)
        {
            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_mouseY * Time.deltaTime * _sensitivity, - _verticalClamp, _verticalClamp);
        }
        else if (_cameraCM.VerticalAxis.Value >= _verticalDown && Mathf.Clamp(Mathf.Atan(_mouseY * Time.deltaTime * _sensitivity), - _verticalClamp, _verticalClamp) > 0)
        {
            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_mouseY * Time.deltaTime * _sensitivity, - _verticalClamp, _verticalClamp);
        }
        else if (_cameraCM.VerticalAxis.Value <= _verticalUp && Mathf.Clamp(Mathf.Atan(_mouseY * Time.deltaTime * _sensitivity), - _verticalClamp, _verticalClamp) < 0f)
        {
            _cameraCM.VerticalAxis.Value -= Mathf.Clamp(_mouseY * Time.deltaTime * _sensitivity, - _verticalClamp, _verticalClamp);
        }
    }
}

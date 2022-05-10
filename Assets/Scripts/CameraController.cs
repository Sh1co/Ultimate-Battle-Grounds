using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _xAxisSpeed = 1.0f;
    [SerializeField] private float _yAxisSpeed = 1.0f;
    [SerializeField] private float _boostMultiplier = 2.0f;
    [SerializeField] [Range(0, 1)] private float _mouseDetectionSize = 0.05f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (Input.mousePosition.y >= Screen.height * (1-_mouseDetectionSize) || Input.mousePosition.y <= Screen.height * _mouseDetectionSize)
        {
            vertical = GetMouseAxis(Screen.height, Input.mousePosition.y);
        }
        
        if (Input.mousePosition.x >= Screen.width * (1-_mouseDetectionSize) || Input.mousePosition.x <= Screen.width * _mouseDetectionSize)
        {
            horizontal = GetMouseAxis(Screen.width, Input.mousePosition.x);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontal *= _boostMultiplier;
            vertical *= _boostMultiplier;
        }

        transform.Translate(vertical * _xAxisSpeed * Time.unscaledDeltaTime, 0,
            horizontal * _yAxisSpeed * Time.unscaledDeltaTime);
    }

    private float GetMouseAxis(float screenSize, float mousePos)
    {
        var ret = mousePos < screenSize * _mouseDetectionSize ? -1f : 1f;
        if (mousePos <= screenSize * _mouseDetectionSize)
        {
            ret *= GetMouseBoost(screenSize * _mouseDetectionSize, 0, mousePos);
        }
        else
        {
            ret *= GetMouseBoost(screenSize * (1-_mouseDetectionSize), screenSize, mousePos);
        }

        return ret;
    }
    
    private float GetMouseBoost(float start, float end, float mousePos)
    {
        var relativePos = mousePos - start;
        var normalizedPos = relativePos / (end - start);
        return Mathf.Lerp(1, _boostMultiplier, normalizedPos);
    }
}
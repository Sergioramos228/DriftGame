using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private const float FullBreakTimePower = 1.3f;

    [SerializeField] private CarPhysic _physic;
    [SerializeField, Range(0.4f, 10f)] private float _steeringPower;
    [SerializeField] private CarBooster _booster;

    private UserInput _carActions;
    private Vector2 _moveInput;
    private float _currentAngle;
    private Coroutine _moving;
    private Coroutine _breaking;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();

        if (_photonView.IsMine)
            _carActions = new UserInput();
    }

    private void OnEnable()
    {
        if (_photonView.IsMine)
        {
            _carActions.Car.Enable();
            _carActions.Car.Move.started += OnStartGas;
            _carActions.Car.Move.canceled += OnStopGas;
            _carActions.Car.Move.performed += OnPerformedGas;
            _carActions.Car.Stop.started += OnStartBreak;
            _carActions.Car.Stop.canceled += OnStopBreak;
            _carActions.Car.Boost.performed += OnBoost;
        }
    }

    private void OnDisable()
    {
        if (_photonView.IsMine)
        {
            _carActions.Car.Disable();
            _carActions.Car.Move.started -= OnStartGas;
            _carActions.Car.Move.canceled -= OnStopGas;
            _carActions.Car.Move.performed -= OnPerformedGas;
            _carActions.Car.Stop.started -= OnStartBreak;
            _carActions.Car.Stop.canceled -= OnStopBreak;
            _carActions.Car.Boost.performed -= OnBoost;
        }
    }

    private void OnPerformedGas(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnStopBreak(InputAction.CallbackContext context)
    {
        if (_breaking == null)
            return;

        StopCoroutine(_breaking);
        _breaking = null;
        _physic.BreakTorque();
    }

    private void OnStartBreak(InputAction.CallbackContext context)
    {
        _breaking = StartCoroutine(Breaking());
    }

    private void OnStartGas(InputAction.CallbackContext context)
    {
        _moving = StartCoroutine(Moving());
    }

    private void OnStopGas(InputAction.CallbackContext context)
    {
        _physic.GasTorque();

        if (_moving == null)
            return;

        StopCoroutine(_moving);
        _currentAngle = 0;
        _physic.ApplyDirection(_currentAngle);
        _moving = null;
    }

    private void OnBoost(InputAction.CallbackContext context)
    {
        if (_booster.IsReady)
            _booster.Boost();
    }

    private IEnumerator Moving()
    {
        WaitForSeconds delay = new WaitForSeconds(1 / 144);
        yield return null;

        while (true)
        {
            _physic.GasTorque(Mathf.Sign(_moveInput.y) * 1);
            _currentAngle = Mathf.Lerp(_currentAngle, _moveInput.x, _steeringPower * Time.deltaTime);
            _physic.ApplyDirection(_currentAngle);
            yield return delay;
        }
    }

    private IEnumerator Breaking()
    {
        WaitForSeconds delay = new WaitForSeconds(1 / 144);
        float currentTime = 0;
        yield return null;

        while (currentTime < FullBreakTimePower)
        {
            _physic.BreakTorque(Mathf.Clamp01(currentTime += Time.deltaTime / FullBreakTimePower));
            yield return delay;
        }
    }
}
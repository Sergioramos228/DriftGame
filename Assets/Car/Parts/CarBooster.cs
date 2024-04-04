using System;
using System.Collections;
using UnityEngine;

public class CarBooster : MonoBehaviour
{
    [SerializeField] private int _maxBoostCount;
    [SerializeField] private float _boostPower;
    [SerializeField] private CarPhysic _physic;
    [SerializeField] private float _reloadSecondsDelay;

    private WaitForSeconds _delay;
    private int _currentBoostCount;
    private Coroutine _reload;

    public bool IsReady => _currentBoostCount > 0;
    public event Action<int> BoosterCountChanged;

    private void Awake()
    {
        _delay = new WaitForSeconds(_reloadSecondsDelay);
        _currentBoostCount = _maxBoostCount;
    }

    private void OnEnable()
    {
        if (_currentBoostCount < _maxBoostCount)
            _reload = StartCoroutine(Reload());

        BoosterCountChanged?.Invoke(_currentBoostCount);
    }

    private void OnDisable()
    {
        if (_reload != null)
        {
            StopCoroutine(_reload);
            _reload = null;
        }
    }

    public void Boost()
    {
        if (IsReady == false)
            return;

        _currentBoostCount--;
        _physic.Boost(_boostPower);
        BoosterCountChanged?.Invoke(_currentBoostCount);

        if (_reload == null)
            _reload = StartCoroutine(Reload());
    }

    public void Init()
    {
        BoosterCountChanged?.Invoke(_currentBoostCount);
    }

    private IEnumerator Reload()
    {
        yield return null;

        while (_currentBoostCount < _maxBoostCount)
        {
            yield return _delay;
            _currentBoostCount++;
            BoosterCountChanged?.Invoke(_currentBoostCount);
        }

        _reload = null;
    }
}

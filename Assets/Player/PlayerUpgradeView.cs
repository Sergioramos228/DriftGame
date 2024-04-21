using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUpgradeView : MonoBehaviour
{
    [SerializeField] private Button _upgrade;
    [SerializeField] private TMP_Text _upgradeLabel;
    [SerializeField] private TypesOfPlayerProperties _property;
    [SerializeField] private TMP_Text _cost;

    public TypesOfPlayerProperties Property => _property;

    public event Action<PlayerUpgradeView> Upgraded;

    private void OnEnable()
    {
        _upgrade.onClick.AddListener(OnButtonUpgradeClick);
    }

    private void OnDisable()
    {
        _upgrade.onClick.RemoveListener(OnButtonUpgradeClick);
    }

    private void OnButtonUpgradeClick()
    {
        Upgraded?.Invoke(this);
    }

    public void ChangeText(int currentLevel, int maxLevel, int cost)
    {
        _upgradeLabel.text = $"{currentLevel}/{maxLevel}";
        if (cost == -1)
            _cost.text = "max";
        else
            _cost.text = cost.ToString();
    }
}

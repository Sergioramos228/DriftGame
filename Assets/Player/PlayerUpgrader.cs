using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class PlayerUpgrader : MonoBehaviour
{
    private const int MaxLevel = 3;

    [SerializeField] private List<PlayerUpgradeView> _upgrades;
    [SerializeField] private List<PlayerUpgradeSettings> _settings;
    [SerializeField] private PlayerProperties _properties;
    [SerializeField] private TMP_Text _goldView;

    private void OnEnable()
    {
        _properties.Initialized += OnUpgradesInitialized;

        foreach (PlayerUpgradeView upgrade in _upgrades)
            upgrade.Upgraded += OnUpgradeRequest;
    }

    private void OnDisable()
    {
        _properties.Initialized -= OnUpgradesInitialized;

        foreach (PlayerUpgradeView upgrade in _upgrades)
            upgrade.Upgraded -= OnUpgradeRequest;
    }

    private void UpdateGoldView()
    {
        _goldView.text = _properties.Values[TypesOfPlayerProperties.Gold].ToString();
    }

    private void OnUpgradesInitialized()
    {
        UpdateGoldView();

        foreach (PlayerUpgradeView upgrade in _upgrades)
            upgrade.ChangeText((int)_properties.Values[upgrade.Property], MaxLevel, GetNextCost(upgrade.Property));
    }

    private void OnUpgradeRequest(PlayerUpgradeView view)
    {
        int currentLevel = (int)_properties.Values[view.Property];

        if (currentLevel == MaxLevel)
            return;

        float currentGold = _properties.Values[TypesOfPlayerProperties.Gold];
        int cost = GetNextCost(view.Property, currentLevel);

        if (currentGold < cost)
            return;

        currentGold -= cost;
        currentLevel++;
        _properties.SetProperty(TypesOfPlayerProperties.Gold, currentGold);
        _properties.SetProperty(view.Property, currentLevel);
        view.ChangeText(currentLevel, MaxLevel, GetNextCost(view.Property));
        UpdateGoldView();
    }

    private int GetNextCost(TypesOfPlayerProperties property, int nextLevel = -1)
    {
        if (nextLevel == -1)
            nextLevel = (int)_properties.Values[property];

        if (nextLevel == MaxLevel)
            return -1;

        return _settings.Where(s => s.Property == property).FirstOrDefault().Price[nextLevel];
    }
}

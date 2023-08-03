using BenStudios;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDumper : PowerUp
{

    private PowerupEntity m_myEntity;
    private void OnEnable()
    {
        m_myEntity = GetComponentInParent<PowerupEntity>();
    }

    public override void Init()
    {
    }

    public override void PerformPowerupAction()
    {
        GlobalEventHandler.RequestToFruitDumperPowerupAction?.Invoke();
        GlobalEventHandler.RequestToActivatePowerUpMode(PowerupType.FruitDumper);
        GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
        if (m_myEntity != null)
        {
            m_myEntity.isOccupied = false;
        }
        gameObject.SetActive(false);
    }
}

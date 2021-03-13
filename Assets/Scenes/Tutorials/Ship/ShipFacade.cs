using UnityEngine;
using Zenject;

public class ShipFacade
{
    readonly ShipHealthHandler _healthHandler;

    public ShipFacade(ShipHealthHandler healthHandler)
    {
        _healthHandler = healthHandler;
    }

    public void TakeDamage(float damage)
    {
        _healthHandler.TakeDamage(damage);
    }

    [Inject]
    public Transform Transform
    {
        get; private set;
    }

    public class Factory : PlaceholderFactory<float, ShipFacade>
    {
    }
}
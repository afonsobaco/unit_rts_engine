using Zenject;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour, IPointerClickHandler
{
    ShipHealthHandler _healthHandler;


    [Inject]
    public void Construct(ShipHealthHandler healthHandler)
    {
        _healthHandler = healthHandler;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.TakeDamage(5);
    }

    public void TakeDamage(float damage)
    {
        _healthHandler.TakeDamage(damage);
    }
    public class Factory : PlaceholderFactory<Ship>
    {
    }
}
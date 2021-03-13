using UnityEngine;
using UnityEngine.UI;

public class ShipHealthHandler : MonoBehaviour
{
    private Text _label;
    float _health = 100;

    private void Start()
    {

        _label = GetComponentInChildren<Text>();
        if (_label)
        {
            UpdateLabel();
        }
    }

    private void Update()
    {
        if (_label)
        {
            _label.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    private void UpdateLabel()
    {
        _label.text = "Health: " + _health;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        UpdateLabel();
    }
}
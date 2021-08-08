using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character.Health;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    private TextMesh textMesh;
    private HealthComponent healthComponent;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        healthComponent = GetComponentInParent<HealthComponent>();
        healthComponent.OnHealthChanged += HealthIndicatoUpdate;
        HealthIndicatoUpdate(healthComponent.Health);
    }

    void HealthIndicatoUpdate(int value)
    {
        textMesh.text = value.ToString();
    }
}

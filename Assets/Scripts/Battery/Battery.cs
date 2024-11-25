using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] private float startingCharges;
    public float currentCharges { get; private set; }

    private void Awake()
    {
        currentCharges = startingCharges;
    }

    public void TakeCharge(float _amount)
    {
        currentCharges = Mathf.Clamp(currentCharges - _amount, 0, 10);
    }

    public void AddCharge(float _amount)
    {
        currentCharges = Mathf.Clamp(currentCharges + _amount, 0, 10);
    }
}

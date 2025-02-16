using System.Collections.Generic;
using UnityEngine;
public class SatisfactionModifier : MonoBehaviour
{
    [SerializeField] StatModifier modifierAmount;
    List<Customer> customers = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Customer>(out Customer cust))
        {
            customers.Add(cust);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Customer>(out Customer cust))
        {
            customers.Remove(cust);
        }
    }
    private void Update()
    {
        foreach (var customer in customers)
        {
            customer.Satisfaction.AddModifier(modifierAmount);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] Customer[] customerPrefabs;

    int customerCount;

    [Header("DEBUG")]
    [field: SerializeField] public List<Customer> Customers { get; private set; } = new List<Customer>();

    public event Action<Customer> OnCustomerArrived;
    public event Action<Customer> OnCustomerDeparted;

    public Customer SpawnCustomer(Transform customerSpawn, Ship ship)
    {
        if (customerPrefabs.Length == 0) return null;

        Customer customer = Instantiate(customerPrefab, customerSpawn.position, customerSpawn.rotation);
        customer.transform.parent = transform;
        customer.Initilize(ship);
        Customers.Add(customer);
        customer.name = "Customer" + ++customerCount;
        OnCustomerArrived?.Invoke(customer);
        return customer;
    }

    public void CustomerDespawn(Customer customer)
    {
        Customers.Remove(customer);
        OnCustomerDeparted?.Invoke(customer);
    }

    Customer customerPrefab => customerPrefabs[UnityEngine.Random.Range(0, customerPrefabs.Length)];
}

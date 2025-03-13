using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] Customer[] customerPrefabs;

    int customerCount;

    [Header("DEBUG")]
    [field: SerializeField] public List<Customer> Customers { get; private set; } = new List<Customer>();
    [field: SerializeField] public List<Customer.CustomerInfo> DepartedCustomers { get; private set; } = new List<Customer.CustomerInfo>();

    public event Action<Customer> OnCustomerArrived;
    public event Action<Customer> OnCustomerDeparted;

    public Customer SpawnCustomer(Transform customerSpawn, Ship ship)
    {
        if (customerPrefabs.Length == 0) return null;

        Customer customer = Instantiate(customerPrefab, customerSpawn.position, customerSpawn.rotation);
        customer.name = "Customer" + ++customerCount;
        customer.transform.parent = transform;
        customer.Initilize(ship);
        Customers.Add(customer);
        OnCustomerArrived?.Invoke(customer);
        return customer;
    }

    public void CustomerDespawn(Customer customer)
    {
        DepartedCustomers.Add(customer.Info);
        Customers.Remove(customer);
        OnCustomerDeparted?.Invoke(customer);
    }

    Customer customerPrefab => customerPrefabs[UnityEngine.Random.Range(0, customerPrefabs.Length)];

    public float AverageLastDepartedRating(int numberOfCustomers)
    {
        float average = 0;
        int count = 0;
        float total = 0;
        int startingI = Mathf.Clamp(DepartedCustomers.Count - numberOfCustomers, 0, DepartedCustomers.Count - 1);

        for (int i = startingI; i < DepartedCustomers.Count; i++)
        {
            count++;
            total += DepartedCustomers[i].Satisfaction;
        }
        average = total / count;

        return average;
    }
}

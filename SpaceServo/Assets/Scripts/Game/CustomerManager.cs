using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] Customer customerPrefab;

    [Header("DEBUG")]
    [field: SerializeField] public List<Customer> Customers { get; private set; } = new List<Customer>();

    public Customer SpawnCustomer(Transform customerSpawn, Ship ship)
    {
        if (customerPrefab == null) return null;

        Customer customer = Instantiate(customerPrefab, customerSpawn.position, customerSpawn.rotation);
        customer.Initilize(ship);
        Customers.Add(customer);
        return customer;
    }

    public void CustomerDespawn(Customer customer)
    {
        Customers.Remove(customer);
    }
}

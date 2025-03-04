using System.Collections.Generic;
using UnityEngine;
public class SatisfactionModifier : MonoBehaviour
{
    [SerializeField] float modifierValue; // how much to change per second
    List<Customer> customersInRange = new(); // list of all customers currently within range
    
    //List<Customer> customersTotal = new(); // list of all customers that have entered the range at any point
    /*[Header("Timer Variables")]
    [SerializeField] float modifierTimerCurrent = 2f;
    [SerializeField] float modifierTimerMax = 2f;*/

    private void Update()
    {
        //ModifierTimer();

        foreach(Customer customer in customersInRange)
        {
            customer.ModifySatisfaction(modifierValue * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Customer>(out Customer cust))
        {
            //customersTotal.Add(cust);
            customersInRange.Add(cust);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Customer>(out Customer cust))
        {
            customersInRange.Remove(cust);
        }
    }

    /*void ModifierTimer()
    {
        // while the timer is above 0, decrease it over time, flooring it at 0
        if (modifierTimerCurrent > 0)
        {
            modifierTimerCurrent -= Time.deltaTime;
        }
        else
        {
            modifierTimerCurrent = 0;
        }
        // when the timer reaches 0 add the satisfaction modifier to all valid customers, then reset the timer
        if (modifierTimerCurrent == 0)
        {
            AddSatisfactionModifierToAllCustomers();
            modifierTimerCurrent = modifierTimerMax;
        }
    }

    void AddSatisfactionModifierToAllCustomers()
    {
        foreach (var customer in customersInRange)
        {
            if (customer != null)
            {
                customer.Satisfaction.AddModifier(new StatModifier(modifierValue, StatModifierType.Flat, gameObject));
                float get = customer.Satisfaction.ValueCurrent; // "getting" the value shows it updated in the inspector
            }
        }
    }*/
    

    private void OnDestroy()
    {
        // if this is sold or otherwise destroyed, it shouldnt affect the satisfaction anymore, so we can remove the satisfaction this gameobject has added from all current/valid customers
        /*foreach (var customer in customersTotal)
        {
            if (customer != null)
            {
                customer.Satisfaction.RemoveAllModifiersFromSource(gameObject);
                float get = customer.Satisfaction.ValueCurrent; // "getting" the value shows it updated in the inspector
            }
        }*/
    }
}
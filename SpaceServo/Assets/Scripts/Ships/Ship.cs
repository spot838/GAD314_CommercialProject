using UnityEngine;

public class Ship : MonoBehaviour
{
    [field: SerializeField] public EState State { get; private set; } = EState.Idle;
    [SerializeField] float speed;
    [SerializeField] float verticalSpeed;

    public LandingPad LandingPad { get; private set; }
    public Customer Customer { get; private set; }

    public enum EState
    {
        Idle,
        Arriving,
        Landing,
        TakingOff,
        Leaving
    }

    private void Update()
    {
        switch (State)
        {
            case EState.Arriving:
                ArrivingUpdate();
                break;

            case EState.Landing:
                LandingUpdate();
                break;

            case EState.TakingOff: 
                TakeOffUpdate();
                break;

            case EState.Leaving: 
                LeavingUpdate();
                break;
        }
    }

    public void Initilize(LandingPad landingPad)
    {
        this.LandingPad = landingPad;
        State = EState.Arriving;
        transform.LookAt(landingPad.ArivalPosition);
        landingPad.CurrentShip = this;
    }

    private void ArrivingUpdate()
    {
        Vector3 step = transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, LandingPad.ArivalPosition) > Vector3.Distance(transform.position, transform.position + step))
        {
            transform.position += step;
        }
        else
        {
            transform.position = LandingPad.ArivalPosition;
            transform.rotation = LandingPad.transform.rotation;
            State = EState.Landing;
        }
    }

    private void LandingUpdate()
    {
        float step = verticalSpeed * Time.deltaTime;
        if (step < transform.position.y - LandingPad.LandingLevel)
        {
            Vector3 position = transform.position;
            position.y -= step;
            transform.position = position;
        }
        else
        {
            Vector3 position = transform.position;
            position.y = LandingPad.LandingLevel;
            transform.position = position;

            State = EState.Idle;
            Customer = Game.CustomerManager.SpawnCustomer(LandingPad.CustomerSpawnPoint, this);
        }
    }

    private void TakeOffUpdate()
    {
        float step = verticalSpeed * Time.deltaTime;
        if (step < LandingPad.ArivalPosition.y - transform.position.y)
        {
            Vector3 position = transform.position;
            position.y += step;
            transform.position = position;
        }
        else
        {
            transform.position = LandingPad.ArivalPosition;
            transform.LookAt(Game.ShipManager.DeparturePoint);
            State = EState.Leaving;
            LandingPad.CurrentShip = null;
            LandingPad = null;
        }
    }

    private void LeavingUpdate()
    {
        Vector3 step = transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, Game.ShipManager.DeparturePoint) > Vector3.Distance(transform.position, transform.position + step))
        {
            transform.position += step;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Game.ShipManager.ShipDespawn(this);
    }

    public void BeginTakeOff()
    {
        State = EState.TakingOff;
        Customer = null;
    }
}

using NUnit.Framework;
using UnityEngine;

public class ObjectiveSystem : MonoBehaviour
{
    [field: SerializeField] public Objective[] Objectives { get; private set; }

    private void Start()
    {
        UI.UpdateObjectives();
    }

    private void OnEnable()
    {
        Station.OnRoomAdded += RoomBuilt;
        Station.OnPlaceableAdded += PlaceablBuilt;
        Station.Money.OnAmountChange += MoneyAmountChanged;
        Station.Rating.OnRatingChange += RatingChanged;
        Station.Staff.OnNewHire += OnHireStaff;
    }

    private void OnDisable()
    {
        Station.OnRoomAdded -= RoomBuilt;
        Station.OnPlaceableAdded -= PlaceablBuilt;
        Station.Money.OnAmountChange -= MoneyAmountChanged;
        Station.Rating.OnRatingChange -= RatingChanged;
        Station.Staff.OnNewHire -= OnHireStaff;
    }

    private void RoomBuilt(RoomObject room)
    {
        bool updated = false;

        if (Game.Tutorial.IsRunning)
        {
            if (Game.Tutorial.CurrentPart.Type == Tutorial.TutorialPart.EType.Objective &&
                Game.Tutorial.CurrentPart.Objective.Type == Objective.EType.BuildRoom &&
                room.Config == (Room)Game.Tutorial.CurrentPart.Objective.Config)
                    Game.Tutorial.PartComplete();
        }
        else
        {
            foreach (Objective objective in Objectives)
            {
                if (objective.Complete || objective.Type != Objective.EType.BuildRoom) continue;

                if (room.Config == (Room)objective.Config)
                {
                    objective.Complete = true;
                    updated = true;
                }
            }
        }
            
        if (updated) UI.UpdateObjectives();
    }

    private void MoneyAmountChanged()
    {
        bool updated = false;
        if (Game.Tutorial.IsRunning)
        {
            if (Game.Tutorial.CurrentPart.Type == Tutorial.TutorialPart.EType.Objective &&
                Game.Tutorial.CurrentPart.Objective.Type == Objective.EType.MoneyTotal &&
                Station.Money.Amount >= Game.Tutorial.CurrentPart.Objective.Value)
            {
                Game.Tutorial.PartComplete();
            }
        }
        else
        {
            foreach (Objective objective in Objectives)
            {
                if (objective.Type != Objective.EType.MoneyTotal) continue;
                bool previous = objective.Complete;
                objective.Complete = Station.Money.Amount >= objective.Value;
                updated = previous == objective.Complete;
            }
        }
            
        if (updated) UI.UpdateObjectives();
    }

    private void OnHireStaff(PlaceableObject placeable, StaffMember staffMember)
    {
        bool updated = false;
        if (Game.Tutorial.IsRunning)
        {
            if (Game.Tutorial.CurrentPart.Type == Tutorial.TutorialPart.EType.Objective &&
                Game.Tutorial.CurrentPart.Objective.Type == Objective.EType.HireEmployee &&
                placeable.Config == Game.Tutorial.CurrentPart.Objective.Config)
            {
                Game.Tutorial.PartComplete();
            }
        }
        else
        {
            foreach (Objective objective in Objectives)
            {
                if (objective.Complete || objective.Type != Objective.EType.HireEmployee) continue;

                if (placeable == (Placeable)objective.Config)
                {
                    objective.Complete = true;
                    updated = true;
                }
            }
        }
        if (updated) UI.UpdateObjectives();
    }

    private void PlaceablBuilt(PlaceableObject placeableObject)
    {
        bool updated = false;
        if (Game.Tutorial.IsRunning)
        {
            if (Game.Tutorial.CurrentPart.Type == Tutorial.TutorialPart.EType.Objective &&
                Game.Tutorial.CurrentPart.Objective.Type == Objective.EType.BuildPlaceable &&
                placeableObject.Config == (Placeable)Game.Tutorial.CurrentPart.Objective.Config)
                    Game.Tutorial.PartComplete();
        }
        else
        {
            foreach (Objective objective in Objectives)
            {
                if (objective.Complete || objective.Type != Objective.EType.BuildPlaceable) continue;

                if (placeableObject.Config == (Placeable)objective.Config)
                {
                    objective.Complete = true;
                    updated = true;
                }
            }
        }
        if (updated) UI.UpdateObjectives();
    }

    private void RatingChanged()
    {
        bool updated = false;
        if (Game.Tutorial.IsRunning)
        {
            if (Game.Tutorial.CurrentPart.Type == Tutorial.TutorialPart.EType.Objective &&
                Game.Tutorial.CurrentPart.Objective.Type == Objective.EType.Rating &&
                Station.Rating.Value >= Game.Tutorial.CurrentPart.Objective.Value)
            {
                Game.Tutorial.PartComplete();
            }
        }
        else
        {
            foreach (Objective objective in Objectives)
            {
                if (objective.Type != Objective.EType.Rating) continue;
                bool previous = objective.Complete;
                objective.Complete = Station.Rating.Value >= objective.Value;
                updated = previous == objective.Complete;
            }
        }
        if (updated) UI.UpdateObjectives();
    }
}

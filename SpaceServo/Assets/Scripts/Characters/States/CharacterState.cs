using UnityEngine;

public abstract class CharacterState
{
    public abstract void StateStart();

    public abstract void StateTick();

    public abstract void StateEnd();

    public abstract string Status();
}

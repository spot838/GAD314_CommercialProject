using UnityEngine;

public abstract class CharacterState
{
    protected Character character;

    public CharacterState(Character character)
    {
        this.character = character;
    }

    public abstract void StateStart();

    public abstract void StateTick();

    public abstract void StateEnd();
}

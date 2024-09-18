using UnityEngine;

public abstract class Target : MonoBehaviour
{
    // Abstract class for interaction targets, such as doors, traps etc.
    // Activate method must be overridden
    // Deactivate method must be overridden if any connected interactable is reversible

    public abstract void Activate();

    public abstract void Deactivate();
}

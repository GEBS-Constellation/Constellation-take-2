using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("The targets that will be affected when the object is interacted with")]
    public Target[] targets = new Target[] {};
    [Tooltip("Whether or not the object can be returned to deactive state from a second interaction")]
    public bool reversable;

    [HideInInspector] public bool selected;
    [HideInInspector] public bool active;
    private Animation anim;

    void Start() {
        anim = GetComponent<Animation>();
        selected = false;
        active = false;
    }

    void Update() {
        // Show/hide icon
        if (selected && (!active || reversable)) {
            transform.GetChild(0).gameObject.SetActive(true);
        } else {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Interact() {
        // Called when object is interacted with

        // Prevent interaction while animation is playing
        if (!anim.isPlaying) {
            // Check current state and act accordingly
            if (!active) {
                foreach (Target t in targets) {
                    t.Activate();
                }
                anim.Play("Interact");
                active = true;
            } else if (reversable) {
                foreach (Target t in targets) {
                    t.Deactivate();
                }
                anim.Play("InteractRev");
                active = false;
            }
        }
    }

    public void ReturnToDefault() {
        // Return object to default (deactivated) state. Used by PuzzleManager from incorrect input
        if (active) {
            anim.Play("InteractRev");
            active = false;
        }
    }
}

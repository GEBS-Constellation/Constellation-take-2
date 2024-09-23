using UnityEngine;

public class Door : Target
{
    private Animation anim;

    void Start() {
        anim = GetComponent<Animation>();
    }

    public override void Activate() {
        anim.PlayQueued("Open");
    }

    public override void Deactivate() {
        anim.PlayQueued("Close");
    }
}

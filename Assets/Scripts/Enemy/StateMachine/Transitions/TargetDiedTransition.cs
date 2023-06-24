using UnityEngine;

public class TargetDiedTransition : Transition
{
    private void Update()
    {
        if(Target == null)
        {
            NeedTransit = true;
        }
    }
}

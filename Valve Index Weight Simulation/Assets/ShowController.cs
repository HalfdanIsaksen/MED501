using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShowController : MonoBehaviour
{
    public bool showController = false;

    // Update is called once per frame
    void Update()
    {
        //creates a variable for each hand of the player
        //then checks if we want to show the controllers in the virtuel environment
        //if we want to show them place them according to the hands skeletons range.
        foreach (var hand in Player.instance.hands)
        {
            if(showController)
            {
                hand.ShowController();
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
            }else{
                hand.HideController();
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
            }
        }
    }
}

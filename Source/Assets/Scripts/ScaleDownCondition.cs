using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolobrainConstants;

public class ScaleDownCondition : SwitchRoomUICondition {

    public override bool SwitchCondition()
    {
        return !GameObject.Find(Names.BRAIN_GAMEOBJECT_NAME).GetComponent<ScaleToggler>().IsSmallestScale() && GameObject.Find("Brain").GetComponent<StateAccessor>().AbleToTakeAnInteraction();
    }
}

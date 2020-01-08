using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolobrainConstants;

public class ScaleUpCondition : SwitchRoomUICondition {

    public override bool SwitchCondition()
    {
        return !GameObject.Find(Names.BRAIN_GAMEOBJECT_NAME).GetComponent<ScaleToggler>().IsLargestScale() && GameObject.Find("Brain").GetComponent<StateAccessor>().AbleToTakeAnInteraction();
    }
}

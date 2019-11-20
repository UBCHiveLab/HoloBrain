using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandCollapseCondition : SwitchRoomUICondition {

    // Use this for initialization
    public override bool SwitchCondition()
    {
        return GameObject.Find("Brain").GetComponent<StateAccessor>().AbleToTakeAnInteraction();
    }
}

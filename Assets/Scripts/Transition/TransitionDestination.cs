using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 传送点的标记
public class TransitionDestination : MonoBehaviour
{
    public enum DestinationTag
    {
        ENTER, A, B, C
    }
    public DestinationTag destinationTag;
}

    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ´«ËÍµã
public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene, EndGame
    }
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;

    private void Update()
    {
        //Debug.Log(canTrans);
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            canTrans = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            canTrans = false;
        }
    }
}

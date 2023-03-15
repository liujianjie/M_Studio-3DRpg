using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public void OnDestroy()
    {
        GameUIAndLogic.Instance.Lv1ShowPotral();
        GameUIAndLogic.Instance.Lv1ShowTip();
    }
}

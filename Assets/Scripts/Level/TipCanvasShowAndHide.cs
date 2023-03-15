using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipCanvasShowAndHide : MonoBehaviour
{
    public void OnEnable()
    {
        StartCoroutine(Hide());
    }
    public IEnumerator Hide()
    {
        yield return new WaitForSeconds(4);
        this.gameObject.SetActive(false);
    }
}


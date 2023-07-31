using UnityEngine;
using UnityEngine.UI;
 
[RequireComponent(typeof(Scrollbar))]
public class AutoHideScrollbar : MonoBehaviour
{
    private Scrollbar scrollbar;
 
    private void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }
 
    private void Update()
    {
        scrollbar.targetGraphic.gameObject.SetActive(scrollbar.size > 1f);
    }
}
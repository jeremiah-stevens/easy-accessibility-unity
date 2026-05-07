using EasyAccessibility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RebindableConflictResolution : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI conflictText;
    BindingConflict conflict;

    GameObject prevActiveObject;

    public void ResolveConflict()
    {
        this.gameObject.SetActive(true);
        prevActiveObject = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(this.GetComponentInChildren<Button>()?.gameObject);
        this.conflict = RebindableInputManager.Instance.CurrentConflict;
        conflictText.text = conflict.GetConflictDescription();
    }

    public void ResolveConflictBlock()
    {
        RebindableInputManager.Instance.ResolveConflictBlock(conflict);
        if(prevActiveObject)
        {
            EventSystem.current.SetSelectedGameObject(prevActiveObject);
            prevActiveObject = null;
        }
        this.gameObject.SetActive(false);
    }
    public void ResolveConflictClear()
    {
        RebindableInputManager.Instance.ResolveConflictClear(conflict);
        if(prevActiveObject)
        {
            EventSystem.current.SetSelectedGameObject(prevActiveObject);
            prevActiveObject = null;
        }
        this.gameObject.SetActive(false);
    }
    public void ResolveConflictSwap()
    {
        RebindableInputManager.Instance.ResolveConflictSwap(conflict);
        if(prevActiveObject)
        {
            EventSystem.current.SetSelectedGameObject(prevActiveObject);
            prevActiveObject = null;
        }
        this.gameObject.SetActive(false);
    }
}

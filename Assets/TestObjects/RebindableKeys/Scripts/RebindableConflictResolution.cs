using EasyAccessibility;
using TMPro;
using UnityEngine;

public class RebindableConflictResolution : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI conflictText;
    BindingConflict conflict;

    public void ResolveConflict()
    {
        this.gameObject.SetActive(true);
        this.conflict = RebindableInputManager.Instance.CurrentConflict;
        conflictText.text = conflict.GetConflictDescription();
    }

    public void ResolveConflictBlock() { RebindableInputManager.Instance.ResolveConflictBlock(conflict); this.gameObject.SetActive(false); }
    public void ResolveConflictClear() { RebindableInputManager.Instance.ResolveConflictClear(conflict); this.gameObject.SetActive(false); }
    public void ResolveConflictSwap() { RebindableInputManager.Instance.ResolveConflictSwap(conflict); this.gameObject.SetActive(false); }
}

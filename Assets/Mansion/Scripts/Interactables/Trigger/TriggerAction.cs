using HFPS.Player;
using HFPS.Systems;
using HFPS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionData
{
    public TriggerAction.ActionType Type;
    public GameObject asociatedGO;
    public float waitTimeAfterAction = 0;
    public string GameManagerMethodKey;
}

public class TriggerAction : MonoBehaviour
{
    public enum ActionType { ACTIVATE_OBJECT, DEACTIVATE_OBJECT, JUMP_SCARE, FADE_IN, FADE_OUT, LOCKDOOR, UNLOCKDOOR, GAMEMANAGER}

    [SerializeField] private ActionData[] actions;
    public bool RemoveComponentAfterTrigger;

    public void OnTriggerAction()
    {
        StartCoroutine(ProcessActions());
    }

    private IEnumerator ProcessActions()
    {
        foreach (var a in actions)
        {
            yield return ExecuteAction(a);
        }

        if (RemoveComponentAfterTrigger)
            DestroyImmediate(this);
    }

    private IEnumerator ExecuteAction(ActionData action)
    {
        switch (action.Type)
        {
            case ActionType.DEACTIVATE_OBJECT:
                if (action.asociatedGO != null)
                    action.asociatedGO.SetActive(false);
                break;
            case ActionType.JUMP_SCARE:
                JumpscareEffects.GenericJumpScare(0.8f, 0.3f, 10f, 8f);
                break;
            case ActionType.FADE_IN:
                UIFadePanel.Instance.FadeIn(true);
                break;
            case ActionType.ACTIVATE_OBJECT:
                if (action.asociatedGO != null)
                    action.asociatedGO.SetActive(true);
                break;
            case ActionType.FADE_OUT:
                UIFadePanel.Instance.FadeOut();
                break;
            case ActionType.LOCKDOOR:
                action.asociatedGO.GetComponent<DynamicObjectAnimator>().LockDoor(true);
                break;
            case ActionType.UNLOCKDOOR:
                action.asociatedGO.GetComponent<DynamicObjectAnimator>().LockDoor(false);
                break;
            case ActionType.GAMEMANAGER:
                GameManager.Instance.CallMethod(action.GameManagerMethodKey);
                break;
        }

        if (action.waitTimeAfterAction > 0)
            yield return new WaitForSeconds(action.waitTimeAfterAction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static VariableContainer;

public class GameEvents : MonoBehaviour
{
    [HideInInspector] public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    [System.Serializable]
    public class CollectibleEvent : UnityEvent<CollectibleType>
    {

    }

    public CollectibleEvent collectedItem;

    public void CollectedItem(CollectibleType collectibleType)
    {
        if (collectedItem != null)
        {
            collectedItem.Invoke(collectibleType);
        }
    }

    
}

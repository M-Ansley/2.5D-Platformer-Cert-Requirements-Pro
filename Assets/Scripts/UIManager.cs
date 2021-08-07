using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static VariableContainer;

public class UIManager : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    [SerializeField] private TextMeshProUGUI _beanCountText;

    private void Start()
    {
        GameEvents.current.collectedItem.AddListener(ItemCollected);
        if (FindObjectOfType<PlayerInventory>() != null)
        {
            _playerInventory = FindObjectOfType<PlayerInventory>();
        }
        StartCoroutine(SetBeanCountText());
    }

    private void ItemCollected(CollectibleType collectible)
    {
        switch (collectible)
        {
            case CollectibleType.Coffee:
                StartCoroutine(SetBeanCountText());
                break;
            case CollectibleType.Coins:
                
                break;
        }
    }

    private IEnumerator SetBeanCountText()
    {
        yield return new WaitForSecondsRealtime(0.15f);
        string text = string.Format("<b>Bean Count:</b> {0}", _playerInventory.Coffee);
        _beanCountText.text = text;
    }

}

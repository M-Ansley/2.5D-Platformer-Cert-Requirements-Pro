using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VariableContainer;

public class PlayerInventory : MonoBehaviour
{
    private Player _player;

    private int _coins = 0;
    public int Coins
    {
        get
        {
            return _coins;
        }
    }

    private int _coffee = 0;
    public int Coffee
    {
        get
        {
            return _coffee;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.collectedItem.AddListener(ItemCollected);
        if (FindObjectOfType<Player>() != null)
        {
            _player = FindObjectOfType<Player>();
        }
    }

    private void ItemCollected(CollectibleType collectible)
    {
        string message = string.Format("You collected some {0}!", collectible.ToString().ToLower());
        Debug.Log(message);

        switch (collectible)
        {
            case CollectibleType.Coffee:
                _coffee++;
                _player.IncreaseSpeed(1);
                break;
            case CollectibleType.Coins:
                _coins++;
                break;
        }
    }
}

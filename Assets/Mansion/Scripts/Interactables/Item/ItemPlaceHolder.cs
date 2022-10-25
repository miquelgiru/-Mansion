using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlaceHolder : Interactable
{
    [System.Serializable]
    public class ItemHolderPair
    {
        public DynamicItem item;
        public GameObject holder;
    }

    public List<ItemHolderPair> itemHolderPairs = new List<ItemHolderPair>();

    Dictionary<DynamicItem, GameObject> depositObjects = new Dictionary<DynamicItem, GameObject>();

    private void Start()
    {
        foreach(var i in itemHolderPairs)
        {
            depositObjects.Add(i.item, i.holder);
        }
    }

    public override void OnSelect()
    {
        base.OnSelect();
        PlaceItem();
    }

    private void PlaceItem()
    {
        //todo
        //if (PlayerController.Instance.Grabbing)
        //{
        //    var item = PlayerController.Instance.GetCurrentItem();
        //    if (depositObjects.ContainsKey(item))
        //    {
        //        PlayerController.Instance.DropItem();
        //        item.gameObject.SetActive(false);
        //        depositObjects[item].SetActive(true);
        //    }
        //}
    }
}

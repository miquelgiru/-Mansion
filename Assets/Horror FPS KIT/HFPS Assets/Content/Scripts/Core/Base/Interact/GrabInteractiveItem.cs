using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using ThunderWire.Utility;
using HFPS.UI;

#if TW_LOCALIZATION_PRESENT
using ThunderWire.Localization;
#endif

namespace HFPS.Systems
{
    public class GrabInteractiveItem : InteractiveItem
    {

        private Vector3 originalScale;
        public Vector3 grabScale;
        private Transform placeHolder;


        public bool AllowExamine;

        public override void Awake()
        {
            SetCustomConfig();
            base.Awake();
            originalScale = transform.localScale;
        }


        private void SetCustomConfig()
        {
            //settings
            itemType = ItemType.GenericItem;
            examineType = ExamineType.Object;
            messageType = MessageType.None;
            disableType = DisableType.None;

            //item settings
            interactShowTitle = false;
            floatingIcon = true;

            //examine settings
            allowExamineTake = true;
        }

        public override void UseObject()
        {
            if(itemType == ItemType.GrabItem)
            {
                if(placeHolder == null)
                    placeHolder = Camera.main.transform.parent.GetChild(2);

                transform.SetParent(placeHolder, false);
                transform.localScale = grabScale;
            }
        }
    }
}
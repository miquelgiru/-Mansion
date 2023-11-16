/*
 * DynamicObject.cs - by ThunderWire Studio
 * ver. 2.0
*/

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using ThunderWire.Helpers;
using HFPS.Player;

#if TW_LOCALIZATION_PRESENT
using ThunderWire.Localization;
#endif

namespace HFPS.Systems
{
    public class DynamicObjectAnimator : MonoBehaviour, ISaveable
    {
        private readonly RandomHelper rand = new RandomHelper();
        private Inventory inventory;

        #region Enums
        public Type_Dynamic dynamicType = Type_Dynamic.Door;
        public Type_Use useType = Type_Use.Normal;
        public Type_Key keyType = Type_Key.Script;
        #endregion

        #region GenericSettings
        private Animator m_Animator;
        private SimpleOpenClose m_openclose;
        public List<Collider> IgnoreColliders = new List<Collider>();
        public UnityEvent InteractEvent;
        public UnityEvent DisabledEvent;
        public string customText;
        public string customTextKey;

        //[InventorySelector]

        #endregion

        #region DoorSettings
        [HideInInspector] public bool isOpened;
        [HideInInspector] public float toPlayerAngle;
        #endregion

        #region ValveSettings
        public AudioClip[] valveTurnSounds;
        public float valveSoundAfter;
        public float valveTurnSpeed;
        public float valveTurnTime;
        public Type_Axis turnAxis = Type_Axis.AxisX;
        #endregion

        #region Sounds
        [Range(0, 1)]
        public float m_Volume = 1;
        public AudioClip Open;
        public AudioClip Close;
        public AudioClip LockedTry;
        public AudioClip UnlockSound;
        public AudioClip LeverUpSound;
        #endregion

        public bool hasKey;
        public bool isLocked;
        [InventorySelector]
        public int keyID;

        private bool onceUnlock;

        private Transform collisionObject;
        private Transform oldCollisionObjectParent;

        public void ParseUseType(int value)
        {
            useType = (Type_Use)value;
        }

#if TW_LOCALIZATION_PRESENT
        void OnEnable()
        {
            if (HFPS_GameManager.LocalizationEnabled)
            {
                LocalizationSystem.Subscribe(OnLocalizationUpdate, customTextKey);
            }
        }

        public void OnLocalizationUpdate(string[] trs)
        {
            customText = trs[0];
        }
#endif

        void Awake()
        {
            if (dynamicType == Type_Dynamic.Door)
            {
                if (GetLockStatus())
                {
                    isLocked = true;
                }
                else if (useType == Type_Use.Normal)
                {
                    isLocked = false;
                }
            }
            else if (dynamicType == Type_Dynamic.Drawer)
            {
                IgnoreColliders.Add(PlayerController.Instance.gameObject.GetComponent<Collider>());
                isLocked = useType != Type_Use.Normal;
            }
            else if (dynamicType == Type_Dynamic.Lever)
            {
                isLocked = false;
            }

            if (IgnoreColliders.Count > 0)
            {
                for (int i = 0; i < IgnoreColliders.Count; i++)
                {
                    Physics.IgnoreCollision(GetComponent<Collider>(), IgnoreColliders[i]);
                }
            }
            inventory = Inventory.Instance;
        }

        void Start()
        {
            Invoke("LateStart", 0.1f);
        }

        void LateStart()
        {
            m_Animator = GetComponent<Animator>();
            if (m_Animator == null)
            {
                m_Animator = GetComponentInParent<Animator>();
            }
            m_openclose = GetComponent<SimpleOpenClose>();
        }

        void Update()
        {
            if (CanLockType())
            {
                if (!isLocked)
                {
                    useType = Type_Use.Normal;
                }
            }
        }

        public void UseObject()
        {
            if (CanLockType())
            {
                if (!onceUnlock && isLocked)
                {
                    if (LockedTry && !CheckHasKey())
                    {
                        AudioSource.PlayClipAtPoint(LockedTry, transform.position, m_Volume);
                    }

                    TryUnlock();
                }
            }

            if (!isLocked)
            {
                if (dynamicType == Type_Dynamic.Door || dynamicType == Type_Dynamic.Drawer || dynamicType == Type_Dynamic.Lever)
                {
                    m_openclose.ObjectClicked();
                 
                }
            }

            var action = GetComponent<TriggerAction>();
            if (action != null) 
                action.OnTriggerAction();
        }

        private void TryUnlock()
        {
            if (hasKey)
            {
                if (UnlockSound) { AudioSource.PlayClipAtPoint(UnlockSound, transform.position, m_Volume); }
                StartCoroutine(WaitUnlock());
                onceUnlock = true;
            }
        }

        IEnumerator WaitUnlock()
        {
            if (UnlockSound)
            {
                yield return new WaitForSeconds(UnlockSound.length);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            isLocked = false;
        }

        public bool GetLockStatus()
        {
            return useType == Type_Use.Locked;
        }

        public bool CheckHasKey()
        {
            if (keyType == Type_Key.Inventory)
            {
                if (inventory)
                {
                    hasKey = inventory.CheckItemInventory(keyID);
                    return hasKey;
                }
            }
            else if (hasKey)
            {
                return true;
            }

            return false;
        }

        private bool CanLockType()
        {
            return dynamicType == Type_Dynamic.Door || dynamicType == Type_Dynamic.Drawer;
        }

        public void UnlockDoor()
        {
            if (isLocked)
            {
                if (UnlockSound) { AudioSource.PlayClipAtPoint(UnlockSound, transform.position, m_Volume); }
                isLocked = false;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() && dynamicType == Type_Dynamic.Drawer)
            {
                collisionObject = collision.transform;
                oldCollisionObjectParent = collisionObject.transform.parent;
                collisionObject.transform.SetParent(transform);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() && dynamicType == Type_Dynamic.Drawer && oldCollisionObjectParent && collisionObject)
            {
                collisionObject.transform.SetParent(oldCollisionObjectParent);
                collisionObject = null;
            }
        }

        public void DoorCloseEvent()
        {
            if (Close) { AudioSource.PlayClipAtPoint(Close, transform.position, m_Volume); }
        }

        public Dictionary<string, object> OnSave()
        {
            Dictionary<string, object> SaveData = new Dictionary<string, object>();

            if (dynamicType == Type_Dynamic.Door)
            {
                SaveData.Add("use_type", useType);
                SaveData.Add("is_opened", isOpened);
                SaveData.Add("is_locked", isLocked);
            }
            else if (dynamicType == Type_Dynamic.Drawer)
            {
                SaveData.Add("use_type", useType);
                SaveData.Add("position", new Vector2(transform.position.x, transform.position.z));
                SaveData.Add("is_opened", isOpened);
                SaveData.Add("is_locked", isLocked);
            }
            else if (dynamicType == Type_Dynamic.Lever)
            {
                SaveData.Add("use_type", useType);
            }

            return SaveData;
        }

        public void OnLoad(JToken token)
        {
            if (dynamicType == Type_Dynamic.Door)
            {
                ParseUseType((int)token["use_type"]);
                Vector3 rot = new Vector3(transform.eulerAngles.x, (float)token["door_angle"], transform.eulerAngles.z);
                transform.eulerAngles = rot;
                isOpened = (bool)token["is_opened"];
                isLocked = (bool)token["is_locked"];
            }
            else if (dynamicType == Type_Dynamic.Drawer)
            {
                ParseUseType((int)token["use_type"]);
                Vector3 pos = new Vector3((float)token["position"]["x"], transform.position.y, (float)token["position"]["y"]);
                transform.position = pos;
                isOpened = (bool)token["is_opened"];
                isLocked = (bool)token["is_locked"];
            }
            else if (dynamicType == Type_Dynamic.Lever)
            {
                ParseUseType((int)token["use_type"]);


            }
        }
    }
}
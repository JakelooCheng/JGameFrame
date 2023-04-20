using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Game.Base.ObjectPool;
using TableData;

namespace GameApp.Entitys
{
    [Serializable]
    public class MapEntityConfig : IEntityConfig
    {
        public GameObject GameObject;

        [ShowInInspector]
        [SerializeField]
        public int MapID;

        [HideInInspector]
        public int ID => MapID;

        [ShowInInspector]
        [SerializeField]
        public EntityType MapEntityType;

        [HideInInspector]
        public EntityType EntityType => MapEntityType;

        [ShowInInspector]
        [SerializeField]
        public int MapCustomID;

        [HideInInspector]
        public int CustomID => MapCustomID;

        [HideInInspector]
        public bool FacingCamera;

        public MapEntityConfig(GameObject target)
        {
            GameObject = target;

            // 默认面向摄像机
            if (target.transform.childCount > 0)
            {
                var m = target.transform.Find("M");
                if (m != null)
                {
                    var spriteRend = m.GetComponentInChildren<SpriteRenderer>();
                    FacingCamera = spriteRend != null;
                }
            }
        }
    }
}
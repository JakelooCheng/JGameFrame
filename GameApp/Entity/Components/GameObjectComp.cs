using Game.Base.EC;
using Game.Base.LifeCycle;
using UnityEngine;

namespace GameApp.Entitys
{
    public class GameObjectComp : ComponentBase, IInitAfter
    {
        private bool isCompActive = true;

        public bool IsActive => GameObject ? GameObject.activeInHierarchy : false;

        public GameObject GameObject { get; private set; }

        public Transform Transform { get; private set; }


        public void InitAfter()
        {
            if (Transform != null)
            {
                /// 劫持条件判断
                (Entity as Entity).CheckActive = () => IsActive;
            }
        }

        /// <summary>
        /// 设置 GameObject
        /// </summary>
        public void SetGameObject(GameObject gameObject)
        {
            GameObject = gameObject;
            Transform = gameObject.transform;
            GameObject.SetActive(isCompActive);
        }

        public void SetActive(bool active)
        {
            isCompActive = active;
            if (GameObject)
                GameObject.SetActive(active);
        }
    }
}
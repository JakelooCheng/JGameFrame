using Game.Base.EC;
using Game.Base.LifeCycle;
using GameApp.Entitys;
using System.Collections.Generic;
using UnityEngine;

namespace GameApp.Entitys
{
    public class FacingCameraGroupComp : ComponentBase, IInitAfter, IUpdate
    {
        private GameObjectComp gameObjectComp;

        private List<Transform> transformList;

        public void InitAfter()
        {
            gameObjectComp = Entity.GetComp<GameObjectComp>();
        }

        public void OnUpdate()
        {
            if (gameObjectComp.IsActive)
            {
                if (transformList == null)
                {
                    transformList = new List<Transform>();
                    var trans = gameObjectComp.Transform;
                    for (int index = 0; index < trans.childCount; index++)
                    {
                        transformList.Add(trans.GetChild(index).GetChild(0));
                    }
                }

                foreach (var child in transformList)
                {
                    child.rotation = Camera.main.transform.rotation;
                }
            }
        }
    }
}
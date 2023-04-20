using Game.Base.EC;
using Game.Base.LifeCycle;
using GameApp.Entitys;
using UnityEngine;

namespace GameApp.Entitys
{
    public class FacingCameraComp : ComponentBase, IInitAfter, IUpdate
    {
        private GameObjectComp gameObjectComp;

        public void InitAfter()
        {
            gameObjectComp = Entity.GetComp<GameObjectComp>();
        }

        public void OnUpdate()
        {
            if (gameObjectComp.IsActive)
            {
                gameObjectComp.Transform.GetChild(0).rotation = Camera.main.transform.rotation;
            }
        }
    }
}

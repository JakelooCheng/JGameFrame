using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Frame.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyUI : Graphic
    {

        /// <summary>
        /// 屏蔽了渲染的UI
        /// </summary>
        public override Material materialForRendering
        {
            get
            {
                return null;
            }
        }

        [System.Obsolete]
        protected override void OnPopulateMesh(Mesh m)
        {

        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        protected override void UpdateGeometry()
        {

        }

        protected override void UpdateMaterial()
        {

        }

        public override void Rebuild(CanvasUpdate update)
        {

        }

#if UNITY_EDITOR

        protected override void Reset()
        {

        }
#endif
    }

}
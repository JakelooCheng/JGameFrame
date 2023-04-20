using GameApp;
using UnityEngine.UI;

namespace GameApp.UI.System
{
    public partial class ToastWindow
    {
        public static ToastWindow Instance;

        protected override void OnCreate()
        {
            InitComponents();

            Instance = this;
        }

        public void Toast(string title, string desc)
        {
            var cell = toastPool.GetCell();
            cell.SetCell(title, desc);
            cell.Root.transform.SetParent(trToastList);
            cell.Root.transform.SetSiblingIndex(0);
        }

        private void ReleaseCell(ToastCell cell)
        {
            cell.Root.transform.SetParent(trToastPool);
            toastPool.Release(cell);
        }

        protected override void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
/// 一些简单的接口
namespace GameApp.Entitys
{
    /// <summary>
    /// 可进行交互的
    /// </summary>
    interface IInteractable
    {
        string GetDesc();
    }

    /// <summary>
    /// 可进行对话的
    /// </summary>
    interface IDialogAble
    {
        float DialogOffset { get; }

        void OnDialog();
    }
}

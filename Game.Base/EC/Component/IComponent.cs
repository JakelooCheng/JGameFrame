using System;

namespace Game.Base.EC
{
    public interface IComponent
    {
        IEntity Entity { get; set; }

        T GetComp<T>() where T : IComponent;

        IComponent GetComp(Type type);
    }
}
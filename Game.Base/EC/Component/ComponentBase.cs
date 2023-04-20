using System;

namespace Game.Base.EC
{
    public class ComponentBase : IComponent
    {
        public IEntity Entity { get; set; }

        public T GetComp<T>() where T : IComponent
        {
            return (T)GetComp(typeof(T));
        }

        public IComponent GetComp(Type type)
        {
            return Entity.GetComp(type);
        }
    }
}

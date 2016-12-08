using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer
{
    public class Container
    {
        private readonly List<Tuple<Type, Type, LifestyleType>> _cache = new List<Tuple<Type, Type, LifestyleType>>();
        private readonly List<Tuple<Type, Type, object>> _singletons = new List<Tuple<Type, Type, object>>();

        public void Register<T, V>(LifestyleType lifestyle = LifestyleType.Transient) where V : T
        {
            _cache.Add(new Tuple<Type, Type, LifestyleType>(typeof(T), typeof(V), lifestyle));
        }

        public dynamic Resolve<T>()
        {
            var tuple = _cache.SingleOrDefault(x => x.Item1 == typeof(T));
            if (tuple == null)
            {
                throw new NullReferenceException("Object has not been registered");
            }

            if (tuple.Item3 == LifestyleType.Transient)
                return ResolveTransient(tuple);

            return ResolveSingleton(tuple);
        }

        private dynamic ResolveSingleton(Tuple<Type, Type, LifestyleType> tuple)
        {
            var resolved = _singletons.SingleOrDefault(x => x.Item1 == tuple.Item1);
            if (resolved == null)
            {
                var newObj = Activator.CreateInstance(tuple.Item2, new object());
                _singletons.Add(new Tuple<Type, Type, object>(tuple.Item1, tuple.Item2, newObj));
                return newObj;
            }

            return resolved.Item3;
        }

        private dynamic ResolveTransient(Tuple<Type, Type, LifestyleType> tuple)
        {
            return Activator.CreateInstance(tuple.Item2, new object());
        }
    }

    public enum LifestyleType
    {
        Transient,
        Singleton
    }
}

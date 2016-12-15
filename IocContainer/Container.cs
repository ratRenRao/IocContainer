using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IocContainer
{
    public class Container
    {
        private readonly List<Tuple<Type, Type, LifestyleType>> _cache = new List<Tuple<Type, Type, LifestyleType>>();
        private readonly List<Tuple<Type, Type, object>> _singletons = new List<Tuple<Type, Type, object>>();
        private bool _resolvingParams = false;

        public void Register<T, V>(LifestyleType lifestyle = LifestyleType.Transient) where V : T
        {
            _cache.Add(new Tuple<Type, Type, LifestyleType>(typeof(T), typeof(V), lifestyle));
        }

        public dynamic Resolve<T>()
        {
            var tuple = _cache.SingleOrDefault(x => x.Item1 == typeof(T));
            if (tuple == null)
            {
                if(_resolvingParams)
                    return default(T);

                throw new NullReferenceException("The requested type has not been registered.");
            }

            if(tuple.Item3 == LifestyleType.Transient)
                return GenerateObject<T>(tuple.Item2);

            var resolved = _singletons.SingleOrDefault(x => x.Item1 == tuple.Item1);
            if (resolved != null) return (T) resolved.Item3;

            var newObj = GenerateObject<T>(tuple.Item2);
            if (newObj == null)
            {
                throw new NullReferenceException("One or more types required by the constructor have not been registered");
            }

            _singletons.Add(new Tuple<Type, Type, object>(tuple.Item1, tuple.Item2, newObj));
            return newObj;
        }

        private T GenerateObject<T>(Type resolvedType)
        {
            var types = _cache.Select(x => x.Item2).ToList();
            var valid = true;
            var validConstructors = new List<ConstructorInfo>();
            foreach(var t in resolvedType.GetConstructors())
            {
                if (t.GetParameters().Any(p => 
                        types.All(x => x.ReflectedType != p.GetType().ReflectedType)))
                {
                    valid = false;
                }
                if (valid)
                    validConstructors.Add(t);
            }

            return ResolveParameters<T>(resolvedType, validConstructors);
        }

        private T ResolveParameters<T>(Type resolvedType, List<ConstructorInfo> validConstructors)
        {
            _resolvingParams = true;
            for (int i = 0; i < validConstructors.Count; i++)
            {
                var parameters = validConstructors[i++].GetParameters().Select(x => x.ParameterType).ToArray();
                if (!parameters.Any())
                {
                    return (T) Activator.CreateInstance(resolvedType, new object[] {});
                }

                object[] paramObjects = GenerateParamaters(parameters).ToArray();
                if (paramObjects.Length > 0)
                {
                    return (T) Activator.CreateInstance(resolvedType, paramObjects);
                }
            }

            _resolvingParams = false;
            return default(T);
        }

        private IEnumerable<object> GenerateParamaters(Type[] paramTypes)
        {
            var resolveMethod = paramTypes.Select(param => typeof (Container)
                .GetMethod("Resolve")).First();
                

            var paramaters = new List<object>();
            foreach (var param in paramTypes)
            {
                resolveMethod.MakeGenericMethod(param);
                paramaters.Add(resolveMethod.MakeGenericMethod(param).Invoke(this, new object[] {}));
            }

            return paramaters.Any(x => x == null) ? null : paramaters;
        }
    }

    public enum LifestyleType
    {
        Transient,
        Singleton
    }
}

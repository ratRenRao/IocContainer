using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer
{
    public interface IContainer
    {
        void Register<T, TV>() where TV : T;
        void Register<T, TV>(LifestyleType lifestyleType) where TV : T;
        TV Resolve<TV>();
    }
}

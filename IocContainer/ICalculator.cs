using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer
{
    public interface ICalculator
    {
        float Add(float x, float y);
        float Subtract(float x, float y);
    }
}

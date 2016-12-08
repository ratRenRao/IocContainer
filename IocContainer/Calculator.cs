using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer
{
    public class Calculator : ICalculator
    {
        public float Add(float x, float y)
        {
            return x + y;
        }

        public float Subtract(float x, float y)
        {
            return x - y;
        }
    }
}

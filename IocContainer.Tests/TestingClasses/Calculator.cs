namespace IocContainer.Tests.TestingClasses
{
    public class Calculator : ICalculator
    {
        public Calculator()
        {
        }

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

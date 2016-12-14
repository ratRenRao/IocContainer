namespace IocContainer.Tests.TestingClasses
{
    class ScientificCalculator : IScientificCalculator
    {
        private ICalculator _calculator;

        public ScientificCalculator(ICalculator calculator)
        {
            _calculator = calculator;
        }
    }
}

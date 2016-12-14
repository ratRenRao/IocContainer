namespace IocContainer.Tests.TestingClasses
{
    public class GraphingCalculator : IGraphingCalculator
    {
        private ICalculator _calculator;
        private IScientificCalculator _scientificCalculator;

        public GraphingCalculator(ICalculator calculator, IScientificCalculator scientificCalculator)
        {
            _calculator = calculator;
            _scientificCalculator = scientificCalculator;
        }
    }
}

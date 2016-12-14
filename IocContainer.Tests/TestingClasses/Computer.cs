namespace IocContainer.Tests.TestingClasses
{
    class Computer : IComputer
    {
        private IGraphingCalculator _graphingCalculator;
        private IScientificCalculator _scientificCalculator;

        public Computer(IGraphingCalculator graphingCalculator, IScientificCalculator scientificCalculator)
        {
            _graphingCalculator = graphingCalculator;
            _scientificCalculator = scientificCalculator;
        }
    }
}

namespace ui.buildingDetails
{
    public struct ConsumptionData
    {
        public readonly double Current;
        public readonly double Max;
        public readonly double CurrentHappiness;
        public readonly double MaxHappiness;

        public ConsumptionData(double current, double max, double currentHappiness, double maxHappiness)
        {
            Current = current;
            Max = max;
            CurrentHappiness = currentHappiness;
            MaxHappiness = maxHappiness;
        }
    }
}
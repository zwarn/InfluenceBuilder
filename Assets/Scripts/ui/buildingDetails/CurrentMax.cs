namespace ui.buildingDetails
{
    public struct CurrentMax
    {
        public readonly double Current;
        public readonly double Max;

        public CurrentMax(double current, double max)
        {
            Current = current;
            Max = max;
        }
    }
}
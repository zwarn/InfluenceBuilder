using System;
using influence;
using UnityEngine;
using UnityEngine.Serialization;

namespace scriptableObjects.map
{
    [Serializable]
    public abstract class TileCondition
    {
        public abstract bool Check(InfluenceController influenceController, Vector2Int position);

        protected bool Compare<T>(T a, T b, ComparisonType comparisonType) where T : IComparable<T>
        {
            switch (comparisonType)
            {
                case ComparisonType.Equal:
                    return a.CompareTo(b) == 0;
                case ComparisonType.NotEqual:
                    return a.CompareTo(b) != 0;
                case ComparisonType.GreaterThan:
                    return a.CompareTo(b) > 0;
                case ComparisonType.LessThan:
                    return a.CompareTo(b) < 0;
                case ComparisonType.GreaterThanOrEqual:
                    return a.CompareTo(b) >= 0;
                case ComparisonType.LessThanOrEqual:
                    return a.CompareTo(b) <= 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, null);
            }
        }
    }


    [Serializable]
    public class LayerValueCondition : TileCondition
    {
        public Layer layer;
        public double threshhold;
        public ComparisonType comparisonType;

        public override bool Check(InfluenceController influenceController, Vector2Int position)
        {
            return Compare(influenceController.GetValue(layer, position.x, position.y), threshhold, comparisonType);
        }
    }

    [Serializable]
    public class HappinessValueCondition : TileCondition
    {
        public double threshhold;
        public ComparisonType comparisonType;

        public override bool Check(InfluenceController influenceController, Vector2Int position)
        {
            return Compare(influenceController.GetHappiness(position.x, position.y), threshhold, comparisonType);
        }
    }

    [Serializable]
    public class StoreValueCondition : TileCondition
    {
        public Layer layer;
        public double threshhold;
        public ComparisonType comparisonType;

        public override bool Check(InfluenceController influenceController, Vector2Int position)
        {
            return Compare(influenceController.GetStore(layer, position.x, position.y), threshhold, comparisonType);
        }
    }

    public enum ComparisonType
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual
    }
}
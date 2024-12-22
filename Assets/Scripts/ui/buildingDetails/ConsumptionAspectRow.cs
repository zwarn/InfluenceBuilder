using System;
using TMPro;
using UnityEngine;

namespace ui.buildingDetails
{
    public class ConsumptionAspectRow : AspectRow<ConsumptionData>
    {
        [SerializeField] private TMP_Text current;
        [SerializeField] private TMP_Text max;
        [SerializeField] private TMP_Text currentHappiness;
        [SerializeField] private TMP_Text maxHappiness;

        public override void SetData(ConsumptionData data)
        {
            current.text = data.Current.ToString("F1");
            max.text = data.Max.ToString("F1");

            currentHappiness.text = FormatPercentage(data.CurrentHappiness);
            maxHappiness.text = FormatPercentage(data.MaxHappiness);
        }

        private String FormatPercentage(double happiness)
        {
            var percent = happiness * 100;
            if (percent >= 100)
            {
                return percent.ToString("+#0;-#0;0") + "%";
            }
            else
            {
                return percent.ToString("+#0.0;-#0.0;0.0") + "%";
            }
        }
    }
}
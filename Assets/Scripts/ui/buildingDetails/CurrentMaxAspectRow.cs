using System;
using TMPro;
using UnityEngine;

namespace ui.buildingDetails
{
    public class CurrentMaxAspectRow : AspectRow<CurrentMax>
    {
        [SerializeField] private TMP_Text current;
        [SerializeField] private TMP_Text max;

        public override void SetData(CurrentMax data)
        {
            current.text = data.Current.ToString("F1");
            max.text = data.Max.ToString("F1");
        }
    }
}
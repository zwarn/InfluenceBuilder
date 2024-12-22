using System;
using System.Collections.Generic;
using ModestTree;
using TMPro;
using UnityEngine;

namespace ui.buildingDetails
{
    public class AspectPanel<T> : MonoBehaviour
    {
        [SerializeField] private AspectRow rowPrefab;
        [SerializeField] private Transform rowsParent;
        [SerializeField] private TMP_Text label;

        private readonly Dictionary<T, AspectRow> _rows = new();

        public void Initialize(String aspectName, Dictionary<T, String> labels)
        {
            label.text = aspectName;

            foreach (var pair in labels)
            {
                var aspectRow = Instantiate(rowPrefab, rowsParent);
                aspectRow.Initialize(pair.Value);
                _rows.Add(pair.Key, aspectRow);
            }
        }

        public void SetData(Dictionary<T, (double, double)> values)
        {
            gameObject.SetActive(!values.IsEmpty());

            foreach (var key in _rows.Keys)
            {
                _rows[key].gameObject.SetActive(values.ContainsKey(key));
            }

            foreach (var pair in values)
            {
                _rows[pair.Key].SetData(pair.Value);
            }
        }
    }
}
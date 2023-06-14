using System;
using TMPro;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.UI.Screen.Views
{
    public class LevelView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelValue;

        public int Level { get => Int32.Parse(_levelValue.text); set => _levelValue.text = value.ToString(); }
    }
}
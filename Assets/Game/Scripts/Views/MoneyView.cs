using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _value;

        public void SetValue(string value) => _value.text = value;
    }
}
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

namespace Tests.ComplexBotBehaviourTest
{
    public class TestableChairMapper : MonoBehaviour
    {
        private Chair[] _chairs;
        private Dictionary<Nationality, List<Chair>> _nationalityToChairListDictionary = new();

        public void SetChairs(Chair[] chairs)
        {
            _chairs = chairs;
        }

        public Dictionary<Nationality, List<Chair>> Dictionary => _nationalityToChairListDictionary;

        public void InvokeMapNationalityToChairListDictionary()
        {
            _nationalityToChairListDictionary.Clear();
            _nationalityToChairListDictionary.Add(Nationality.Joker, new List<Chair>(_chairs));
            foreach (Chair chair in _chairs)
            {
                var (nat1, nat2) = chair.GetNationalities();
                if (!_nationalityToChairListDictionary.ContainsKey(nat1))
                {
                    _nationalityToChairListDictionary.Add(nat1, new List<Chair>());
                }
                _nationalityToChairListDictionary[nat1].Add(chair);
                if (nat2 != null && nat1 != nat2.Value)
                {
                    if (!_nationalityToChairListDictionary.ContainsKey(nat2.Value))
                    {
                        _nationalityToChairListDictionary.Add(nat2.Value, new List<Chair>());
                    }
                    _nationalityToChairListDictionary[nat2.Value].Add(chair);
                }
            }
        }
    }
}
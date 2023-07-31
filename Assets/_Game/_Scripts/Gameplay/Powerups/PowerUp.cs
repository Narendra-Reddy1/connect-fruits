using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitFrenzy
{
    public abstract class PowerUp : MonoBehaviour
    {

        public abstract void Init();
        public abstract void PerformPowerupAction();

    }
    public enum PowerupType
    {
        FruitBomb,
        TripleBomb,
        FruitDumper,
    }
}
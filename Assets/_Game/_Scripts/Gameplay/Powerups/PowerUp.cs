using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    public abstract class PowerUp : MonoBehaviour
    {
        public TextMeshProUGUI powerupCountTxt;
        public Image powerupHolderImage;
        public Sprite powerupCountHolderSprite;
        public Sprite plusIconSprite;
        public abstract void Init();
        public abstract void PerformPowerupAction();

    }
    public enum PowerupType
    {
        FruitBomb,
        TripleBomb,
        FruitDumper,
        Hint,
    }
}
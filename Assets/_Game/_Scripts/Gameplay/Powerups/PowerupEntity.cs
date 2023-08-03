using System.Collections.Generic;
using UnityEngine;


namespace BenStudios
{
    public class PowerupEntity : MonoBehaviour
    {
        public bool isOccupied = false;
        public Transform powerupPose;
        public List<PowerUp> powerups;

        public void Init(int index)
        {
            powerups[index].gameObject.SetActive(true);
            isOccupied = true;
        }
    }
}
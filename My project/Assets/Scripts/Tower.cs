using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class Tower : MonoBehaviour
    {
        private int healthPoint;
        public int MaxHealthPoint;
        public int HealthPoint
        {
            get => healthPoint;
            set
            {
                healthPoint = value;
                GetComponentInChildren<Image>().fillAmount = (float)healthPoint / MaxHealthPoint;
                if (healthPoint <= 0) Destroy(gameObject);
            }
        }
        public void Start()
        {
            healthPoint = MaxHealthPoint;
        }
    }
}

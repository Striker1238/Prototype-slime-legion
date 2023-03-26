using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Match3.HeroAttack
{
    public class Swordsman : Projectile
    {
        //public GameObject SwordsPref;
        //[Range(1,5)]
        //public int AttackSpeed = 1;
        
        /*
        public async Task Attack()
        {
            while (true)
            {
                //var a = Instantiate(SwordsPref);

                await Task.Delay(5000 / AttackSpeed);
            }
        }
        */
        public void EndAttackAnimation()
        {
            Destroy(gameObject);
        }
    }
}


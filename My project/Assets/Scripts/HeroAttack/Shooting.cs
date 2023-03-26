using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Match3.HeroAttack
{
    public class Shooting : Projectile
    {
        public void EndAttackAnimation()
        {
            Destroy(gameObject);
        }
    }
}

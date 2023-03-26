using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Match3.HeroAttack
{
    public class Freezing : Projectile
    {
        public void EndAttackAnimation()
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector]
        public int damage;
        [Range(1, 5)]
        public int AttackSpeed = 1;
        public TypeProjectile typeProjectile;
        public async Task Move(float newX, float newY, float time)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(newX, newY, 15);

            for (float t = 0; t <= 1 * time; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, endPos, t / time);
                await Task.Delay(1);
            }

            transform.position = endPos;
        }
    }
}


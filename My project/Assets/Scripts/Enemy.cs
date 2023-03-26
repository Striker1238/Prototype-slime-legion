using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class Enemy : MonoBehaviour
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
        public int damage;


        public void Start()
        {
            HealthPoint = MaxHealthPoint;
            var towerPos = FindObjectOfType<Tower>().transform.position;
            MoveToTower(towerPos.y,30);
        }

        //Изменить
        public void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.tag == "Ball")
            {
                HealthPoint -= collision.GetComponentInParent<Projectile>().damage;
                Destroy(collision.gameObject);
            }
            else if (collision.tag == "Swords")
            {
                Debug.Log("touch");
                HealthPoint -= collision.GetComponentInParent<Projectile>().damage;
            }
            else if (collision.tag == "Tower")
            {
                collision.GetComponent<Tower>().HealthPoint -= damage;
                Destroy(gameObject);
            }
            else if (collision.tag == "Player") collision.GetComponentInParent<Cell>().TriggerEnter(this);
        }
        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player") collision.GetComponentInParent<Cell>().TriggerExit(this);
        }



        private async Task MoveToTower(float newY, float time)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(transform.position.x, newY, 15);

            for (float t = 0; t <= 1 * time; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, endPos, t / time);
                await Task.Delay(1);
            }
            transform.position = endPos;
        }

        public void OnDestroy()
        {
            FindObjectOfType<GameController>().AllEnemies.Remove(this);
            if(FindObjectOfType<GameController>().AllEnemies.Count == 0)
                FindObjectOfType<StartMatch3>().EndBattle();

        }
    }
}

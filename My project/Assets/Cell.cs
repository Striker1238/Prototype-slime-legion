using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Cell : MonoBehaviour
    {
        public static GameController _gameController;

        public int x;
        public int y;
        public int X
        {
            get => x;
            set { x = value; }
        }
        public int Y
        {
            get => y;
            set { y = value; }
        }

        private IEnumerator moveCoroutine;


        public void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
        }
        public void OnPointerEnter() => _gameController.OnPointerEnter(this);
        public void OnPointerDown() => _gameController.OnPointerDown(this);
        public void OnPointerUp() => _gameController.OnPointerUp();

        public void Move(float newX, float newY, float time)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = IEMove(newX, newY, time);
            StartCoroutine(moveCoroutine);
        }
        private IEnumerator IEMove(float newPosX, float newPosY, float time)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(newPosX, newPosY, 0);

            for (float t = 0; t <= 1 * time; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, endPos, t / time);
                yield return null;
            }

            transform.position = endPos;
        }
    }
}

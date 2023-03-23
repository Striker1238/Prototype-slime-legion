using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


        public typeHero type;
        private IEnumerator moveCoroutine;


        public void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
        }
        public void OnPointerEnter() => _gameController.OnPointerEnter(this);
        public void OnPointerDown() => _gameController.OnPointerDown(this);
        public void OnPointerUp() => _gameController.OnPointerUp();


        /// <param name="DirectionX">0 - in plus and minus, 1 - in minus, 2 - in plus</param>
        /// <param name="XPos"></param>
        public int NeighboringSimilarX(out int[] XPos, int DirectionX = 0)
        {
            List<int> _XPos = new List<int>();
            if ((X > 0) && (DirectionX == 1 || DirectionX == 0))
                if (_gameController.Matrix[X - 1, Y].cell.type == type) _XPos.Add(X - 1);
            if ((X < _gameController.Len - 1) && (DirectionX == 2 || DirectionX == 0))
                if (_gameController.Matrix[X + 1, Y].cell.type == type) _XPos.Add(X + 1);
            XPos = _XPos.ToArray();
            return _XPos.Count;
        }
        public int NeighboringSimilarY(out int[] YPos, int DirectionY = 0)
        {
            List<int> _YPos = new List<int>();
            if ((Y > 0) && (DirectionY == 1 || DirectionY == 0))
                if (_gameController.Matrix[X, Y - 1].cell.type == type) _YPos.Add(Y - 1);
            if ((Y < _gameController.Len - 1) && (DirectionY == 2 || DirectionY == 0))
                if (_gameController.Matrix[X, Y + 1].cell.type == type) _YPos.Add(Y + 1);
            YPos = _YPos.ToArray();
            return YPos.Length;
        }


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace Match3
{
    public class Cell : MonoBehaviour
    {
        public static GameController _gameController;
        public CellPos thisCellObj;
        public typeHero type;
        public int levelHero;


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
            if ((thisCellObj.X > 0) && (DirectionX == 1 || DirectionX == 0))
                if (_gameController.Matrix[thisCellObj.X - 1, thisCellObj.Y].cell.type == type &&
                    _gameController.Matrix[thisCellObj.X - 1, thisCellObj.Y].cell.levelHero == levelHero) _XPos.Add(thisCellObj.X - 1);

            if ((thisCellObj.X < _gameController.Len - 1) && (DirectionX == 2 || DirectionX == 0))
                if (_gameController.Matrix[thisCellObj.X + 1, thisCellObj.Y].cell.type == type &&
                    _gameController.Matrix[thisCellObj.X + 1, thisCellObj.Y].cell.levelHero == levelHero) _XPos.Add(thisCellObj.X + 1);

            XPos = _XPos.ToArray();
            return _XPos.Count;
        }
        public int NeighboringSimilarY(out int[] YPos, int DirectionY = 0)
        {
            List<int> _YPos = new List<int>();
            if ((thisCellObj.Y > 0) && (DirectionY == 1 || DirectionY == 0))
                if (_gameController.Matrix[thisCellObj.X, thisCellObj.Y - 1].cell.type == type &&
                    _gameController.Matrix[thisCellObj.X, thisCellObj.Y - 1].cell.levelHero == levelHero) _YPos.Add(thisCellObj.Y - 1);

            if ((thisCellObj.Y < _gameController.Len - 1) && (DirectionY == 2 || DirectionY == 0))
                if (_gameController.Matrix[thisCellObj.X, thisCellObj.Y + 1].cell.type == type &&
                    _gameController.Matrix[thisCellObj.X, thisCellObj.Y + 1].cell.levelHero == levelHero) _YPos.Add(thisCellObj.Y + 1);

            YPos = _YPos.ToArray();
            return YPos.Length;
        }


        public async Task Move(float newX, float newY, float time)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(newX, newY, 0);

            for (float t = 0; t <= 1 * time; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, endPos, t / time);
                await Task.Delay(1);
            }

            transform.position = endPos;

        }
    }
    [System.Serializable]
    public struct CellPos
    {
        [SerializeField]
        private int x;
        [SerializeField]
        private int y;
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
    }
}

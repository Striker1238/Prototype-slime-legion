using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Cell : MonoBehaviour
    {
        public static GameController _gameController;

        private int x;
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


        public void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
        }
        public void OnPointerEnter() => _gameController.OnPointerEnter(this);
        public void OnPointerDown() => _gameController.OnPointerDown(this);
        public void OnPointerUp() => _gameController.OnPointerUp();
    }
}

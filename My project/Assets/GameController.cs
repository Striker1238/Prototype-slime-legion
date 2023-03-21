using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GameController : MonoBehaviour
    {
        public float time;
        private Cell pressedCell;
        private Cell enteredPiece;
        public void OnPointerEnter(Cell cell) => enteredPiece = cell;
        public void OnPointerDown(Cell cell) => pressedCell = cell;

        private bool IsAdjacent(Cell startCell, Cell endCell) =>
            (startCell.X == endCell.X && Mathf.Abs(startCell.Y - endCell.Y) == 1) ||
            (startCell.Y == endCell.Y && Mathf.Abs(startCell.X - endCell.X) == 1);

        public void OnPointerUp()
        {
            if (IsAdjacent(pressedCell, enteredPiece))
            {
                //здесь свап
                StartCoroutine(Move());
                Debug.Log("Swap!");
            }
            else
            {
                Debug.Log("Not Swap!");
            }
        }
        public IEnumerator Move()
        {
            for (float t = 0; t <= time; t += Time.deltaTime)
            {
                pressedCell.transform.position = Vector3.Lerp(pressedCell.gameObject.transform.position, enteredPiece.gameObject.transform.position, t / time);
                enteredPiece.transform.position = Vector3.Lerp(enteredPiece.gameObject.transform.position, pressedCell.gameObject.transform.position, t / time);
                yield return null;
            }
        }
    }
}

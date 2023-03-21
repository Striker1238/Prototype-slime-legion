using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class StartMath3 : MonoBehaviour
    {
        [System.Serializable]
        public struct cellStruct
        {
            public typeHero type;
            public Cell cell;
            public float x_pos;
            public float y_pos;
        }
        [System.Serializable]
        public struct square
        {
            public Vector2 StartPos;
            public Vector2 EndPos;
        }
        private cellStruct[,] Matrix;
        public GameObject cellPref;
        public Transform perentForPref;


        public int Len;
        public square SquareArea;
        public Vector2 CellSize;
        public Vector2 Spacing;

        public void Start()
        {
            GenerateMatrix();
        }
        public void GenerateMatrix()
        {
            //—оздание матрицы €чеек
            Matrix = new cellStruct[Len, Len];
            for (int line = 0; line < Len; line++)
            {
                for (int column = 0; column < Len; column++)
                {
                    var cell = Instantiate(cellPref, perentForPref);
                    cell.GetComponent<RectTransform>().sizeDelta = CellSize;
                    cell.GetComponent<RectTransform>().localPosition = new Vector3((SquareArea.StartPos.x + (CellSize.x + Spacing.x) * line), (SquareArea.StartPos.y - (CellSize.y + Spacing.y) * column), 0);
                    cell.GetComponent<Cell>().X = line;
                    cell.GetComponent<Cell>().Y = column;
                    Matrix[line, column] = new cellStruct
                    {
                        type = (typeHero)Random.Range(0, 4),
                        cell = cell.GetComponent<Cell>(),
                        x_pos = cell.transform.position.x,
                        y_pos = cell.transform.position.y
                    };
                    //¬ременный код
                    switch (Matrix[line, column].type)
                    {
                        case typeHero.Shooting:
                            cell.GetComponent<Image>().color = Color.yellow;
                            break;
                        case typeHero.Swordsman:
                            cell.GetComponent<Image>().color = Color.grey;
                            break;
                        case typeHero.Poisoing:
                            cell.GetComponent<Image>().color = Color.green;
                            break;
                        case typeHero.Freezing:
                            cell.GetComponent<Image>().color = Color.cyan;
                            break;
                    }
                }
            }
        }
    }
}
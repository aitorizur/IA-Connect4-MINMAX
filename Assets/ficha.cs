using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ficha : MonoBehaviour
{
    public int[,] mainBoard;

    public int row, column;//1win 2lose
    public GameCTRL gamectrl;
    public bool used = false;
    

    private void OnMouseDown()
    {
        if (gamectrl.CheckGameOver(gamectrl.mainBoard)==0) 
        {
            if (gamectrl.playerTurn)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!gamectrl.fichas[row, i].GetComponent<ficha>().used)
                    {

                        gamectrl.mainBoard[row, i]=1;
                        gamectrl.fichas[row, i].GetComponent<SpriteRenderer>().color = Color.blue;
                        gamectrl.fichas[row, i].GetComponent<ficha>().used = true;

                        i = 10;
                        gamectrl.playerTurn = false;
                        gamectrl.CheckGameOver(gamectrl.mainBoard);
                        gamectrl.MakeMove();
                    }
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!gamectrl.fichas[row, i].GetComponent<ficha>().used)
                    {
                        gamectrl.mainBoard[row, i] = -1;
                        gamectrl.fichas[row, i].GetComponent<SpriteRenderer>().color = Color.red;
                        gamectrl.fichas[row, i].GetComponent<ficha>().used = true;

                        i = 10;
                        gamectrl.playerTurn = true;
                        gamectrl.CheckGameOver(gamectrl.mainBoard);
                    }
                }
            }
        }
    }
}
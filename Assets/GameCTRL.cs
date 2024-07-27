using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCTRL : MonoBehaviour
{
    public int[,] mainBoard;

    public GameObject[,] fichas;

    int high = 6, lenght = 7;

    public GameObject ficha;

    public bool playerTurn;

    List<int> possibleMoves;

    //AI
    public AIAbstract currentIA;


    int score, result = 0, bestmove = 0, bestscore = 0;
    
    private void Start()
    {
        mainBoard = new int[lenght, high];

        for (int x = 0; x < lenght; ++x)
        {
            for (int y = 0; y < high; ++y)
            {
                mainBoard[x, y] = 0;
            }
        }
        SpawnBoard();
    }


    //Spawnear el tablero
    void SpawnBoard()
    {
        fichas = new GameObject[lenght, high];

        for (int x = 0; x < lenght; ++x)
        {
            for (int y = 0; y < high; ++y)
            {
                GameObject fichaprefab = Instantiate(ficha);
                fichaprefab.transform.position = new Vector2(x * 2, y * 2);
                fichaprefab.GetComponent<ficha>().mainBoard = mainBoard;
                fichaprefab.GetComponent<ficha>().gamectrl = this;
                fichaprefab.GetComponent<ficha>().row = x;
                fichaprefab.GetComponent<ficha>().column = y;
                
                fichas[x, y] = fichaprefab;
            }
        }
    }


    //Check if ended
    public int CheckGameOver(int[,] board)
    {

        result = 0;

        for (int x = 0; x < lenght; x++)
        {
            for (int y = 0; y < high; y++)
            {
                if (board[x, y] != 0)
                {
                    if (x + 3 < lenght)
                    {
                        if (board[x, y] == board[x + 1, y])//horizontal
                        {
                            //print("H");
                            if (board[x, y] == board[x + 1, y] &&
                                board[x, y] == board[x + 2, y] &&
                                board[x, y] == board[x + 3, y])
                            {
                                if (board[x, y] == 1) { result = 1; }
                                else if (board[x, y] == -1) { result = -1; }
                            }
                        }
                    }

                    if (x + 3 < lenght && y + 3 < high)
                    {
                        //print("DU");
                        if (board[x, y] == board[x + 1, y + 1])//diagonal up
                        {
                            if (board[x, y] == board[x + 1, y + 1] &&
                                board[x, y] == board[x + 2, y + 2] &&
                                board[x, y] == board[x + 3, y + 3])
                            {
                                if (board[x, y] == 1) { result = 1; }
                                else if (board[x, y] == -1) { result = -1; }
                            }
                        }
                    }

                    if (y + 3 < high)
                    {
                        //print("V");
                        if (board[x, y] == board[x, y + 1])//vertical
                        {
                            if (board[x, y] == board[x, y + 1] &&
                                board[x, y] == board[x, y + 2] &&
                                board[x, y] == board[x, y + 3])
                            {
                                if (board[x, y] == 1) { result = 1; }
                                else if (board[x, y] == -1) { result = -1; }
                            }
                        }
                    }

                    if (x + 3 < lenght && y - 3 > 0)
                    {
                        //print("DD");
                        if (board[x, y] == board[x + 1, y - 1])//diagonal down
                        {
                            if (board[x, y] == board[x + 1, y - 1] &&
                                board[x, y] == board[x + 2, y - 2] &&
                                board[x, y] == board[x + 3, y - 3])
                            {
                                if (board[x, y] == 1) { result = 1; }
                                else if (board[x, y] == -1) { result = -1; }
                            }
                        }
                    }
                }
            }
        }
        return result;
    }
    

    /*-----------------AI STUFF------------------------------*/
    //AI
    public void MakeMove()
    {
        int x = currentIA.NextMove(mainBoard);

        if (!playerTurn)
        {
            for (int y = 0; y < high; ++y)
            {
                if (mainBoard[x, y] == 0)
                {
                    mainBoard[x, y] = -1;
                    fichas[x, y].GetComponent<SpriteRenderer>().color = Color.red;
                    fichas[x, y].GetComponent<ficha>().used = true;

                    y = high;
                    playerTurn = true;
                }
            }
        }
    }

    //Check possible moves
    public List<int> CheckMoves(int[,] board)
    {
        List<int> possibleMoves = new List<int>();

        for (int i = 0; i < lenght; ++i)
        {
            if (board[i, high - 1] == 0)
            {
                possibleMoves.Add(i);
            }
        }

        return possibleMoves;
    }

    //Generate a new board from a move
    public int[,] GenerateBoardFromMove(int[,] board, int move, int turn)
    {
        int[,] newBoard = new int[lenght,high];
        for(int x = 0; x < lenght; ++x)
        {
            for (int y = 0; y < high; ++y)
            {
                newBoard[x, y] = board[x, y];
            }
        }
        
        for (int y = 0; y < high; ++y)
        {
            if(newBoard[move, y] == 0)
            {
                newBoard[move, y] = turn;

                y = high;
            }
        }
        return newBoard;
    }
    
    //Check status
    public int CheckBoard(int[,] board, int turn)
    {
        score = CheckGameOver(board) * 100;

        int[,] boardReference = board;

        if (score == 0)//Si no es un estado final
        {
            //el score es el del mejor movimiento posible en ese tablero
            List<int> possibleMoves = CheckMoves(boardReference);

            for (int x = 0; x <possibleMoves.Count; ++x)
            {
                int[,] newBoard = GenerateBoardFromMove(boardReference, possibleMoves[x], turn);

                int moveScore = MoveScore(newBoard, possibleMoves[x], turn);

                if (moveScore > score)
                {
                    score = moveScore;
                }
                else if (moveScore == score)//Que no se quede siempre con el primero
                {
                    if (Random.Range(0, 2) == 1) { score = moveScore; }
                }
            }
        }

        return score;
    }
    
    //Checks the value of a move
    public int MoveScore(int[,] board, int move, int turn)
    {
        int score = 0, newvalue=0;
        
        for (int y = high - 1; y > -1; --y)
        {
            if (board[move, y] != 0)
            {
                //esta es la ficha que se ha puesto
                /*
                //H
                newvalue = CheckHorizontal(board, turn, move, y);
                if (newvalue > score)  { score = newvalue; }
                //V
                newvalue = CheckVertical(board, turn, move, y);
                if (newvalue > score) { score = newvalue; }
                //DD

                newvalue = CheckDiagonalDown(board, turn, move, y);
                if (newvalue > score) { score = newvalue; }

                //DU
                newvalue = CheckDiagonalUp(board, turn, move, y);
                if (newvalue > score) { score = newvalue; }
                */

                score = CheckHorizontal(board, turn, move, y) + CheckVertical(board, turn, move, y) +
                        CheckDiagonalDown(board, turn, move, y) + CheckDiagonalUp(board, turn, move, y);

                y = -1;
            }
        }
        return score;
    }


    //Get best move on board
    public int BestMove(int[,] board, int turn)
    {
        int move = 0, score = 0;

        int[,] boardReference = board;

        bool winingMove = false;

        //el score es el del mejor movimiento posible en ese tablero
        List<int> possibleMoves = CheckMoves(boardReference);

        for (int x = 0; x < possibleMoves.Count; ++x)
        {
            int[,] newBoard = GenerateBoardFromMove(boardReference, possibleMoves[x], turn);

            int y = 0;

            for(int i = high - 1; i > 0; --i)
            {
                if (board[x, i] == 0)
                {
                    --i;
                }
                else
                {
                    y = i;
                    i = 0;
                }
            }
            
            if (CheckHorizontal(newBoard, turn, x, y) == 4)
            {
                return x;
            }
            if (CheckVertical(newBoard, turn, x, y) == 4)
            {
                return x;
            }
            if (CheckDiagonalDown(newBoard, turn, x, y) == 4)
            {
                return x;
            }
            if (CheckDiagonalUp(newBoard, turn, x, y) == 4)
            {
                return x;
            }
            



            int moveScore = MoveScore(newBoard, possibleMoves[x], turn);

            if (moveScore > score)
            {
                score = moveScore;
                move = possibleMoves[x];
            }
            else if (moveScore == score)//Que no se quede siempre con el primero
            {
                if (Random.Range(0, 2) == 1) { score = moveScore; move = possibleMoves[x]; }
            }

        }

        return move;
    }



    //CHECK COMBOS
    int CheckHorizontal(int[,] board, int turn, int x, int y)
    {
        int score = 0;
        bool canContinueCombo = true;

        //Checkear conexiones antes
        for (int i = 0; i < lenght; ++i)
        {
            if (x - i > -1)
            {
                if (board[x - i, y] == turn)
                {
                    ++score;
                }
                else
                {
                    if (board[x - i, y] != 0) { canContinueCombo = false; }
                    i = lenght;
                }
            }
            else { i = lenght; }
        }
        //Checkear conexiones despues
        for (int j = 1; j < lenght; ++j)
        {
            if(x + j < lenght)
            {
                if(board[x + j, y] == turn)
                {
                    ++score;
                }
                else
                {
                    if (board[x + j, y] == 0) { canContinueCombo = true; }
                    j = lenght;
                }
            }
            else { j = lenght; }
        }
        //si no puede continuar por ningun lado no tiene valor O SI PORQUE LA RAMA ES BUENA????????
        if (!canContinueCombo) score = 0;
        
        return score;
    }

    int CheckVertical(int[,] board, int turn, int x, int y)
    {
        int score = 0;
        bool canContinueCombo = true;

        for(int i=0; i < high; ++i)
        {
            if ( y - i > -1)
            {
                if(board[x, y - i] == turn)
                {
                    ++score;
                }
                else
                {
                    i = lenght;
                }
            }
            else { i = high; }
        }

        for(int j = 1; j < high; ++j)
        {
            if (y + j < high)
            {
                if (board[x, y + j] == turn)
                {
                    ++score;
                }
                else
                {
                    if (board[x, y + j] != 0) { canContinueCombo = false; }
                    j = high;
                }
            }
            else { j = high; }
        }

        //si no puede continuar por ningun lado no tiene valor O SI PORQUE LA RAMA ES BUENA????????
        if (!canContinueCombo) score = 0;
        
        return score;
    }
    
    int CheckDiagonalDown(int[,] board, int turn, int x, int y)
    {
        int score = 0;
        bool canContinueCombo = true;
        
        for (int i=0; i < lenght; ++i)
        {
            if( x - i >-1 && y + i < high)
            {
                if (board[x - i, y + i] == turn)
                {
                    ++score;
                }
                else
                {
                    if(board[x - i, y + i] == -1) { canContinueCombo = false; }
                    i = lenght;
                }
            }
            else
            {
                canContinueCombo = false;
                i = lenght;
            }            
        }

        for (int j = 1; j < lenght; ++j)
        {
            if (x + j < lenght && y - j > -1)
            {
                if (board[x + j, y - j] == turn)
                {
                    ++score;
                }
                else
                {
                    if (board[x + j, y - j] == 0) { canContinueCombo = true; }
                    j = lenght;
                }
            }
            else { j = lenght; }
        }

        //si no puede continuar por ningun lado no tiene valor O SI PORQUE LA RAMA ES BUENA????????
        if (!canContinueCombo) score = 0;
        
        return score;
    }

    int CheckDiagonalUp(int[,] board, int turn, int x, int y)
    {
        int score = 0;
        bool canContinueCombo = true;
        
        for(int i = 0; i < lenght; ++i)
        {
            if(x - i > -1 && y - i > -1)
            {
                if (board[x - i, y - i] == turn)
                {
                    ++score;
                }
                else
                {
                    if(board[x - i, y - i] != 0) { canContinueCombo = false; }
                    i = lenght;
                }
            }
            else
            {
                canContinueCombo = false;
                i = lenght;
            }
        }

        for(int j = 1; j < lenght; ++j)
        {
            if(x + j < lenght && y + j < high)
            {
                if (board[x + j, y + j] == turn)
                {
                    ++score;
                }
                else
                {
                    if(board[x + j, y + j] == 0) { canContinueCombo = true; }
                    j = lenght;
                }
            }
            else
            {
                j = lenght;
            }
        }

        //si no puede continuar por ningun lado no tiene valor O SI PORQUE LA RAMA ES BUENA????????
        if (!canContinueCombo) score = 0;
        
        return score;
    }

    

    

    //Chekear si hay algun movimiento que pueda ganar la partida para el jugador seleccionado
    //Si no ver con que movimiento se conectan mas fichas ¿Comprobar antes si tiene futuro esa solucion?


    //si el rival tiene 3 fichas consecutivas conectads
    //darle puntuacion negativa alta para que lo evite
    //si solo hay un lado en el que puede poner y se puede tapar taparlo
    //si solo hay un lado en el que puede poner pero no se puede tapar evitarlo
    //https://www.youtube.com/watch?v=6DxHdWH-Z8A
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NegamaxAB : AIAbstract
{
    public int maxDepth, move, turn = -1;

    public int NegamaxMove(int[,] board, int depth, int alfa, int beta)
    {
        
        int bestMove=0, currentScore, bestScore;


        //Fin de recursion por jugada terminal o max depth
        if (gameCtrl.CheckGameOver(board) != 0 || depth == maxDepth)
        {
            //Scoring move es el resultado de evaluar el tablero (teniendo en cuenta el turno)
            
            //Comportamiento de negamax-> invertir los valores en los turnos impares
            if (depth % 2 == 0)
            {
                bestMove = gameCtrl.BestMove(board, turn);
            }
            else
            {
                bestMove = -gameCtrl.BestMove(board, turn);
            }
        }
        else
        {
            //Puntuacion de inicio
            bestScore = -99999999;
            
            //Movimientos
            List<int> possibleMoves = gameCtrl.CheckMoves(board);
            
            //cambiar el turno
            turn = -turn;
            
            //Recursividad        
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                //hacer minmax del tablero nuevo con el depth +1
                int[,] newBoard = gameCtrl.GenerateBoardFromMove(board, possibleMoves[i], turn);

                currentScore = -NegamaxMove(newBoard,depth+1,-beta, - Mathf.Max(alfa, bestScore));

                //Sacar valor del tablero invirtiendo el signo para el efecto nega
                currentScore = gameCtrl.CheckBoard(newBoard, turn);


                // Actualizar score si obtenemos una jugada mejor.
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = possibleMoves[i];
                }
                if (bestScore >= beta)
                {
                    return bestMove;
                }
            }
            bestMove = gameCtrl.BestMove(board, turn);
        }
        return bestMove;
    }

    public override int NextMove(int[,] board)
    {
        return NegamaxMove( board, 0, -99, 99);
    }

    //tabla de transposicion para no repetir tableros
    //esa tabla guarda un registro de estados del tablero con su core
    //antes de calcular un score se consulta la tabla para ver si esta asociado
    //si no lo esta se calcula el score y se guarda
    //comparar los estados es costoso por lo que se usa una tabla hash

}



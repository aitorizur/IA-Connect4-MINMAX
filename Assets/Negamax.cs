using System.Collections.Generic;
using UnityEngine;

public class Negamax : AIAbstract
{
    [SerializeField] private int maxDepth = 4;
    private int turn = -1;

    public override int NextMove(int[,] board)
    {
        return MinimaxNextMove(board, 0);
    }

    public int MinimaxNextMove(int[,] board, int depth)
    {
        int bestMove = 0;
        int bestScore = 0;
        int currentScore;
        int scoringMove;
        int[,] newboard;

        if (gameCtrl.CheckGameOver(board) != 0 || depth == maxDepth)
        {
            if (depth % 2 == 0)
            {
                bestMove = gameCtrl.BestMove(board, turn);
            }
            else
            {
                bestMove = -gameCtrl.BestMove(board, turn);
            }
            scoringMove = gameCtrl.BestMove(board, turn);
        }
        else
        {
            bestScore = -999999;

            List<int> possibleMoves;
            possibleMoves = gameCtrl.CheckMoves(board);

            foreach (int move in possibleMoves)
            {
                newboard = gameCtrl.GenerateBoardFromMove(board, move, turn);
                scoringMove = MinimaxNextMove(newboard, depth + 1);
                currentScore = -scoringMove;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = move;
                }
            }
            scoringMove = gameCtrl.BestMove(board, turn);
        }
        return scoringMove;
    }
}

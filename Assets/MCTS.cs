/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS : AIAbstract
{
    [SerializeField] private int targetFrameRate = 60;

    private float timeStamp = default;
    private float averageTimePerFrame = default;
    private int bestNextMove;
    private List<Node> exploredNodes = new List<Node>();
    private Node currentRoot = null;

    private class Node
    {
        public int[,] gameBoard;
        public Node parentNode;
        public Node[] children = null;
        public int numberOfSearchs;
        public int numberOfWins;
    }

    public override int NextMove(int[,] board)
    {
        return bestNextMove;
    }

    private void Awake()
    {
        SetInstanceVariables();
    }

    private void SetInstanceVariables()
    {
        averageTimePerFrame = 1.0f / targetFrameRate;
    }

    private void Update()
    {
        bestNextMove = Search();
    }

    private int Search()
    {
        timeStamp = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup > timeStamp + averageTimePerFrame)
        {
            Node leaf = Select(gameCtrl.mainBoard);

        }

        return BestChild(gameCtrl.mainBoard);
    }

    private Node FindNodeFromBoard(int[,] board)
    {
        return exploredNodes.Find(item => item.gameBoard == board);
    }

    private Node Select(Node node)
    {
        while (node.children != null)
        {
            node
        }
        return node;
    }

    private Node UnVisitedNode(List<Node> nodes)
    {
        return nodes.Find(searchCount => );
    }

    private int[,] Rollout(int[,] leaf)
    {
        return leaf;
    }

    private void BackPropagate(Node root, Node nodeToExplore, bool isWin)
    {
        nodeToExplore.numberOfSearchs++;
        nodeToExplore.numberOfWins += isWin ? 1 : 0;
        if (nodeToExplore == root) return;
        BackPropagate(root, nodeToExplore.parentNode, isWin);
    }

    private int BestChild(int[,] root)
    {

        return 1;
    }
}
*/
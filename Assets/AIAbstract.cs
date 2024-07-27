using UnityEngine;

public abstract class AIAbstract : MonoBehaviour
{
    [SerializeField] protected GameCTRL gameCtrl;

    public abstract int NextMove(int[,] board);
}

using UnityEngine;

public class PuzzleNumber : MonoBehaviour
{
    [SerializeField] private int number = 1;
    
    public void OnShot()
    {
        if (PuzzleWall.Instance != null)
            PuzzleWall.Instance.RegisterShot(number);
    }
}
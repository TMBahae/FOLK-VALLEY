using UnityEngine;

public class PuzzleWall : MonoBehaviour
{
    public static PuzzleWall Instance { get; private set; }
    
    [SerializeField] private int[] correctSequence = new int[] { 3, 6, 2, 4, 1, 5 };
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    
    private int progress = 0;
    private bool solved = false;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void RegisterShot(int number)
    {
        if (solved) return;
        
        if (number == correctSequence[progress])
        {
            progress++;
            if (progress >= correctSequence.Length)
                Solve();
        }
        else if (number == correctSequence[0])
        {
            progress = 1;
        }
        else
        {
            progress = 0;
        }
    }
    
    private void Solve()
    {
        solved = true;
        
        if (objectToEnable != null)
            objectToEnable.SetActive(true);
        
        if (audioSource != null && successSound != null)
            audioSource.PlayOneShot(successSound);
    }
}
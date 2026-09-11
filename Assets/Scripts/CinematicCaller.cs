using UnityEngine;

public class CinematicCaller : MonoBehaviour
{
    [SerializeField] private CinematicSystem cinematicSystem;
    
    public void PlayCinematic(int index)
    {
        if (cinematicSystem != null)
        {
            cinematicSystem.PlayCinematic(index);
        }
    }
}
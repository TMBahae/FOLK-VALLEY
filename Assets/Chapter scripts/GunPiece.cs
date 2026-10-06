using UnityEngine;

public class GunPiece : Interactable
{
    [SerializeField] private NotificationSystem notificationSystem;
    [SerializeField] private GameObject gunObject;
    [SerializeField] private string pieceName = "Gun Piece";
    
    private static int piecesCollected = 0;
    private static int totalPieces = 6;
    
    public override void Interact()
    {
        piecesCollected++;
        
        if (notificationSystem != null)
        {
            notificationSystem.ShowNotification("Picked up " + pieceName + " (" + piecesCollected + "/" + totalPieces + ")");
        }
        
        if (piecesCollected >= totalPieces)
        {
            if (notificationSystem != null)
            {
                notificationSystem.ShowNotification("All pieces collected! Gun assembled!", "#b1fc03");
            }
            
            if (gunObject != null)
            {
                gunObject.SetActive(true);
            }
        }
        
        gameObject.SetActive(false);
    }
    
    public static void ResetPieces()
    {
        piecesCollected = 0;
    }
}
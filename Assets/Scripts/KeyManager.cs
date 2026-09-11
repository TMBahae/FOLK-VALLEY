using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static bool KeysFound { get; private set; } = false;
    
    public void FoundKeys()
    {
        KeysFound = true;
        
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleportTrigger : MonoBehaviour
{
    public string targetSceneName = "Chapter2";
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            FirstPersonController controller = other.GetComponent<FirstPersonController>();
            if (controller != null)
            {
                PlayerSceneData.Position = other.transform.position;
                PlayerSceneData.Rotation = other.transform.rotation;
                PlayerSceneData.CameraPitch = controller.GetVerticalRotation();
                PlayerSceneData.HasData = true;

                SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}
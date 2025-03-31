using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool IsChargeShotActive = false;
    public static int CurrentRevolutions = 0;
    public static int playerHealth = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        IsChargeShotActive = false;
        CurrentRevolutions = 0;
    }
}

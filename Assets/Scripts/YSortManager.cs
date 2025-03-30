using System.Collections.Generic;
using UnityEngine;

public class YSortManager : MonoBehaviour
{
    public static YSortManager Instance { get; private set; }
    public static bool InstanceExists => Instance != null;

    [Header("Sorting Layer Names")]
    public string behindPlayerLayer = "BehindPlayer";
    public string inFrontOfPlayerLayer = "InFrontOfPlayer";

    private List<SortableObject> sortableObjects = new List<SortableObject>();
    private SortableObject playerSortable;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetPlayerReference(SortableObject player)
    {
        playerSortable = player;
    }

    public void ClearPlayerReference(SortableObject player)
    {
        if (playerSortable == player)
            playerSortable = null;
    }

    public void Register(SortableObject obj)
    {
        if (!sortableObjects.Contains(obj))
            sortableObjects.Add(obj);
    }

    public void Unregister(SortableObject obj)
    {
        sortableObjects.Remove(obj);
    }

    void LateUpdate()
    {
        if (playerSortable == null || playerSortable.spriteRenderer == null)
            return;

        float playerY = playerSortable.GetSortY();

        foreach (var obj in sortableObjects)
        {
            if (obj == null || obj.spriteRenderer == null)
                continue;

            // Don't re-sort the player — they already have the correct layer
            if (obj == playerSortable)
                continue;

            float objY = obj.GetSortY();
            playerY = playerSortable.GetSortY();

            string sortingLayer = obj.sortAgainstPlayer
                ? (playerY < objY ? behindPlayerLayer : inFrontOfPlayerLayer)
                : inFrontOfPlayerLayer;

            obj.spriteRenderer.sortingLayerName = sortingLayer;

            // Optional: sortingOrder for smooth stacking inside layer
            obj.spriteRenderer.sortingOrder = Mathf.RoundToInt(-objY * 100);
        }
    }
}

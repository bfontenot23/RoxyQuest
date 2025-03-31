using UnityEngine;

public class OrbStaffVisual : MonoBehaviour
{
    [SerializeField] private GameObject orbVisual;

    void Start()
    {
        orbVisual.SetActive(false);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) orbVisual.SetActive(true);
        else if(Input.GetMouseButtonUp(0)) orbVisual.SetActive(false);
    }
}

using UnityEngine;

public class StaffShoot : MonoBehaviour
{
    [SerializeField] private Transform spawnOrb;
    [SerializeField] private GameObject orb;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newOrb = Instantiate(orb, spawnOrb.position, spawnOrb.rotation);
            newOrb.transform.SetParent(spawnOrb);
        }
    }
}

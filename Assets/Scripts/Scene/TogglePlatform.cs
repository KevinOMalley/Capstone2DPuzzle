using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlatform : MonoBehaviour
{
    [SerializeField]
    GameObject[] platforms1;  // The first group of platforms that are active, active while platforms2 platforms are inactive 
    [SerializeField]          
    GameObject[] platforms2;  // Second to activate, is activated when platforms in platforms1 are inactive

    [SerializeField]
    float cycleTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TogglePlatforms());
    }

    IEnumerator TogglePlatforms()
    {
        while(true)
        {
            SetActiveGroup(platforms1, true);
            SetActiveGroup (platforms2, false);
            
            yield return new WaitForSeconds(cycleTime);

            SetActiveGroup(platforms1, false);
            SetActiveGroup(platforms2 , true);

            yield return new WaitForSeconds(cycleTime);
        }

    }

    void SetActiveGroup(GameObject[] platforms, bool isActive)
    {
        foreach(GameObject p in platforms)
        {
            p.SetActive(isActive);
        }
    }
    
}

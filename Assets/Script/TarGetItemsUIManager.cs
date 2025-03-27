using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarGetItemsUIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> TarGetItemspos;
    [SerializeField] List<GameObject> TarGetItems;

    [SerializeField] GameManager gameManager;

    private void Start()
    {
     
    }

    public void SetTarget()
    {
        for (int i = 0; i < gameManager.GoalTableNumber.Count; i++)
        {
            Target target = gameManager.TarGetItems[gameManager.target].GetComponent<Target>();
            GameObject gameObject = Instantiate(TarGetItems[gameManager.GoalTableNumber[i]], TarGetItemspos[i].transform);
            gameObject.transform.tag = "Untagged";
            GameObject targetCostspos = TarGetItemspos[i].transform.Find("targetCosts").gameObject;
            for (int j = 0; j < target.targetCosts.Count; j++)
            {
                Instantiate(target.targetCosts[j], targetCostspos.transform);
            }
        }
    }
}

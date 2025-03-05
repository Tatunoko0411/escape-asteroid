using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public GameObject ItemButton;
    public GameObject RecipeButton;
    [SerializeField] GameObject menupanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackMenu()
    {
        menupanel.SetActive(false);
        ItemButton.SetActive(false);
        RecipeButton.SetActive(false);
        this.gameObject.SetActive(false);
    }

}

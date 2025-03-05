using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Button Button;
    public GameObject ItemButton;
    public GameObject RecipeButton;
    public GameObject BackButton;
    [SerializeField] GameObject menupanel;

    // Start is called before the first frame update
    void Start()
    {
        ItemButton.SetActive(false);
        RecipeButton.SetActive(false);
        BackButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void displayMenu()
    {
        menupanel.SetActive(true);
        ItemButton.SetActive(true);
        RecipeButton.SetActive(true);
        BackButton.SetActive(true);
    }
}

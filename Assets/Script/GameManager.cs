using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] public List<GameObject> trouts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDirection()
    {
        GameObject.Find($"Player").GetComponent<Player>().direction = - GameObject.Find($"Player").GetComponent<Player>().direction ;
    }
}

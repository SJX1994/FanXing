using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActiveBinding : MonoBehaviour
{
    [SerializeField]
    GameObject GO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Image>().enabled = GO.activeSelf;
    }
}

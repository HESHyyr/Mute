using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = transform.Find("Health Bar").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(10, player.GetComponent<PlayerController>().playerHealth);
    }

}

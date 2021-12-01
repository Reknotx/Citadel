using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSpawnerScript : MonoBehaviour
{

    public Transform pos;

    public GameObject mineCartPrefab;

    public int currentCarts;
    public int trackedCarts;


    // Start is called before the first frame update
    void Awake()
    {
        currentCarts = trackedCarts;
    }

    // Update is called once per frame
    void Update()
    {
        var tracker = GameObject.FindGameObjectWithTag("GoldTracker");
        trackedCarts = tracker.GetComponent<PlayerStatTrackerScript>().numCart;

        if(currentCarts < trackedCarts)
        {
            spawnCart();
            currentCarts ++;
        }
    }


    public void spawnCart()
    {
        var mineCart = (GameObject)Instantiate(this.gameObject.GetComponent<CartSpawnerScript>().mineCartPrefab, pos.transform.position, pos.transform.rotation);
        pos.transform.position += new Vector3(0, 5, 0);
    }
}

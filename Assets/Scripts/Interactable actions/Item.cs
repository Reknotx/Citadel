
/*
 * Author: Chase O'Connor
 * Date: 10/5/2021
 * 
 * Brief: The base class for all Items in the game. If it's an
 * item it also needs to override and CALL the base function here in
 * Item.
 * 
 * 
 */

using System.Collections;
using UnityEngine;

namespace Interactables
{

    public abstract class Item : Interactable
    {

        

        public bool grounded = false;

        public void Awake()
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 200, 0));
            StartCoroutine(pickUpDelay());
        }



        /// <summary>
        /// The base interact function for all items which destroys them.
        /// </summary>
        public override void Interact()
        {
            Destroy(gameObject);
        }


        public void Update()
        {


            
        }

        public IEnumerator pickUpDelay()
        {
            float delayTime = 1.5f;
            yield return new WaitForSeconds(delayTime);
            grounded = true;
        }
    }
}

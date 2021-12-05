
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
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{

    public abstract class Item : Interactable
    {

        ///<summary>This is the range of detection to the ground.</summary>
        private float _Reach = 1f;

        ///<summary>This tracks what the ground detection raycast hits.</summary>
        private RaycastHit hit;

        public bool grounded = false;

        public void Awake()
        {
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 200, 0));
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


            var groundCheck = transform.TransformDirection(Vector3.down);
            Debug.DrawRay(transform.position, groundCheck * _Reach, Color.red);
            if (Physics.Raycast(transform.position, groundCheck, out hit, _Reach) && hit.transform.tag != "Player")
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
    }
}

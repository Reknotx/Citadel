using UnityEngine;

using Map;


namespace Interactables
{

    public class Compass :Interactable
    {
        [Tooltip("Toggle this to activate the item's special effect.")]
        public bool triggerEffect = false;

        void Update()
        {
            if (triggerEffect)
            {
                Interact();
                Destroy(gameObject);
            }
        }


        public override void Interact()
        {
            MapGenerator.Instance.ExposeSpecialRooms();
        }
    }
}

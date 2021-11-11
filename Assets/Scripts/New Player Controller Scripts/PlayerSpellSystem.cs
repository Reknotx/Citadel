using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellSystem : MonoBehaviour
{

    public GameObject spellOne;
    public GameObject spellTwo;
    public GameObject spellThree;


    public List<GameObject> acquiredSpells;


    public void CastSpell(string spellSlot)
    {
        GameObject spellToCast = null;


        switch (spellSlot)
        {
            case "spellOne":
                spellToCast = spellOne;
                break;

            case "spellTwo":
                spellToCast = spellTwo;
                break;

            case "spellThree":
                spellToCast = spellThree;
                break;

            default:
                Debug.LogError("Input system setup incorrectly for spell system");
                return;
        }

        ///Apply force to spell or perform unique movement math
        Instantiate(spellToCast);

    }

    public void SwapSpell(GameObject spell, int spellSlot)
    {
        switch (spellSlot)
        {
            case 1:
                spellOne = spell;
                break;

            case 2:
                spellTwo = spell;
                break;

            case 3:
                spellThree = spell;
                break;
            
            default:
                break;
        }
    }

    /// <summary>
    /// Adds a new spell into the book.
    /// </summary>
    /// <param name="newSpell">The spell prefab to add.</param>
    public void AddSpellToBook(GameObject newSpell)
    {
        //if (newSpell.GetComponent<Spell>() == null)
        //    Debug.LogError(newSpell.name + " does not have a spell script attached.");

        acquiredSpells.Add(newSpell);
    }



}
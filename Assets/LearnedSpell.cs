using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnedSpell : MonoBehaviour
{
    private GameObject _spell;
    public GameObject Spell
    {
        get {  return _spell; }
        set
        {
            _spell = value;
            DisplayImage.sprite = value.GetComponent<Spell>().spellUIImage;
            nameText.text = value.name;
        }
    }

    public Image DisplayImage;

    public Text nameText;
}

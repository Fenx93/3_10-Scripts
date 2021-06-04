using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    [SerializeField] private string charName;
    [SerializeField] private Sprite charImg;

    public string GetName() { return charName; }

    public Sprite GetSprite() { return charImg; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class TestCut : MonoBehaviour
{
    public ObiRope Rope;

    private void Start()
    {
        // int lol = Rope.
        ObiStructuralElement elem = Rope.elements[10];
        Rope.Tear(elem);
        Rope.RebuildConstraintsFromElements();
        // Rope.
    }
}

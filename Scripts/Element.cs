using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Element
{
    public Vector3 Position;

    public Element(Vector3 position)
    {
        Position = position;
    }

    public void Draw()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Position, 0.2f);
    }
}

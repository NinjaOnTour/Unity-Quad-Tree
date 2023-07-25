using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Element
{
    public Vector3 Position;
    public static Chunk Tree;
    public static float SideLenght;
    public Chunk Parent;

    private float speed = 3f;
    private float angle = 0f;
    

    public Element(Vector3 position)
    {
        Position = position;
        angle = Random.Range(0f, 360f);
        Parent = Tree;
    }

    public void SetAngle(float value) 
    {
        if (value < 0f) value += 360f; else if (value > 360f) value -= 360f;
        angle = value;
    }

    public float GetAngle() { return angle; }

    public void Move()
    {
        angle += Random.Range(-2f, 2f);
        Position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed * Time.deltaTime;

        Position = new Vector3(Mathf.Clamp(Position.x, -SideLenght / 2f, SideLenght / 2f), Mathf.Clamp(Position.y, -SideLenght / 2f, SideLenght / 2f));
        if (Mathf.Abs(Position.x) == SideLenght / 2f || Mathf.Abs(Position.y) == SideLenght / 2f)
        {
            SetAngle(angle + 90f);
        }

        if (!Parent.Contains(this))
        {
            Parent.Remove(this);
            Tree.Add(this);
        }
    }

    public void Draw()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Position, 0.2f);
    }
}

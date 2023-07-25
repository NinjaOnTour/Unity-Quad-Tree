using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Element> Points;
    public int PointCount;
    public float SideLenght;

    public Chunk Tree;


    private void Start()
    {
        float halfSideLenght = SideLenght / 2f;
        for (int i = 0; i < PointCount; i++)
        {
            Points.Add(new Element(new Vector3(Random.Range(-halfSideLenght, halfSideLenght), Random.Range(-halfSideLenght, halfSideLenght), Random.Range(-halfSideLenght, halfSideLenght))));
        }

        Tree = new Chunk(SideLenght, Vector3.zero, null);
        Tree.Add(Points.ToArray());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Tree.Draw();
    }
}

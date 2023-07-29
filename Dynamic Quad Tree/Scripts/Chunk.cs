using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable]
public class Chunk
{
    public const int MaxElementCount = 5;
    public const int MaxLayer = 4;

    public int Layer; // Generation of chunk. first chunk's layer is 0 first chunk's children's layer is 1. Every generation layer increases
    public float SideLenght;
    public float HalfSideLenght; // = SideLenght/2
    public List<Element> Elements; // includes only its own elements
    public Vector3[] Corners; // 0->Up Left  1->Up Right  2->Down Left  3->Down Right
    public Vector3 Center;
    public Chunk Parent;
    public Chunk[] Children;

    public bool isParent
    {
        get
        {
            return Elements == null;
        }
    }
    

    public Chunk(float sideLenght, Vector3 center, Chunk parent)
    {
        SideLenght = sideLenght;
        HalfSideLenght = sideLenght / 2f;
        Center = center;
        Corners = new Vector3[4];
        Corners[0] = new Vector3(-HalfSideLenght, HalfSideLenght) + center;
        Corners[1] = new Vector3(HalfSideLenght, HalfSideLenght) + center;
        Corners[2] = new Vector3(-HalfSideLenght, -HalfSideLenght) + center;
        Corners[3] = new Vector3(HalfSideLenght, -HalfSideLenght) + center;
        Elements = new List<Element>();
        Parent = parent;
        if (parent == null) Layer = 0; else Layer = parent.Layer + 1;
    }

    public void Split()
    {
        if (Layer < MaxLayer) 
        {
            Children = new Chunk[4];
            float QuarterSideLenght = SideLenght / 4f;
            Children[0] = new Chunk(HalfSideLenght, Center + new Vector3(-QuarterSideLenght, QuarterSideLenght), this);
            Children[1] = new Chunk(HalfSideLenght, Center + new Vector3(QuarterSideLenght, QuarterSideLenght), this);
            Children[2] = new Chunk(HalfSideLenght, Center + new Vector3(-QuarterSideLenght, -QuarterSideLenght), this);
            Children[3] = new Chunk(HalfSideLenght, Center + new Vector3(QuarterSideLenght, -QuarterSideLenght), this);

            for (int i = 0; i < 4; i++)
            {
                Children[i].Add(Elements.ToArray());
            }

            Elements = null;
        }
    }

    public void Add(Element element)
    {
        if (!Contains(element)) return;

        if (isParent) 
        {
            for (int i = 0; i < 4; i++)
            {
                Children[i].Add(element);
            }
        }
        else
        {
            Elements.Add(element);
            element.Parent = this;
            if (Elements.Count > MaxElementCount)
            {
                Split();
            }
        }
    }

    public void Add(Element[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (Contains(elements[i]))
            { 
                Elements.Add(elements[i]);
                elements[i].Parent = this;
            }
        }

        if (Elements.Count > MaxElementCount)
        {
            Split();
        }
    }

    public bool Contains(Element element) // is element in the area of chunk
    {
        Vector3 pos = element.Position;
        return pos.x >= Corners[0].x && pos.x <= Corners[1].x && pos.y >= Corners[3].y && pos.y <= Corners[1].y;
    }

    public void Remove(Element element)
    {
        if(isParent)
        {
            for (int i = 0; i < 4; i++)
            {
                Children[i].Remove(element);
            }
        }
        else if (Elements.Contains(element))
        {
            Elements.Remove(element);
            element.Parent = null;
            if (Parent != null)
            {
                if (Parent.GetAllElements().Count <= MaxElementCount)
                {
                    Parent.Unite();
                }
            }
        }
    }

    public void Unite() // Removes children
    {
        List<Element> allElements = GetAllElements();
        Elements = new List<Element>();
        Elements.AddRange(allElements);
        Children = null;
    }

    public List<Element> GetAllElements() // Returns all elements. Includes children's and grandchildren's elements.
    {
        if (!isParent) return Elements;

        List<Element> list = new List<Element>();

        for (int i = 0; i < 4; i++)
        {
            list.AddRange(Children[i].GetAllElements());
        }

        return list;
    } 

    public void Draw()
    {
        Gizmos.color = Color.white;
        if(isParent)
        {
            for (int i = 0; i < Children.Length; i++)
            {
                Children[i].Draw();
            }
        }
        else
        {
            Gizmos.DrawLine(Corners[0], Corners[1]);
            Gizmos.DrawLine(Corners[1], Corners[3]);
            Gizmos.DrawLine(Corners[3], Corners[2]);
            Gizmos.DrawLine(Corners[2], Corners[0]);
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw();
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class FloatComparer : IComparer<float>
{
    private bool reverseSort;
    public FloatComparer(bool reverseSort)
    {
        this.reverseSort = reverseSort;
    }
    public int Compare(float x, float y)
    {
        int comp = (int)Mathf.Clamp(x - y, -1, 1);
        return reverseSort ? -comp : comp;
    }
}
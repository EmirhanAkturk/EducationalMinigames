using System.Collections.Generic;
using UnityEngine;

public class IntegerComparer : IComparer<int>
{
    private bool reverseSort;
    public IntegerComparer(bool reverseSort)
    {
        this.reverseSort = reverseSort;
    }
    public int Compare(int x, int y)
    {
        int comp = Mathf.Clamp(x - y, -1, 1);
        return reverseSort ? -comp : comp;
    }
}
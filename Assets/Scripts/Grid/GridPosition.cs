//struct value type
//vector2Int would wor as well but due to the fact taht it works with x and y and we need x and z its easier and cleaner to create ones own struct
//own name is easier to unterstand
using System;
using UnityEngine;

public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.z = z;
        this.x = x;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
        x == position.x &&
        z == position.z;
    }

    public bool Equals(GridPosition other)
    {
       return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x,z);
    }

    public override string ToString()
    {
        return "" + x + " " +z;
    }

    public static bool operator == (GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

      public static bool operator != (GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator + (GridPosition a, GridPosition b)
    {
        return new GridPosition (a.x + b.x, a.z + b.z);
    }

    public static GridPosition operator - (GridPosition a, GridPosition b)
    {
        return new GridPosition (a.x - b.x, a.z - b.z);
    }

}
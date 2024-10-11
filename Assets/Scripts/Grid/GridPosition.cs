//struct value type
//vector2Int would wor as well but due to the fact taht it works with x and y and we need x and z its easier and cleaner to create ones own struct
//own name is easier to unterstand
public struct GridPosition
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.z = z;
        this.x = x;
    }

    public override string ToString()
    {
        return "" + x + " " +z;
    }
}
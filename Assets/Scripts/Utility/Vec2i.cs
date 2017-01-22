using UnityEngine;

public struct Vec2i
{
    public Vec2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x;
    public int y;

    public static Vec2i Zero = new Vec2i(0, 0);
    public static Vec2i Down = new Vec2i(0, -1);
    public static Vec2i Up = new Vec2i(0, 1);
    public static Vec2i Left = new Vec2i(-1, 0);
    public static Vec2i Right = new Vec2i(1, 0);

    public static Vec2i toVec2i(Vector2 a)
    {
        return new Vec2i((int)a.x, (int)a.y);
    }

    public static Vec2i operator *(Vec2i a, int scale)
    {
        return new Vec2i(a.x * scale, a.y * scale);
    }

    public static Vec2i operator +(Vec2i a, Vec2i b)
    {
        return new Vec2i(a.x + b.x, a.y + b.y);
    }

    public static Vector2 operator +(Vec2i a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }
}

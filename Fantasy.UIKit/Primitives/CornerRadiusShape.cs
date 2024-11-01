#region

using System.Diagnostics.CodeAnalysis;
using Math = System.Math;

#endregion

namespace Fantasy.UIKit.Primitives;

/// <summary>
///     定义圆角形状
/// </summary>
/// <param name="topLeft">左上角</param>
/// <param name="topRight">右上角</param>
/// <param name="bottomLeft">左下角</param>
/// <param name="bottomRight">右下角</param>
public struct CornerRadiusShape(float topLeft, float topRight, float bottomLeft, float bottomRight)
{
    public readonly float TopLeft { get; } = topLeft;
    public readonly float TopRight { get; } = topRight;
    public readonly float BottomLeft { get; } = bottomLeft;
    public readonly float BottomRight { get; } = bottomRight;

    /// <summary>
    ///     统一圆角
    /// </summary>
    /// <param name="uniformCornerRadius">圆角值</param>
    public CornerRadiusShape(float uniformCornerRadius) : this(uniformCornerRadius, uniformCornerRadius,
        uniformCornerRadius, uniformCornerRadius)
    {
    }

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return TopLeft;
                case 1: return TopRight;
                case 2: return BottomLeft;
                case 3: return BottomRight;
                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public static readonly CornerRadiusShape None = 0;
    public static readonly CornerRadiusShape ExtraSmall = 4;
    public static readonly CornerRadiusShape ExtraSmallTop = new(4, 4, 0, 0);
    public static readonly CornerRadiusShape Small = 8;
    public static readonly CornerRadiusShape Medium = 12;
    public static readonly CornerRadiusShape Large = 16;
    public static readonly CornerRadiusShape LargeTop = new(16, 16, 0, 0);
    public static readonly CornerRadiusShape LargeEnd = new(0, 0, 16, 16);
    public static readonly CornerRadiusShape ExtraLarge = 28;
    public static readonly CornerRadiusShape ExtraLargeTop = new(28, 28, 0, 0);
    public static readonly CornerRadiusShape Full = -1;

    /// <summary>
    ///     根据<see cref="width" />和<see cref="height" />获得圆角大小
    /// </summary>
    /// <param name="width">图形的宽度</param>
    /// <param name="height">图形的长度</param>
    /// <returns>返回圆角大小</returns>
    public double[] GetCornerRadius(float width, float height)
    {
        if (TopLeft is -1 && TopRight is -1 && BottomLeft is -1 && BottomRight is -1)
        {
            var full = Math.Min(width, height) / 2;
            return [full, full, full, full];
        }

        return [TopLeft, TopRight, BottomLeft, BottomRight];
    }


    public static implicit operator CornerRadiusShape(float uniformCornerRadius) => new(uniformCornerRadius);

    public static bool operator ==(CornerRadiusShape left, CornerRadiusShape right)
    {
        return left.TopLeft == right.TopLeft &&
               left.TopRight == right.TopRight &&
               left.BottomLeft == right.BottomLeft &&
               left.BottomRight == right.BottomRight;
    }

    public static bool operator !=(CornerRadiusShape left, CornerRadiusShape right)
    {
        return left.TopLeft != right.TopLeft
               || left.TopRight != right.TopRight
               || left.BottomLeft != right.BottomLeft
               || left.BottomRight != right.BottomRight;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"{TopLeft},{TopRight},{BottomLeft},{BottomRight}";
    }
}
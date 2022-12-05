using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Cysharp.Text;

namespace ZBase.Foundation
{
    /// <summary>
    /// <para>
    /// This class implements <see cref="TryFormat"/> methods for some data structures in the <see cref="UnityEngine"/> namespace,
    /// and will automatically register them to <see cref="Cysharp"/>.<see cref="Cysharp.Text"/>.<see cref="Utf16ValueStringBuilder"/>.
    /// </para>
    /// <para>Currently supported:</para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Bounds"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="BoundsInt"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Color"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Color32"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Matrix4x4"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Quaternion"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Rect"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="RectInt"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Resolution"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Vector2"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Vector2Int"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Vector3"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Vector3Int"/></para>
    /// <para>- <see cref="UnityEngine"/>.<see cref="Vector4"/></para>
    /// </summary>
    /// <remarks>
    /// The automated process is initiated by <see cref="RuntimeInitializeOnLoadMethodAttribute"/>.
    /// </remarks>
    public static class UnityEngineZStringFormatter
    {
        [RuntimeInitializeOnLoadMethod]
        static void RegisterForamtters()
        {
            Utf16ValueStringBuilder.RegisterTryFormat<Bounds>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<BoundsInt>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Color>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Color32>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Matrix4x4>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Quaternion>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Rect>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<RectInt>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Resolution>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Vector2>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Vector2Int>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Vector3>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Vector3Int>(TryFormat);
            Utf16ValueStringBuilder.RegisterTryFormat<Vector4>(TryFormat);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool False(out int charsWritten)
        {
            charsWritten = 0;
            return false;
        }

        public static bool TryFormat(Bounds value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (Center: _, Extents: _)

                var centerLabel = "(Center: ".AsSpan();
                centerLabel.CopyTo(destination);

                charsWritten = centerLabel.Length;
                destination = destination[charsWritten..];

                if (TryFormat(value.center, destination, out var centerLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[centerLength++] = ',';
                destination[centerLength++] = ' ';
                destination = destination[centerLength..];

                var extentsLabel = "Extents: ".AsSpan();
                extentsLabel.CopyTo(destination);

                var extentsLabelLength = extentsLabel.Length;
                destination = destination[extentsLabelLength..];

                if (TryFormat(value.extents, destination, out var extentsLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[extentsLength++] = ')';

                charsWritten += centerLength + extentsLabelLength + extentsLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(BoundsInt value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (Position: _, Size: _)

                var positionLabel = "(Position: ".AsSpan();
                positionLabel.CopyTo(destination);

                charsWritten = positionLabel.Length;
                destination = destination[charsWritten..];

                if (TryFormat(value.position, destination, out var positionLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[positionLength++] = ',';
                destination[positionLength++] = ' ';
                destination = destination[positionLength..];

                var sizeLabel = "Size: ".AsSpan();
                sizeLabel.CopyTo(destination);

                var sizeLabelLength = sizeLabel.Length;
                destination = destination[sizeLabelLength..];

                if (TryFormat(value.size, destination, out var sizeLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[sizeLength++] = ')';

                charsWritten += positionLength + sizeLabelLength + sizeLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Color value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// RGBA(r, g, b, a)

                var label = "RGBA(".AsSpan();
                label.CopyTo(destination);

                charsWritten = label.Length;
                destination = destination[charsWritten..];

                if (value.r.TryFormat(destination, out var rLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[rLength++] = ',';
                destination[rLength++] = ' ';
                destination = destination[rLength..];

                if (value.g.TryFormat(destination, out var gLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[gLength++] = ',';
                destination[gLength++] = ' ';
                destination = destination[gLength..];

                if (value.b.TryFormat(destination, out var bLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[bLength++] = ',';
                destination[bLength++] = ' ';
                destination = destination[bLength..];

                if (value.a.TryFormat(destination, out var aLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[aLength++] = ')';

                charsWritten += rLength + gLength + bLength + aLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Color32 value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// RGBA(r, g, b, a)

                var label = "RGBA(".AsSpan();
                label.CopyTo(destination);

                charsWritten = label.Length;
                destination = destination[charsWritten..];

                if (value.r.TryFormat(destination, out var rLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[rLength++] = ',';
                destination[rLength++] = ' ';
                destination = destination[rLength..];

                if (value.g.TryFormat(destination, out var gLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[gLength++] = ',';
                destination[gLength++] = ' ';
                destination = destination[gLength..];

                if (value.b.TryFormat(destination, out var bLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[bLength++] = ',';
                destination[bLength++] = ' ';
                destination = destination[bLength..];

                if (value.a.TryFormat(destination, out var aLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[aLength++] = ')';

                charsWritten += rLength + gLength + bLength + aLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Matrix4x4 value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// {m00} {m01} {m02} {m03}
                /// {m10} {m11} {m21} {m13}
                /// {m20} {m21} {m22} {m23}
                /// {m30} {m31} {m32} {m33}

                if (value.m00.TryFormat(destination, out var m00Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m00Length++] = '\t';
                destination = destination[m00Length..];

                if (value.m01.TryFormat(destination, out var m01Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m01Length++] = '\t';
                destination = destination[m01Length..];

                if (value.m02.TryFormat(destination, out var m02Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m02Length++] = '\t';
                destination = destination[m02Length..];

                if (value.m03.TryFormat(destination, out var m03Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m03Length++] = '\n';
                destination = destination[m03Length..];

                if (value.m10.TryFormat(destination, out var m10Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m10Length++] = '\t';
                destination = destination[m10Length..];

                if (value.m11.TryFormat(destination, out var m11Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m11Length++] = '\t';
                destination = destination[m11Length..];

                if (value.m12.TryFormat(destination, out var m12Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m12Length++] = '\t';
                destination = destination[m12Length..];

                if (value.m13.TryFormat(destination, out var m13Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m13Length++] = '\n';
                destination = destination[m13Length..];

                if (value.m20.TryFormat(destination, out var m20Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m20Length++] = '\t';
                destination = destination[m20Length..];

                if (value.m21.TryFormat(destination, out var m21Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m21Length++] = '\t';
                destination = destination[m21Length..];

                if (value.m22.TryFormat(destination, out var m22Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m22Length++] = '\t';
                destination = destination[m22Length..];

                if (value.m23.TryFormat(destination, out var m23Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m23Length++] = '\n';
                destination = destination[m23Length..];

                if (value.m30.TryFormat(destination, out var m30Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m30Length++] = '\t';
                destination = destination[m30Length..];

                if (value.m31.TryFormat(destination, out var m31Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m31Length++] = '\t';
                destination = destination[m31Length..];

                if (value.m32.TryFormat(destination, out var m32Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m32Length++] = '\t';
                destination = destination[m32Length..];

                if (value.m33.TryFormat(destination, out var m33Length, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[m33Length++] = '\n';

                charsWritten = m00Length + m01Length + m02Length + m03Length
                             + m10Length + m11Length + m12Length + m13Length
                             + m20Length + m21Length + m22Length + m23Length
                             + m30Length + m31Length + m32Length + m33Length
                             ;

                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Quaternion value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x, y, z, w)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ',';
                destination[yLength++] = ' ';
                destination = destination[yLength..];

                if (value.z.TryFormat(destination, out var zLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[zLength++] = ',';
                destination[zLength++] = ' ';
                destination = destination[zLength..];

                if (value.w.TryFormat(destination, out var wLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[wLength++] = ')';

                charsWritten += xLength + yLength + zLength + wLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Rect value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x: _, y: _, width: _, height: _)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination[charsWritten++] = 'x';
                destination[charsWritten++] = ':';
                destination[charsWritten++] = ' ';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination[xLength++] = 'y';
                destination[xLength++] = ':';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ',';
                destination[yLength++] = ' ';
                destination = destination[yLength..];

                var widthLabel = "width: ".AsSpan();
                widthLabel.CopyTo(destination);

                var widthLabelLength = widthLabel.Length;
                destination = destination[widthLabelLength..];

                if (value.width.TryFormat(destination, out var widthLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[widthLength++] = ',';
                destination[widthLength++] = ' ';
                destination = destination[widthLength..];

                var heightLabel = "height: ".AsSpan();
                heightLabel.CopyTo(destination);

                var heightLabelLength = heightLabel.Length;
                destination = destination[heightLabelLength..];

                if (value.height.TryFormat(destination, out var heightLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[heightLength++] = ')';

                charsWritten += xLength + yLength + widthLabelLength + widthLength + heightLabelLength + heightLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(RectInt value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x: _, y: _, width: _, height: _)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination[charsWritten++] = 'x';
                destination[charsWritten++] = ':';
                destination[charsWritten++] = ' ';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination[xLength++] = 'y';
                destination[xLength++] = ':';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ',';
                destination[yLength++] = ' ';
                destination = destination[yLength..];

                var widthLabel = "width: ".AsSpan();
                widthLabel.CopyTo(destination);

                var widthLabelLength = widthLabel.Length;
                destination = destination[widthLabelLength..];

                if (value.width.TryFormat(destination, out var widthLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[widthLength++] = ',';
                destination[widthLength++] = ' ';
                destination = destination[widthLength..];

                var heightLabel = "height: ".AsSpan();
                heightLabel.CopyTo(destination);

                var heightLabelLength = heightLabel.Length;
                destination = destination[heightLabelLength..];

                if (value.height.TryFormat(destination, out var heightLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[heightLength++] = ')';

                charsWritten += xLength + yLength + widthLabelLength + widthLength + heightLabelLength + heightLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Resolution value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (width x height @ refreshRate Hz)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination = destination[charsWritten..];

                if (value.width.TryFormat(destination, out var widthLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[widthLength++] = ' ';
                destination[widthLength++] = 'x';
                destination[widthLength++] = ' ';
                destination = destination[widthLength..];

                if (value.height.TryFormat(destination, out var heightLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[heightLength++] = ' ';
                destination[heightLength++] = '@';
                destination[heightLength++] = ' ';
                destination = destination[heightLength..];

                if (value.refreshRate.TryFormat(destination, out var refreshRateLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[refreshRateLength++] = 'H';
                destination[refreshRateLength++] = 'z';
                destination[refreshRateLength++] = ')';

                charsWritten += widthLength + heightLength + refreshRateLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Vector2 value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x, y)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ')';

                charsWritten += xLength + yLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Vector2Int value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x, y)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ')';

                charsWritten += xLength + yLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Vector3 value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x, y, z)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ',';
                destination[yLength++] = ' ';
                destination = destination[yLength..];

                if (value.z.TryFormat(destination, out var zLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[zLength++] = ')';

                charsWritten += xLength + yLength + zLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Vector3Int value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x, y, z)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ',';
                destination[yLength++] = ' ';
                destination = destination[yLength..];

                if (value.z.TryFormat(destination, out var zLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[zLength++] = ')';

                charsWritten += xLength + yLength + zLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }

        public static bool TryFormat(Vector4 value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format)
        {
            try
            {
                /// (x, y, z, w)

                charsWritten = 0;

                destination[charsWritten++] = '(';
                destination = destination[charsWritten..];

                if (value.x.TryFormat(destination, out var xLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[xLength++] = ',';
                destination[xLength++] = ' ';
                destination = destination[xLength..];

                if (value.y.TryFormat(destination, out var yLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[yLength++] = ',';
                destination[yLength++] = ' ';
                destination = destination[yLength..];

                if (value.z.TryFormat(destination, out var zLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[zLength++] = ',';
                destination[zLength++] = ' ';
                destination = destination[zLength..];

                if (value.w.TryFormat(destination, out var wLength, format) == false)
                {
                    return False(out charsWritten);
                }

                destination[wLength++] = ')';

                charsWritten += xLength + yLength + zLength + wLength;
                return true;
            }
            catch
            {
                return False(out charsWritten);
            }
        }
    }
}

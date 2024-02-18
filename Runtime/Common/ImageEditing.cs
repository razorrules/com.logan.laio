
using UnityEngine;

namespace Laio
{
    public static class ImageEditing
    {

        public static void ApplyOutline(this Texture2D texture, int size, int offset, Color color)
        {
            int x1 = offset;
            int x2 = offset + size;
            int x3 = texture.width - (offset + size);
            int x4 = texture.width - offset;

            int y1 = offset;
            int y2 = offset + size;
            int y3 = texture.width - (offset + size);
            int y4 = texture.width - offset;

            bool ValidPosition(int x, int y)
            {
                if (x <= x1 || x >= x4)
                    return false;
                if (y <= y1 || y >= y4)
                    return false;

                bool validX = (x >= x1 && x < x2) || (x >= x3 && x < x4);
                bool validY = (y >= y1 && y < y2) || (y >= y3 && y < y4);

                return validX || validY;
            }

            for (int x = 0; x < texture.width * 2; x++)
            {
                for (int y = 0; y < texture.height * 2; y++)
                {
                    if (ValidPosition(x, y))
                        texture.SetPixel(x, y, color);
                }
            }
        }

    }
}

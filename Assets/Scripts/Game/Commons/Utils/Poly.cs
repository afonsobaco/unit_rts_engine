using UnityEngine;

namespace RTSEngine.Utils
{
    public static class Poly
    {
        public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
        {
            var j = polyPoints.Length - 1;
            var inside = false;
            for (int i = 0; i < polyPoints.Length; j = i++)
            {
                var pi = polyPoints[i];
                var pj = polyPoints[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                    (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;
            }
            return inside;
        }

        public static Vector2[] GetDefaultQuadrilateral(Vector2 startPoint, Vector2 endPoint)
        {
            return new Vector2[] {
                new Vector2(startPoint.x, startPoint.y),
                new Vector2(endPoint.x, startPoint.y),
                new Vector2(endPoint.x, endPoint.y),
                new Vector2(startPoint.x, endPoint.y)
            };
        }
    }
}
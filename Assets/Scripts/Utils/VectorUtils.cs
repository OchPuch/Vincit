

using UnityEngine;

namespace Utils
{
    public static class VectorUtils
    {
        public static bool IsLookingAtThePlane(Vector3 look, Vector3 planeNormal)
        {
            if (planeNormal == Vector3.zero)
            {
                return false;
            }
            //prevent dot product from returning 1 or -1
            if (look.normalized == planeNormal.normalized)
            {
                return false;
            }

            if (look.normalized == -planeNormal.normalized)
            {
                return true;
            }
            
            return !(Vector3.Dot(look, planeNormal) > 0);
        }
        
        public static bool AreCodirected(Vector2 a, Vector2 b)
        {
            return Vector2.Dot(a, b) > 1 - Mathf.Epsilon;
        }
    }
}
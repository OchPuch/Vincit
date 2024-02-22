using UnityEngine;

namespace Utils
{
    public static class LayerUtils
    {
        public static bool IsInLayerMask(int layer, LayerMask mask)
        {
            return (mask & (1 << layer)) != 0;
        }
    }
}
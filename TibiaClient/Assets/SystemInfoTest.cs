using UnityEngine;

namespace Utils
{
    public static class SystemInfoTest
    {
        public static bool Test()
        {
            return SystemInfo.supportsInstancing
                && SystemInfo.maxTextureSize >= 8192
                && SystemInfo.graphicsShaderLevel >= 40;
        }
    }
}

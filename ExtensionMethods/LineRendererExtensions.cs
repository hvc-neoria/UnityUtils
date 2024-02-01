using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class LineRendererExtensions
    {
        /// <summary>
        /// GetPositions()のワンライナー。
        /// </summary>
        /// <param name="lineRenderer">LineRenderer</param>
        /// <returns>Vector3の配列</returns>
        public static Vector3[] GetPositions(this LineRenderer lineRenderer)
        {
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(positions);
            return positions;
        }
    }
}

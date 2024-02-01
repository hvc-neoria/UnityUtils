using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class CollisionExtensions
    {
        /// <summary>
        /// 反射ベクトル。コリジョンに衝突した後の軌道。
        /// </summary>
        /// <param name="targetCollision">衝突先のコリジョン</param>
        /// <returns>反射ベクトル</returns>
        public static Vector3 ReflectionVector(this Collision targetCollision)
        {
            Vector3 inVector = targetCollision.relativeVelocity;
            Vector3 normalizedNormal = targetCollision.impulse.normalized;
            Vector3 outVector = Vector3.Reflect(-inVector, normalizedNormal);
            return outVector;
        }
    }
}

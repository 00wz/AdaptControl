using UnityEngine;

public static class Extentions
{
    private const float ERROR = 0.01f;

    /// <summary>
    /// Returns a vector with the given magnitude
    /// </summary>
    /// <param name="self"></param>
    /// <param name="magnitude">New magnitude</param>
    /// <returns></returns>
    public static Vector3 WithMagnitude(this Vector3 self, float magnitude)
    {
        return self.normalized * magnitude;
    }

    /// <summary>
    /// Moves a Rigidbody while checking for obstacles
    /// </summary>
    /// <param name="self"></param>
    /// <param name="position">Provides the new position for the Rigidbody object</param>
    public static void MovePositionSweep(this Rigidbody self, Vector3 position)
    {
        Vector3 worldOffset = position - self.position;
        if(self.SweepTest(worldOffset,out RaycastHit raycastHit,worldOffset.magnitude))
        {
            worldOffset = worldOffset.WithMagnitude(Mathf.Max(raycastHit.distance-ERROR, 0f));
            self.MovePosition(self.position + worldOffset);
            return;
        }
        self.MovePosition(position);
    }

    /// <summary>
    /// Moves a Rigidbody around obstacles
    /// </summary>
    /// <param name="self"></param>
    /// <param name="position">Provides the new position for the Rigidbody object</param>
    public static void MovePositionFlowAround(this Rigidbody self, Vector3 position)
    {
        Vector3 worldOffset = position - self.position;
        if (self.SweepTest(worldOffset, out RaycastHit raycastHit, worldOffset.magnitude))
        {
            worldOffset = worldOffset.WithMagnitude(Mathf.Max(raycastHit.distance - ERROR, 0f));
            self.MovePosition(self.position + worldOffset);
            worldOffset = position - self.position;
            worldOffset = Vector3.ProjectOnPlane(worldOffset, raycastHit.normal);
            AddWorldOffsetSweep(self, worldOffset);
            return;
        }
        self.MovePosition(position);
    }

    /// <summary>
    /// Add world offset to Rigidbody while checking for obstacles
    /// </summary>
    /// <param name="self"></param>
    /// <param name="position">Provides the new position for the Rigidbody object</param>
    public static void AddWorldOffsetSweep(this Rigidbody self, Vector3 offset)
    {
        if(self.SweepTest(offset,out RaycastHit raycastHit,offset.magnitude))
        {
            offset = offset.WithMagnitude(Mathf.Max(raycastHit.distance-ERROR, 0f));
        }
        self.MovePosition(self.position + offset);
    }

    public static T GetOrAddComponent<T>(this GameObject self) where T: Component
    {
        T component;
        if(self.TryGetComponent<T>(out component))
        {
            return component;
        }
        return self.AddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this Component self) where T: Component
    {
        T component;
        if(self.TryGetComponent<T>(out component))
        {
            return component;
        }
        return self.gameObject.AddComponent<T>();
    }
}


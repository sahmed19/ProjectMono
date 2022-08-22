using System;
using Microsoft.Xna.Framework;

public static class MonoHelper {

    public static float DeltaTime(this GameTime gameTime)
    { 
        return (float) gameTime.ElapsedGameTime.TotalSeconds;
    }
    public static float CurrentTime(this GameTime gameTime)
    { 
        return (float) gameTime.TotalGameTime.TotalSeconds;
    }

    public static Vector2 Normalized(this Vector2 vector)
    {
        if(vector.LengthSquared() < .0001f) return Vector2.Zero;
        return Vector2.Normalize(vector);
    }

    public static Vector2 ClampMagnitude(this Vector2 vector, float maxMagnitude)
    {
        if((maxMagnitude * maxMagnitude) > vector.LengthSquared()) return vector; //return the vector if below max magnitude
        vector = vector.Normalized() * MathHelper.Min(maxMagnitude, vector.Length());
        return vector;
    }

    public static System.Numerics.Vector4 MonoCol2SysVec_RGBA(this Microsoft.Xna.Framework.Color color)
    {
        return new System.Numerics.Vector4(
            color.R,
            color.G,
            color.B,
            color.A
        );
    }

    public static System.Numerics.Vector3 MonoCol2SysVec_RGB(this Microsoft.Xna.Framework.Color color)
    {
        return new System.Numerics.Vector3(
            color.R,
            color.G,
            color.B
        );
    }

    public static Microsoft.Xna.Framework.Color SysVec2MonoCol_RGBA(this System.Numerics.Vector4 vector)
    {
        return new Microsoft.Xna.Framework.Color(
            vector.X,
            vector.Y,
            vector.Z,
            vector.W
        );
    }

    public static Microsoft.Xna.Framework.Color SysVec2MonoCol_RGB(this System.Numerics.Vector3 vector)
    {
        return new Microsoft.Xna.Framework.Color(
            vector.X,
            vector.Y,
            vector.Z
        );
    }

    public static System.Numerics.Vector2 MonoVec2SysVec(this Microsoft.Xna.Framework.Vector2 vector)
    {
        return new System.Numerics.Vector2(
            vector.X,
            vector.Y
        );
    }

    public static Microsoft.Xna.Framework.Vector2 SysVec2MonoVec(this System.Numerics.Vector2 vector)
    {
        return new Microsoft.Xna.Framework.Vector2(
            vector.X,
            vector.Y
        );
    }

    public static float SmoothDamp (float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        smoothTime = Math.Max (0.0001f, smoothTime);
        float num = 2f / smoothTime;
        float num2 = num * deltaTime;
        float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
        float num4 = current - target;
        float num5 = target;
        float num6 = maxSpeed * smoothTime;
        num4 = Math.Clamp (num4, -num6, num6);
        target = current - num4;
        float num7 = (currentVelocity + num * num4) * deltaTime;
        currentVelocity = (currentVelocity - num * num7) * num3;
        float num8 = target + (num4 + num7) * num3;
        if (num5 - current > 0f == num8 > num5)
        {
            num8 = num5;
            currentVelocity = (num8 - num5) / deltaTime;
        }
        return num8;
    }

    public static Vector2 SmoothDamp (Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        smoothTime = Math.Max (0.0001f, smoothTime);
        float num = 2f / smoothTime;
        float num2 = num * deltaTime;
        float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
        Vector2 displacement = current - target;
        Vector2 targPos = target;
        float maxDistance = maxSpeed * smoothTime;
        displacement = displacement.ClampMagnitude(maxDistance);
        target = current - displacement;
        Vector2 num7 = (currentVelocity + num * displacement) * deltaTime;
        currentVelocity = (currentVelocity - num * num7) * num3;
        Vector2 num8 = target + (displacement + num7) * num3;
        if ((targPos - current).LengthSquared() > 0f == num8.LengthSquared() > targPos.LengthSquared())
        {
            num8 = targPos;
            currentVelocity = (num8 - targPos) / deltaTime;
        }
        return num8;
    }


 
}
using Microsoft.Xna.Framework;

public static class MonoHelper {

    public static float DeltaTime(this GameTime gameTime) { 
        return (float) gameTime.ElapsedGameTime.TotalSeconds;
    }
    public static float CurrentTime(this GameTime gameTime) { 
        return (float) gameTime.TotalGameTime.TotalSeconds;
    }

    public static Vector2 Normalized(this Vector2 vector)
    {
        if(vector.LengthSquared() < .0001f) return Vector2.Zero;
        return Vector2.Normalize(vector);
    }

    public static Vector2 ClampMagnitude(this Vector2 vector, float maxMagnitude) {
        if((maxMagnitude * maxMagnitude) > vector.LengthSquared()) return vector; //return the vector if below max magnitude
        vector = vector.Normalized() * MathHelper.Min(maxMagnitude, vector.Length());
        return vector;
    }

    public static System.Numerics.Vector2 MonoVec2SysVec(this Microsoft.Xna.Framework.Vector2 vector) {
        return new System.Numerics.Vector2(
            vector.X,
            vector.Y
        );
    }

    public static Microsoft.Xna.Framework.Vector2 SysVec2MonoVec(this System.Numerics.Vector2 vector) {
        return new Microsoft.Xna.Framework.Vector2(
            vector.X,
            vector.Y
        );
    }
 
}
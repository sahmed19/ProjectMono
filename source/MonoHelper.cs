using Microsoft.Xna.Framework;

public static class MonoHelper {

    public static float DeltaTime(this GameTime gameTime) { 
        return (float) gameTime.ElapsedGameTime.TotalSeconds;
    }
    public static float CurrentTime(this GameTime gameTime) { 
        return (float) gameTime.TotalGameTime.TotalSeconds;
    }

}
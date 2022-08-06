using Microsoft.Xna.Framework;

public static class MonoHelper {

    public static float DeltaTime(this GameTime gameTime) { 
        return gameTime.ElapsedGameTime.Seconds;
    }
    public static float CurrentTime(this GameTime gameTime) { 
        return gameTime.TotalGameTime.Seconds;
    }

}
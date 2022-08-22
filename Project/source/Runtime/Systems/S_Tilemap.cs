using System;
using System.Linq;
using Flecs;
using TiledCS;
using Microsoft.Xna.Framework;
using ProjectMono.Core;
using ProjectMono.Graphics;

namespace ProjectMono.Maps {

    public static class S_Tilemap
    {
        
        public static Entity InitializeMap(ProjectMonoApp game, string mapName, int xOffset, int yOffset)
        {
            string rootDir = game.RootDir;
            World world = game.World;
            var map = new TiledMap(rootDir + "/data/tilemaps/" + mapName + ".tmx");
            var tilesets = map.GetTiledTilesets(rootDir + "/data/tilesets/"); // DO NOT forget the / at the end
            var tileLayers = map.Layers.Where(x => x.type == TiledLayerType.TileLayer);

            Entity mapEntity = world.CreateEntity(mapName);

            foreach (var layer in tileLayers)
            {
                for (var i = 0; i < layer.data.Length; i++)
                {
                    var gid = layer.data[i]; // The tileset tile index

                    // Gid 0 is used to tell there is no tile set
                    if (gid == 0)
                        continue;

                    int tileFrame = gid - 1;

                    var mapTileset = map.GetTiledMapTileset(gid);
                    var tileset = tilesets[mapTileset.firstgid];

                    int column = tileFrame % tileset.Columns;
                    int row = (int)Math.Floor((double)tileFrame / (double)tileset.Columns);

                    float x = (i % map.Width);
                    float y = (float)Math.Floor(i / (double)map.Width);

                    Rectangle tilesetRec = new Rectangle(map.TileWidth * column, map.TileHeight * row, map.TileWidth, map.TileHeight);

                    var tile = world.CreateEntity("Tile (" + x + "," + y + ") " + "[" + gid + "]");
                    tile.Set(new C_Sprite(tileset.Name, map.TileWidth, map.TileHeight).SetRectangle(tileFrame));
                    tile.Set(new C_SpriteLayer(){Anchor=SpriteAnchor.TOP_LEFT, Layer=SpriteLayer.TILEMAP});
                    tile.Set(new C_Position() {Position = new Vector2(x + xOffset, y + yOffset)});
                    tile.Set(new C_Scale() {Scale=Vector2.One});

                    tile.AddParent(mapEntity);
                    
                    
                }
            }

            return mapEntity;

        }
    }

}
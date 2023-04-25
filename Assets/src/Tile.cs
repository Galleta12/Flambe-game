using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace src
{
    public class Tile : UnityEngine.Tilemaps.Tile
    {
        public enum DisplayOptions
        {
            /// <summary>Only one valid sprite for tile</summary>
            Single,
            
            /// <summary>Selects sprite depending on surrounding tiles of same type</summary>
            Directional,
            
            /// <summary>Selects one of the available sprites randomly</summary>
            Random
        }
        
        /// <summary>
        /// The method which the Tile uses to select its sprite
        /// </summary>
        public DisplayOptions displayMode;

        [SerializeField]
        private Sprite[] sprites;
        private Dictionary<string, Sprite> _spriteMap;

        /// <summary>
        /// Refreshes itself and other Tiles of the same type that are orthogonally and diagonally adjacent
        /// </summary>
        /// <param name="location">The location of the tile in the grid</param>
        /// <param name="tilemap">A reference to the tilemap</param>
        public override void RefreshTile(Vector3Int location, ITilemap tilemap)
        {
            if (displayMode == DisplayOptions.Directional)
            {
                for (int yd = -1; yd <= 1; yd++)
                for (int xd = -1; xd <= 1; xd++)
                {
                    Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                    if (HasSameTileDef(tilemap, position))
                        tilemap.RefreshTile(position);
                }
            }
            else
            {
                tilemap.RefreshTile(location);
            }
        }

        /// <summary>
        /// Determine the sprite for a specific tile (and potentially fill in other TileData items)
        /// </summary>
        /// <param name="location">The location of the tile in the grid</param>
        /// <param name="tilemap">A reference to the tilemap</param>
        /// <param name="tileData">An empty TileData object to be filled in</param>
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(location, tilemap, ref tileData);
            switch (displayMode)
            {
                case DisplayOptions.Single:
                    tileData.sprite = sprite;
                    break;
                case DisplayOptions.Random:
                    tileData.sprite = sprites[Random.Range(0, sprites.Length)];
                    break;
                case DisplayOptions.Directional:
                    string neighbourString = GetNeighbours(tilemap, location);
                    try
                    {
                        tileData.sprite = _spriteMap[neighbourString];
                    }
                    catch (KeyNotFoundException)
                    {
                        Debug.LogError($"Key \"{neighbourString}\" is missing from the spriteMap of {name}");
                        tileData.sprite = null;
                    }
                    break;
                default:
                    Debug.LogError($"{name} has an invalid displayMode value");
                    break;
            }
        }
        
        private bool HasSameTileDef(ITilemap tilemap, Vector3Int location)
        {
            return tilemap.GetTile(location) == this;
        }

        /// <summary>
        /// Checks for the presence of neighbours of the same type
        /// </summary>
        /// <param name="tilemap">The tilemap</param>
        /// <param name="location">Location of the subject tile in the grid</param>
        /// <returns>A string with a character for each cardinal direction that contains a neighbour of the same type </returns>
        private string GetNeighbours(ITilemap tilemap, Vector3Int location)
        {
            string result = "";
            result += HasSameTileDef(tilemap, location + new Vector3Int(0, 1, 0)) ? "n" : "";
            result += HasSameTileDef(tilemap, location + new Vector3Int(0, -1, 0)) ? "s" : "";
            result += HasSameTileDef(tilemap, location + new Vector3Int(1, 0, 0)) ? "e" : "";
            result += HasSameTileDef(tilemap, location + new Vector3Int(-1, 0, 0)) ? "w" : "";
            return result;
        }

        private void OnValidate()
        {
            if (displayMode != DisplayOptions.Directional) return;
            
            _spriteMap = new Dictionary<string, Sprite>();
            foreach (Sprite item in sprites)
            {
                _spriteMap[item.name.Split('_')[^1]] = item;
            }
        }

        #if UNITY_EDITOR
        [MenuItem("Assets/Create/TileDef")]
        public static void CreateTileDef()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save TileDef", "New TileDef", "Asset", "Save TileDef", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(CreateInstance<Tile>(), path);
        }
        #endif
    }
}
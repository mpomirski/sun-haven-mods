// From https://github.com/Morthy/sunhaven-mods/tree/main/Morthy.Util
using UnityEngine;

namespace AutoPetter;

public class SpriteUtil {
    private static Texture2D CreateTexture(byte[] data) {
        var texture = new Texture2D(1, 1);
        texture.LoadImage(data);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.wrapModeU = TextureWrapMode.Clamp;
        texture.wrapModeV = TextureWrapMode.Clamp;
        texture.wrapModeW = TextureWrapMode.Clamp;
        return texture;
    }

    public static Sprite CreateSprite(byte[] textureData, Vector2 pivot, string name) {
        var texture = CreateTexture(textureData);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot, 24, 0,
            SpriteMeshType.FullRect);
        sprite.name = name;
        texture.name = name;
        return sprite;
    }
}
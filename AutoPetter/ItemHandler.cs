using System;
using PSS;
using UnityEngine;
using Wish;

namespace AutoPetter;

public static class ItemHandler {
    public const int AutoPetterId = 62137;

    private static void EnableAutoPetter(ItemData item) {
        if (!item.name.Contains("Automatic Petter")) throw new Exception($"Item {item.id} does not appear to be an Auto petter.");

        var placeable = (Placeable)item.useItem;
        placeable.snapToTile = true;
        placeable.previewOffset = new Vector2(-0.1f, 0);
        placeable._decoration.transform.Find("Graphics").localPosition = new Vector3(-0.1f, 0, 0);
    }

    public static void CreateAutoPetters() {
        Database.GetData<ItemData>(AutoPetterId, EnableAutoPetter);
    }
}
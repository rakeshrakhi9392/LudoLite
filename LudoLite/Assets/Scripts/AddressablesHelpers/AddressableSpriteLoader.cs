using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.AddressablesHelpers
{
    public static class AddressableSpriteLoader
    {
        public static void LoadSprite(string spriteAddress, Action<Sprite> done)
        {
            var initHandle = Addressables.InitializeAsync();

            initHandle.Completed += _ =>
            {
                // Use Addressables to load the sprite.
                var loadHandle = Addressables.LoadAssetAsync<Sprite>(spriteAddress);

                loadHandle.Completed += operation =>
                {
                    done(operation.Result);
                };
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Monsters;



using SpaceCore.Events;
using SpaceCore.Interface;
using StardewValley.TerrainFeatures;
using xTile.Tiles;
using xTile;
using StardewValley.Tools;
using StardewValley.Network;

namespace NuclearBombs
{
    public class ModEntry : Mod
    {
        public static string NukulerBomb = "Nuclear Bomb"; //"(O)ApryllForever.NuclearBombCP_NuclearBomb";
        internal static IMonitor? ModMonitor { get; set; }
        internal new static IModHelper? Helper { get; set; }

        private float totalTimer;

        public override void Entry(IModHelper helper)
        {

            var harmony = new Harmony(this.ModManifest.UniqueID);

            ModMonitor = Monitor;
            Helper = helper;

            //helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            //SpaceEvents.BombExploded += OnBombExploded;

                               //Attempt using code based on Elizabeth's Pearl Code (Thx luv!!!). Complete Triumph!!!
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.canBePlacedHere)),
               postfix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.CanBePlacedHere_Postfix))
            );

            // Cause explosion when placed
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.placementAction)),
               prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.PlacementAction_Prefix))
            );

            // Allow placeable
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.isPlaceable)),
               postfix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.IsPlaceable_Postfix))
            );

            
        }

       // private void OnGameLaunched(object sender, EventArgs e)
        //{
            //NuclearBomb.Initialize(this);
        //}

       //public static void OnBombExploded(object sender, EventArgsBombExploded e)
        //{

        //}

        private static void CanBePlacedHere_Postfix(StardewValley.Object __instance, GameLocation l, Vector2 tile, ref bool __result)
        {
            // Not our item, we don't care
            if (!__instance.Name.Contains(NukulerBomb, StringComparison.OrdinalIgnoreCase) || __instance.bigCraftable.Value)
            {
                return;
            }
            else
            {
                // If the tile is suitable, it can be placed
                if ((!l.isTilePlaceable

                    
                    (tile, true) || l.isTileOccupiedByFarmer(tile) != null))
                {
                    __result = true;
                }
            }
        }

        private static bool PlacementAction_Prefix(StardewValley.Object __instance, GameLocation location, int x, int y, Farmer who, ref bool __result)
        {

            Vector2 placementTile = new Vector2(x, y);
            Game1.player.Position = placementTile;

            // Not our item, we don't care
            if (!__instance.Name.Contains(NukulerBomb, StringComparison.OrdinalIgnoreCase) || __instance.bigCraftable.Value)
            {
                return true;
            }
            else
            {
               
               /*
                int idNum;
                idNum = Game1.random.Next();
               
                Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite(__instance.ParentSheetIndex, 100f, 1, 24, placementTile * 64f, flicker: true, flipped: false, location, who)
                {
                    shakeIntensity = 0.5f,
                    shakeIntensityChange = 0.002f,
                    extraInfoForEndBehavior = idNum,
                    endFunction = location.removeTemporarySpritesWithID
                });
                Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * 64f + new Vector2(5f, 0f) * 4f, flicker: true, flipped: false, (float)(y + 7) / 10000f, 0f, Color.Yellow, 4f, 0f, 0f, 0f)
                {
                    id = idNum
                });
                Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * 64f + new Vector2(5f, 0f) * 4f, flicker: true, flipped: true, (float)(y + 7) / 10000f, 0f, Color.Orange, 4f, 0f, 0f, 0f)
                {
                    delayBeforeAnimationStart = 100,
                    id = idNum
                });
                Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * 64f + new Vector2(5f, 0f) * 4f, flicker: true, flipped: false, (float)(y + 7) / 10000f, 0f, Color.White, 3f, 0f, 0f, 0f)
                {
                    delayBeforeAnimationStart = 200,
                    id = idNum
                });  */

                Game1.currentLocation.playSound("ApryllForever.NuclearBomb_Siren", null, null, StardewValley.Audio.SoundContext.Default);

                //DoNukulerExplosionAnimation(location, x, y, who);

                bool success = DoNukulerExplosionAnimation(location, x, y, who);
                if (success)
                {
                __result = true;
                }
                return false;
            }
        }
        private static void IsPlaceable_Postfix(StardewValley.Object __instance, ref bool __result)
        {
            if (__instance.Name.Contains(NukulerBomb, StringComparison.OrdinalIgnoreCase))
            {
                __result = true;
            }
        }

        private static bool DoNukulerExplosionAnimation(GameLocation location, int x, int y, Farmer who)
        {
            Vector2 placementTile = new Vector2(x / 64, y / 64);
            foreach (TemporaryAnimatedSprite temporarySprite2 in location.temporarySprites)
            {
                if (temporarySprite2.position.Equals(placementTile * 64f))
                {
                    return false;
                }
            }

            int idNum = Game1.random.Next();
            location.playSound("thudStep");

            Game1.changeMusicTrack("none", false, StardewValley.GameData.MusicContext.Default);

            StardewValley.Object Nukebomb = new StardewValley.Object("ApryllForever.NuclearBombCP_NuclearBomb", 1);

            TemporaryAnimatedSprite TAS = new TemporaryAnimatedSprite(Nukebomb.parentSheetIndex, 100f, 1, 24, placementTile * 64f, flicker: true, flipped: false, location, who)
            {
                delayBeforeAnimationStart = 11000,
                bombRadius = 37,
                bombDamage = 999,
                shakeIntensity = 5f,
                shakeIntensityChange = 0.2f,
                
                
                extraInfoForEndBehavior = idNum,
                endFunction = location.removeTemporarySpritesWithID
            };

            //Microsoft.Xna.Framework.Rectangle areaOfEffect = ????????;

            for (int i = location.resourceClumps.Count - 1; i >= 0; i--)
            {
                ResourceClump feature = location.resourceClumps[i];
                if (feature is ResourceClump bush //&& bush.getBoundingBox().Contains(areaOfEffect)
                    && bush.parentSheetIndex.Value == 600)
                {
                    bush.health.Value = -1f;
                    Axe axe = new() { UpgradeLevel = 3 };
                    bush.performToolAction(axe, 999, bush.Tile);
                    location.resourceClumps.RemoveAt(i);
                }
            }

            Game1.Multiplayer.broadcastSprites(location, TAS);
            DelayedAction.playSoundAfterDelay("ApryllForever.NuclearBomb_Blast", 11000, Game1.player.currentLocation, null);


            return true;
        }
    }
}
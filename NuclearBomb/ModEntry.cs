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
using NuclearBombs.Locations;
using SpaceCore.Events;
using SpaceCore.Interface;
using StardewValley.TerrainFeatures;
using xTile.Tiles;
using xTile;
using StardewValley.Tools;
using StardewValley.Network;
using System.Reflection;
using SpaceShared.APIs;
using StardewModdingAPI.Enums;
using StardewValley.Menus;
using xTile.Layers;

namespace NuclearBombs
{
    public class ModEntry : Mod
    {
        public static string NukulerBomb = "Nuclear Bomb"; //"(O)ApryllForever.NuclearBombCP_NuclearBomb";
        internal static IMonitor? ModMonitor { get; set; }
        internal static IModHelper? Helper { get; set; }

        private float totalTimer;
        private static bool x2;

        public override void Entry(IModHelper helper)
        {

            var harmony = new Harmony(this.ModManifest.UniqueID);

            ModMonitor = Monitor;
            Helper = helper;

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.UpdateTicked += OnTickUpdated;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;

            GameLocation.RegisterTouchAction("ApyllForever.NuclearBomb_NuclearShower", this.NuclearShower);
            GameLocation.RegisterTouchAction("MermaidDress", this.MermaidDress);
            GameLocation.RegisterTouchAction("MermaidUndress", this.MermaidUndress);

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

        private void NuclearShower(GameLocation location, string[] args, Farmer player, Vector2 tile)
        {


            // IAssetDataForMap mapHelper = ModEntry.Helper.GameContent.GetPatchHelper(location.map).AsMap();
            // mapHelper.PatchMap(
            //  ModEntry.Helper.GameContent.Load<Map>("assets/Blank.tmx"),
            // targetArea: new Rectangle((int)tile.X - 1, (int)tile.Y - 1, 2, 2),
            // patchMode: PatchMapMode.Replace
            //);
            
            Game1.playSound("doorCreak");
            Game1.playSound("ApryllForever.NuclearBomb_Shower");

           

        }

        private void MermaidUndress(GameLocation location, string[] args, Farmer player, Vector2 tile)
        {
            Game1.player.bathingClothes.Value = true;
        }

        private void MermaidDress(GameLocation location, string[] args, Farmer player, Vector2 tile)
        {
            Game1.player.bathingClothes.Value = false;
        }

        private void OnGameLaunched(object sender, EventArgs e)
        {
            ModEntry.Helper = Helper;
            // ModEntry.Monitor = Monitor;

            var sc = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            //sc.RegisterSerializerType(typeof(NuclearLocation));
            //sc.RegisterSerializerType(typeof(AtomicScienceSilo));
            //sc.RegisterSerializerType(typeof(AdvancedAtomicScienceSilo));

          


            //NuclearBomb.Initialize(this);
        }


       public static void OnTickUpdated(object sender, EventArgs e)

        { 
        
       // if (Game1.player.currentLocation.Equals("Custom_AtomicScienceSilo") || Game1.player.currentLocation.Equals("Custom_AdvancedAtomicScienceSilo"))
          //  {



           // }
        
        
        
        
        
        
        
        
        }


        public void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            e.Edit(asset =>
    {
        //var editor = asset.AsMap();

       // Map sourceMap = ModEntry.Helper.ModContent.Load<Map>("AtomicScienceSilo.tmx");
        //Map sourceMap2 = ModEntry.Helper.ModContent.Load<Map>("AtomicScienceSilo.tmx");
        //editor.PatchMap(sourceMap, targetArea: new Rectangle(30, 10, 20, 20));
    });

        }



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

           // int placeX = Convert.ToInt32(placementTile.X);
            //int placeY = Convert.ToInt32(placementTile.Y);

           // Rectangle reccie = new Rectangle(  placeX -20, placeY - 20, 37, 37);

            //BoundingBox boxxxy = new();


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

            StardewValley.Object Nukebomb = new StardewValley.Object("ApryllForever.NuclearBombCP_NuclearBomb", 1); //Nukebomb.parentSheetIndex


            TemporaryAnimatedSprite TAS2 = new TemporaryAnimatedSprite(11429, 100f, 1, 24, placementTile * 64f, flicker: true, flipped: false, location, who)
            {
                delayBeforeAnimationStart = 10000,
                bombRadius = 31,
                bombDamage = 999,
                shakeIntensity = 1f,
                shakeIntensityChange = 0.0002f,
                color = Color.LightGoldenrodYellow,

                extraInfoForEndBehavior = idNum,
                endFunction = location.removeTemporarySpritesWithID
            };



            TemporaryAnimatedSprite TAS = new TemporaryAnimatedSprite(11429, 100f, 1, 24, placementTile * 64f, flicker: true, flipped: false, location, who)
            {
                delayBeforeAnimationStart = 11000,
                bombRadius = 41,
                bombDamage = 999,
                shakeIntensity = 1f,
                shakeIntensityChange = 0.002f,
                color = Color.LightCoral,
               
                
                extraInfoForEndBehavior = idNum,
                endFunction = location.removeTemporarySpritesWithID
            };

            

            //Microsoft.Xna.Framework.Rectangle areaOfEffect = ????????;

            DelayedAction.functionAfterDelay(delegate
            {


                int placeX = Convert.ToInt32(placementTile.X);
                int placeY = Convert.ToInt32(placementTile.Y);

                Rectangle reccie = new Rectangle(placeX - 20, placeY - 20, 37, 37);

                //reccie.

                for (int i = location.resourceClumps.Count - 1; i >= 0; i--)
                {
                    ResourceClump feature = location.resourceClumps[i];
                    if (feature is ResourceClump bush //bush.getBoundingBox().Contains(reccie)
                        && bush.parentSheetIndex.Value == 600)
                    {
                        if (Vector2.DistanceSquared(bush.Tile, placementTile) < (1600))  //(Math.Sqrt((bush.Tile.X - placementTile.X) + (bush.Tile.Y - placementTile.Y)) >= 0)

                        {
                            bush.health.Value = -1f;
                            Axe axe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Axe), "lastUser");
                            lastUserField.SetValue(axe, Game1.player);


                            bool x2 = axe.getLastFarmerToUse() == Game1.player;
                            bush.performToolAction(axe, 999, bush.Tile);
                            location.resourceClumps.RemoveAt(i);
                        }
                    }

                    if (feature is ResourceClump bush2 //&& bush2.getBoundingBox().Contains(reccie)
                        && bush2.parentSheetIndex.Value == 602)
                    {
                        if (Vector2.DistanceSquared(bush2.Tile, placementTile) < (1600))
                        {
                            bush2.health.Value = -1f;
                            Axe axe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Axe), "lastUser");
                            lastUserField.SetValue(axe, Game1.player);


                            bool x2 = axe.getLastFarmerToUse() == Game1.player;
                            bush2.performToolAction(axe, 999, bush2.Tile);
                            location.resourceClumps.RemoveAt(i);
                        }
                    }
                    if (feature is ResourceClump bush3 //&& bush3.getBoundingBox().Contains(reccie)
                        && bush3.parentSheetIndex.Value == 672)
                       
                        {
                        if (Vector2.DistanceSquared(bush3.Tile, placementTile) < (1600))
                        {
                                bush3.health.Value = -1f;
                                Pickaxe pickaxe = new() { UpgradeLevel = 3 };

                                FieldInfo lastUserField = AccessTools.Field(typeof(Pickaxe), "lastUser");
                                lastUserField.SetValue(pickaxe, Game1.player);


                                bool x2 = pickaxe.getLastFarmerToUse() == Game1.player;
                                bush3.performToolAction(pickaxe, 999, bush3.Tile);
                                location.resourceClumps.RemoveAt(i);
                            }
                        }
                    if (feature is ResourceClump bush4 //&& bush4.getBoundingBox().Contains(reccie)
                      && bush4.parentSheetIndex.Value == 752)
                    {

                        if (Vector2.DistanceSquared(bush4.Tile, placementTile) < (1600))
                        {
                            bush4.health.Value = -1f;
                            Pickaxe pickaxe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Pickaxe), "lastUser");
                            lastUserField.SetValue(pickaxe, Game1.player);


                            bool x2 = pickaxe.getLastFarmerToUse() == Game1.player;
                            bush4.performToolAction(pickaxe, 999, bush4.Tile);
                            location.resourceClumps.RemoveAt(i);
                        }
                    }
                    if (feature is ResourceClump bush5 //&& bush5.getBoundingBox().Contains(reccie)
                    && bush5.parentSheetIndex.Value == 754)

                    {
                        if (Vector2.DistanceSquared(bush5.Tile, placementTile) < (1600))
                        {
                            bush5.health.Value = -1f;
                            Pickaxe pickaxe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Pickaxe), "lastUser");
                            lastUserField.SetValue(pickaxe, Game1.player);


                            bool x2 = pickaxe.getLastFarmerToUse() == Game1.player;
                            bush5.performToolAction(pickaxe, 999, bush5.Tile);
                            location.resourceClumps.RemoveAt(i);
                        }
                    }

                    if (feature is ResourceClump bush6 //&& bush6.getBoundingBox().Contains(reccie)
                   && bush6.parentSheetIndex.Value == 756)
                    {
                        if (Vector2.DistanceSquared(bush6.Tile, placementTile) < (1600))
                        {
                            bush6.health.Value = -1f;
                            Pickaxe pickaxe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Pickaxe), "lastUser");
                            lastUserField.SetValue(pickaxe, Game1.player);


                            bool x2 = pickaxe.getLastFarmerToUse() == Game1.player;
                            bush6.performToolAction(pickaxe, 999, bush6.Tile);
                            location.resourceClumps.RemoveAt(i);
                        }
                    }
                    if (feature is ResourceClump bush7 //&& bush7.getBoundingBox().Contains(reccie)
                   && bush7.parentSheetIndex.Value == 758)
                    {
                        if (Vector2.DistanceSquared(bush7.Tile, placementTile) < (1600))
                        {
                            bush7.health.Value = -1f;
                            Pickaxe pickaxe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Pickaxe), "lastUser");
                            lastUserField.SetValue(pickaxe, Game1.player);


                            bool x2 = pickaxe.getLastFarmerToUse() == Game1.player;
                            bush7.performToolAction(pickaxe, 999, bush7.Tile);
                            location.resourceClumps.RemoveAt(i);
                        }
                    }
                    if (feature is ResourceClump bush8 //&& bush8.getBoundingBox().Contains(reccie)
                   && bush8.parentSheetIndex.Value == 622)
                    {
                        if (Vector2.DistanceSquared(bush8.Tile, placementTile) < (1600))
                        {
                            bush8.health.Value = -1f;
                            Pickaxe pickaxe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Pickaxe), "lastUser");
                            lastUserField.SetValue(pickaxe, Game1.player);


                            bool x2 = pickaxe.getLastFarmerToUse() == Game1.player;
                            bush8.performToolAction(pickaxe, 999, bush8.Tile);
                            location.resourceClumps.RemoveAt(i);
                        }
                    }

                    for (int q = location.largeTerrainFeatures.Count - 1; q >= 0; q--)
                    {
                        LargeTerrainFeature featur = location.largeTerrainFeatures[q];
                        if (featur is Bush bushq && (Vector2.DistanceSquared(bushq.Tile, placementTile) < (1600))) 
                        {
                            bushq.health = -1f;
                            Axe axe = new() { UpgradeLevel = 3 };

                            FieldInfo lastUserField = AccessTools.Field(typeof(Axe), "lastUser");
                            lastUserField.SetValue(axe, Game1.player);

                            bushq.performToolAction(axe, 0, bushq.Tile);
                            location.largeTerrainFeatures.RemoveAt(q);
                        }
                    }



                }
            },
            12200);
            Game1.Multiplayer.broadcastSprites(location, TAS2);
            Game1.Multiplayer.broadcastSprites(location, TAS);
            DelayedAction.playSoundAfterDelay("ApryllForever.NuclearBomb_Blast", 11000, Game1.player.currentLocation, null);


            return true;
        }
    }
}
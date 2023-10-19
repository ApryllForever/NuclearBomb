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

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
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

             // Meddle with the Explosion
            //harmony.Patch(
              // original: AccessTools.Method(typeof(StardewValley.TemporaryAnimatedSprite), nameof(StardewValley.TemporaryAnimatedSprite.update)),
              // prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.update_Prefix))
            //);



        }

        private void OnGameLaunched(object sender, EventArgs e)
        {

            //NuclearBomb.Initialize(this);

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
                foreach (TemporaryAnimatedSprite temporarySprite3 in location.temporarySprites)
                {
                    if (temporarySprite3.position.Equals(placementTile * 64f))
                    {
                        return false;
                    }
                }
                Game1.player.currentLocation = location;
                int idNum;
                idNum = Game1.random.Next();
                location.playSound("thudStep");
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
                });
                //location.netAudio.StartPlaying("fuse");
                Game1.currentLocation.playSound("ApryllForever.NuclearBomb_Siren", null, null, StardewValley.Audio.SoundContext.Default);


                DoNukulerExplosionAnimation(location, x, y, who);

                //bool success = DoNukulerExplosionAnimation(location, x, y, who);
                //if (success)
                //{
                __result = true;
                //}
                return false;
            }
        }









        private static bool update_Prefix(TemporaryAnimatedSprite __instance, GameTime time, float totalTimer, float pulseTimer, float originalScale)
        {

    

            // Not our item, we don't care
            if (Game1.player.CurrentItem.Name.Contains(NukulerBomb, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
               

                if (__instance.paused)
                {
                    return false;
                }
                if (__instance.bombRadius > 0 && !Game1.shouldTimePass())
                {
                    return false;
                }
                if (__instance.ticksBeforeAnimationStart > 0)
                {
                    __instance.ticksBeforeAnimationStart--;
                    return false;
                }
                if (__instance.delayBeforeAnimationStart > 0)
                {
                    __instance.delayBeforeAnimationStart -= time.ElapsedGameTime.Milliseconds;
                    if (__instance.delayBeforeAnimationStart <= 0 && __instance.startSound != null)
                    {
                        Game1.playSound(__instance.startSound);
                    }
                    if (__instance.delayBeforeAnimationStart <= 0 && __instance.parentSprite != null)
                    {
                        __instance.position = __instance.parentSprite.position + __instance.position;
                    }
                    return false;
                }
                __instance.timer += time.ElapsedGameTime.Milliseconds;
                totalTimer += time.ElapsedGameTime.Milliseconds;
                __instance.alpha -= __instance.alphaFade * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                __instance.alphaFade -= __instance.alphaFadeFade * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                if (__instance.alphaFade > 0f && __instance.light && __instance.alpha < 1f && __instance.alpha >= 0f)
                {
                    LightSource ls;
                    ls = Utility.getLightSource(__instance.lightID);
                    if (ls != null)
                    {
                        ls.color.A = (byte)(255f * __instance.alpha);
                    }
                }
                __instance.shakeIntensity += __instance.shakeIntensityChange * (float)time.ElapsedGameTime.Milliseconds;
                __instance.scale += __instance.scaleChange * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                __instance.scaleChange += __instance.scaleChangeChange * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                __instance.rotation += __instance.rotationChange;
                if (__instance.xPeriodic)
                {
                    __instance.position.X = __instance.initialPosition.X + __instance.xPeriodicRange * (float)Math.Sin(Math.PI * 2.0 / (double)__instance.xPeriodicLoopTime * (double)totalTimer);
                }
                else
                {
                    __instance.position.X += __instance.motion.X * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                }
                if (__instance.yPeriodic)
                {
                    __instance.position.Y = __instance.initialPosition.Y + __instance.yPeriodicRange * (float)Math.Sin(Math.PI * 2.0 / (double)__instance.yPeriodicLoopTime * (double)(totalTimer + __instance.yPeriodicLoopTime / 2f));
                }
                else
                {
                    __instance.position.Y += __instance.motion.Y * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                }
                if (__instance.attachedCharacter != null && !__instance.positionFollowsAttachedCharacter)
                {
                    if (__instance.xPeriodic)
                    {
                        __instance.attachedCharacter.position.X = __instance.initialPosition.X + __instance.xPeriodicRange * (float)Math.Sin(Math.PI * 2.0 / (double)__instance.xPeriodicLoopTime * (double)totalTimer);
                    }
                    else
                    {
                        __instance.attachedCharacter.position.X += __instance.motion.X * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                    }
                    if (__instance.yPeriodic)
                    {
                        __instance.attachedCharacter.position.Y = __instance.initialPosition.Y + __instance.yPeriodicRange * (float)Math.Sin(Math.PI * 2.0 / (double)__instance.yPeriodicLoopTime * (double)totalTimer);
                    }
                    else
                    {
                        __instance.attachedCharacter.position.Y += __instance.motion.Y * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                    }
                }
                int sign;
                sign = Math.Sign(__instance.motion.X);
                __instance.motion.X += __instance.acceleration.X * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                if (__instance.stopAcceleratingWhenVelocityIsZero && Math.Sign(__instance.motion.X) != sign)
                {
                    __instance.motion.X = 0f;
                    __instance.acceleration.X = 0f;
                }
                sign = Math.Sign(__instance.motion.Y);
                __instance.motion.Y += __instance.acceleration.Y * (float)((!__instance.timeBasedMotion) ? 1 : time.ElapsedGameTime.Milliseconds);
                if (__instance.stopAcceleratingWhenVelocityIsZero && Math.Sign(__instance.motion.Y) != sign)
                {
                    __instance.motion.Y = 0f;
                    __instance.acceleration.Y = 0f;
                }
                __instance.acceleration.X += __instance.accelerationChange.X;
                __instance.acceleration.Y += __instance.accelerationChange.Y;
                if (__instance.xStopCoordinate != -1 || __instance.yStopCoordinate != -1)
                {
                    int oldY;
                    oldY = (int)__instance.motion.Y;
                    if (__instance.xStopCoordinate != -1 && Math.Abs(__instance.position.X - (float)__instance.xStopCoordinate) <= Math.Abs(__instance.motion.X))
                    {
                        __instance.motion.X = 0f;
                        __instance.acceleration.X = 0f;
                        __instance.xStopCoordinate = -1;
                    }
                    if (__instance.yStopCoordinate != -1 && Math.Abs(__instance.position.Y - (float)__instance.yStopCoordinate) <= Math.Abs(__instance.motion.Y))
                    {
                        __instance.motion.Y = 0f;
                        __instance.acceleration.Y = 0f;
                        __instance.yStopCoordinate = -1;
                    }
                    if (__instance.xStopCoordinate == -1 && __instance.yStopCoordinate == -1)
                    {
                        __instance.rotationChange = 0f;
                        __instance.reachedStopCoordinate?.Invoke(oldY);
                        __instance.reachedStopCoordinateSprite?.Invoke(__instance);
                    }
                }
                if (!__instance.pingPong)
                {
                    __instance.pingPongMotion = 1;
                }
                if (__instance.pulse)
                {
                    pulseTimer -= time.ElapsedGameTime.Milliseconds;
                    if (originalScale == 0f)
                    {
                        originalScale = __instance.scale;
                    }
                    if (pulseTimer <= 0f)
                    {
                        pulseTimer = __instance.pulseTime;
                        __instance.scale = originalScale * __instance.pulseAmount;
                    }
                    if (__instance.scale > originalScale)
                    {
                        __instance.scale -= __instance.pulseAmount / 100f * (float)time.ElapsedGameTime.Milliseconds;
                    }
                }
                if (__instance.light)
                {
                    if (!__instance.hasLit)
                    {
                        __instance.hasLit = true;
                        __instance.lightID = Game1.random.Next(int.MinValue, int.MaxValue);
                        if (__instance.Parent == null || Game1.currentLocation == __instance.Parent)
                        {
                            Game1.currentLightSources.Add(new LightSource(4, __instance.position + new Vector2(32f, 32f), __instance.lightRadius, __instance.lightcolor.Equals(Color.White) ? new Color(0, 65, 128) : __instance.lightcolor, __instance.lightID, LightSource.LightContext.None, 0L));
                        }
                    }
                    else
                    {
                        Utility.repositionLightSource(__instance.lightID, __instance.position + new Vector2(32f, 32f));
                    }
                }
                if (__instance.alpha <= 0f || (__instance.position.X < -2000f && !__instance.overrideLocationDestroy) || __instance.scale <= 0f)
                {
                    __instance.unload();
                    return __instance.destroyable;
                }
                if (__instance.timer > __instance.interval)
                {
                    __instance.currentParentTileIndex += __instance.pingPongMotion;
                    __instance.sourceRect.X += __instance.sourceRect.Width * __instance.pingPongMotion;
                    if (__instance.Texture != null)
                    {
                        if (!__instance.pingPong && __instance.sourceRect.X >= __instance.Texture.Width)
                        {
                            __instance.sourceRect.Y += __instance.sourceRect.Height;
                        }
                        if (!__instance.pingPong)
                        {
                            __instance.sourceRect.X %= __instance.Texture.Width;
                        }
                        if (__instance.pingPong)
                        {
                            if ((float)__instance.sourceRect.X + ((float)__instance.sourceRect.Y - __instance.sourceRectStartingPos.Y) / (float)__instance.sourceRect.Height * (float)__instance.Texture.Width >= __instance.sourceRectStartingPos.X + (float)(__instance.sourceRect.Width * __instance.animationLength))
                            {
                                __instance.pingPongMotion = -1;
                                __instance.sourceRect.X -= __instance.sourceRect.Width * 2;
                                __instance.currentParentTileIndex--;
                                if (__instance.sourceRect.X < 0)
                                {
                                    __instance.sourceRect.X = __instance.Texture.Width + __instance.sourceRect.X;
                                }
                            }
                            else if ((float)__instance.sourceRect.X < __instance.sourceRectStartingPos.X && (float)__instance.sourceRect.Y == __instance.sourceRectStartingPos.Y)
                            {
                                __instance.pingPongMotion = 1;
                                __instance.sourceRect.X = (int)__instance.sourceRectStartingPos.X + __instance.sourceRect.Width;
                                __instance.currentParentTileIndex++;
                                __instance.currentNumberOfLoops++;
                                if (__instance.endFunction != null)
                                {
                                    __instance.endFunction(__instance.extraInfoForEndBehavior);
                                    __instance.endFunction = null;
                                }
                                if (__instance.currentNumberOfLoops >= __instance.totalNumberOfLoops)
                                {
                                    __instance.unload();
                                    return __instance.destroyable;
                                }
                            }
                        }
                        else if (__instance.totalNumberOfLoops >= 1 && (float)__instance.sourceRect.X + ((float)__instance.sourceRect.Y - __instance.sourceRectStartingPos.Y) / (float)__instance.sourceRect.Height * (float)__instance.Texture.Width >= __instance.sourceRectStartingPos.X + (float)(__instance.sourceRect.Width * __instance.animationLength))
                        {
                            __instance.sourceRect.X = (int)__instance.sourceRectStartingPos.X;
                            __instance.sourceRect.Y = (int)__instance.sourceRectStartingPos.Y;
                        }
                    }
                    __instance.timer = 0f;
                    if (__instance.flicker)
                    {
                        if (__instance.currentParentTileIndex < 0 || __instance.flash)
                        {
                            __instance.currentParentTileIndex = __instance.oldCurrentParentTileIndex;
                            __instance.flash = false;
                        }
                        else
                        {
                            __instance.oldCurrentParentTileIndex = __instance.currentParentTileIndex;
                            if (__instance.bombRadius > 0)
                            {
                                __instance.flash = true;
                            }
                            else
                            {
                                __instance.currentParentTileIndex = -100;
                            }
                        }
                    }
                    if (__instance.currentParentTileIndex - __instance.initialParentTileIndex >= __instance.animationLength)
                    {
                        __instance.currentNumberOfLoops++;
                        if (__instance.holdLastFrame)
                        {
                            __instance.currentParentTileIndex = __instance.initialParentTileIndex + __instance.animationLength - 1;
                            if (__instance.texture != null)
                            {
                               // this.setSourceRectToCurrentTileIndex();

                                __instance.sourceRect.X = (int)(__instance.sourceRectStartingPos.X + (float)(__instance.currentParentTileIndex * __instance.sourceRect.Width)) % __instance.texture.Width;
                                if (__instance.sourceRect.X < 0)
                                {
                                    __instance.sourceRect.X = 0;
                                }
                                __instance.sourceRect.Y = (int)__instance.sourceRectStartingPos.Y;



                            }
                            if (__instance.endFunction != null)
                            {
                                __instance.endFunction(__instance.extraInfoForEndBehavior);
                                __instance.endFunction = null;
                            }
                            return false;
                        }
                        __instance.currentParentTileIndex = __instance.initialParentTileIndex;
                        if (__instance.currentNumberOfLoops >= __instance.totalNumberOfLoops)
                        {
                            if (__instance.bombRadius > 0)
                            {
                                if (Game1.currentLocation == __instance.Parent)
                                {
                                    Game1.flashAlpha = 4f;
                                }
                                if (Game1.IsMasterGame)
                                {
                                    //__instance.Parent.netAudio.StopPlaying("fuse");
                                    __instance.Parent.playSound("ApryllForever.NuclearBomb_Blast");
                                    __instance.Parent.explode(new Vector2((int)(__instance.position.X / 64f), (int)(__instance.position.Y / 64f)), __instance.bombRadius, __instance.owner, damageFarmers: true, __instance.bombDamage);
                                }
                            }
                            __instance.unload();
                            return __instance.destroyable;
                        }
                        if (__instance.bombRadius > 0 && __instance.currentNumberOfLoops == __instance.totalNumberOfLoops - 5)
                        {
                            __instance.interval -= __instance.interval / 3f;
                        }
                    }
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

            TemporaryAnimatedSprite pearlTAS = new TemporaryAnimatedSprite(Nukebomb.parentSheetIndex, 100f, 1, 24, placementTile * 64f, flicker: true, flipped: false, location, who)
            {
                delayBeforeAnimationStart = 11000,
                bombRadius = 37,
                bombDamage = 999,
                shakeIntensity = 5f,
                shakeIntensityChange = 0.2f,
                extraInfoForEndBehavior = idNum,
                endFunction = location.removeTemporarySpritesWithID
                
            };
            //Game1.currentLocation.playSound("ApryllForever.NuclearBomb_Blast", null, null, StardewValley.Audio.SoundContext.Default);
            Game1.Multiplayer.broadcastSprites(location, pearlTAS);
            //location.netAudio.StartPlaying("fuse");
            DelayedAction.playSoundAfterDelay("ApryllForever.NuclearBomb_Blast", 11000, Game1.player.currentLocation, null);


            return true;
        }
    }
}
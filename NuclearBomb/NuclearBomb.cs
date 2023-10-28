

//                     DISREGARD THIS HERE CS FILE. It is only here in case of I might could need it in a minute.



/*


using StardewModdingAPI;
using System;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using StardewValley.Locations;
using System.Runtime.CompilerServices;
using HarmonyLib;
using xTile.Dimensions;


namespace NuclearBombs
{
    

    internal class NuclearBomb : StardewValley.Object
    {

        

        public static string NukulerBomb = null;
        static Color color = Color.Indigo;

        static IModHelper Helper;
        static IMonitor Monitor;

        internal static void Initialize(IMod ModInstance)
        {

            Helper = ModInstance.Helper;
            Monitor = ModInstance.Monitor;
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;




        }


        private static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            NukulerBomb = "(O)ApryllForever.NuclearBombCP_NuclearBomb";
        }

        private static void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
            {
                return;
            }

            if (e.Button.IsActionButton())
            {
                try
                {
                    if (Game1.player.CurrentItem?.Name != null)
                    {
                        if ((bool)(Game1.player.CurrentItem?.Name.Contains("Nuclear Bomb")))
                        {
                            int buttX = Convert.ToInt32(Game1.player.Tile.X);
                            int buttY = Convert.ToInt32(Game1.player.Tile.Y);

                            Monitor.Log($"Using Nuclear Bomb!!! May the Goddesses spare us all!!!");
                            Game1.player.reduceActiveItemByOne();
                            DoTotemWarpEffects(Game1.player, (f) => StartBomb(Game1.currentLocation, buttX, buttY, Game1.player));


                        }
                    }
                }
                catch (Exception ex)
                {
                    Monitor.Log($"Could not find Nuclear Bomb ID. That doesn't seem good... Error: {ex}");
                }
            }
        }


        public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
        {
            Vector2 placementTile;
            placementTile = new Vector2(x / 64, y / 64);
            this.health = 10;
            this.Location = location;
            this.TileLocation = placementTile;
            this.owner.Value = who?.UniqueMultiplayerID ?? Game1.player.UniqueMultiplayerID;


            if (base.QualifiedItemId == NukulerBomb)
                    {
                        foreach (TemporaryAnimatedSprite temporarySprite3 in location.temporarySprites)
                        {
                            if (temporarySprite3.position.Equals(placementTile * 64f))
                            {
                                return false;
                            }
                        }
                        int idNum;
                        idNum = Game1.random.Next();
                        location.playSound("thudStep");
                        Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite(base.ParentSheetIndex, 100f, 1, 24, placementTile * 64f, flicker: true, flipped: false, location, who)
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
                        location.netAudio.StartPlaying("fuse");
                        return true;
                    }


                    return false;
        }
      




        public static bool StartBomb(GameLocation location, int x, int y, Farmer who)
        {


            if (!(Game1.player.currentLocation is null) || !Game1.isFestival())
            {

                if (!(Game1.timeOfDay > 2550))
                {

                    Vector2 placementTile = new Vector2(x / 64, y / 64);
                    foreach (TemporaryAnimatedSprite temporarySprite2 in Game1.player.currentLocation.temporarySprites)
                    {
                        if (temporarySprite2.position.Equals(placementTile * 64f))
                        {
                            return false;
                        }
                    }

                    int idNum = Game1.random.Next();
                    Game1.player.currentLocation.playSound("thudStep");
                    TemporaryAnimatedSprite TAS = new TemporaryAnimatedSprite(0, 100f, 1, 24, placementTile * 64f, flicker: true, flipped: false, location, who)
                    {
                        bombRadius = 30,
                        bombDamage = 999,
                        shakeIntensity = 2f,
                        shakeIntensityChange = 0.02f,
                        extraInfoForEndBehavior = idNum,
                        endFunction = Game1.player.currentLocation.removeTemporarySpritesWithID
                    };
                    Game1.Multiplayer.broadcastSprites(Game1.player.currentLocation, TAS);
                    //location.netAudio.StartPlaying("fuse");
                    return true;










                    return true;
                }
                else
                {
                    Monitor.Log("Failed to explode. Which sucks");
                    
                    return false;
                }
            }
            else
            {
                Monitor.Log("Failed to explode. Stuff really sucks.");
              
                return false;


              
            }

        }

        private static void DoTotemWarpEffects(Farmer who, Func<Farmer, bool> action)
        {
            who.jitterStrength = 1f;
            Game1.changeMusicTrack("none", false, StardewValley.GameData.MusicContext.Default);

            

            Game1.currentLocation.playSound("ApryllForever.NuclearBomb_Siren", null, null, StardewValley.Audio.SoundContext.Default);
            DelayedAction.functionAfterDelay(delegate
            {
                Game1.flashAlpha = 7f;

               // Game1.playSound("ApryllForever.NuclearBomb_Blast");
                Game1.currentLocation.playSound("ApryllForever.NuclearBomb_Blast", null, null, StardewValley.Audio.SoundContext.Default);

            } , 12000);
          

            //DelayedAction.playSoundAfterDelay("ApryllForever.NuclearBomb_Blast", 19000, Game1.player.currentLocation, null);



            /*
            Game1.Multiplayer.broadcastSprites(who.currentLocation,
            new TemporaryAnimatedSprite(354, 9999f, 1, 999, who.Position + new Vector2(0.0f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -1f),
                scaleChange = 0.01f,
                alpha = 1f,
                alphaFade = 0.0075f,
                shakeIntensity = 1f,
                initialPosition = who.Position + new Vector2(0.0f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 1f
            },
            new TemporaryAnimatedSprite(354, 9999f, 1, 999, who.Position + new Vector2(-64f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -0.5f),
                scaleChange = 0.005f,
                scale = 0.5f,
                alpha = 1f,
                alphaFade = 0.0075f,
                shakeIntensity = 1f,
                delayBeforeAnimationStart = 10,
                initialPosition = who.Position + new Vector2(-64f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 0.9999f
            },
            new TemporaryAnimatedSprite(354, 9999f, 1, 999, who.Position + new Vector2(64f, -96f), false, false, false, 0.0f)
            {
                motion = new Vector2(0.0f, -0.5f),
                scaleChange = 0.005f,
                scale = 0.5f,
                alpha = 1f,
                alphaFade = 0.0075f,
                delayBeforeAnimationStart = 20,
                shakeIntensity = 1f,
                initialPosition = who.Position + new Vector2(64f, -96f),
                xPeriodic = true,
                xPeriodicLoopTime = 1000f,
                xPeriodicRange = 4f,
                layerDepth = 0.9988f
            });   */

/*

            Game1.screenGlowOnce(color, false, 0.005f, 0.3f);
            Utility.addSprinklesToLocation(who.currentLocation, Convert.ToInt32(who.Tile.X), Convert.ToInt32(who.Tile.Y), 16, 16, 1300, 20, color, null, true);
        }

    }
}

*/





//   BOMB PATCH




/*
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
} */

// Meddle with the Explosion
//harmony.Patch(
// original: AccessTools.Method(typeof(StardewValley.TemporaryAnimatedSprite), nameof(StardewValley.TemporaryAnimatedSprite.update)),
// prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.update_Prefix))
//);
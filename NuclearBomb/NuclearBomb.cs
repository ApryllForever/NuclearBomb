

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
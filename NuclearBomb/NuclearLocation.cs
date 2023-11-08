/*

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using SpaceShared;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using StardewValley.Locations;



namespace NuclearBombs.Locations
{
    [XmlType("Mods_ApryllForever_NuclearBomb_NuclearLocation")]
    public class NuclearLocation : GameLocation
    {
       

        public NuclearLocation() { }
        public NuclearLocation(IModContentHelper content, string mapPath, string mapName)
        : base(content.GetInternalAssetName("Assets/" + mapPath + ".tmx").BaseName, "Custom_" + mapName)
        {
           // var ass = "Custom_AtomicScienceSilo";
           // ModEntry.Helper.ModContent.Load<Map>("Assets/" + ass );
        }

        protected override void initNetFields()
        {
            base.initNetFields();


            terrainFeatures.OnValueAdded += (sender, added) =>
            {
                if (added is Grass grass)
                {
                    grass.grassType.Value = Grass.lavaGrass;
                    grass.loadSprite();
                }
                
            };
        }

        public override bool SeedsIgnoreSeasonsHere()
        {
            return true;
        }





       

        public override void tryToAddCritters(bool onlyIfOnScreen = false)
        {
            base.tryToAddCritters(onlyIfOnScreen);  
        }



        
    }
}

*/
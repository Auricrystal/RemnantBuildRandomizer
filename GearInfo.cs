﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using static RemnantBuildRandomizer.RemnantItem;

namespace RemnantBuildRandomizer
{
    class GearInfo
    {
        public static Dictionary<SlotType, List<RemnantItem>> GearList = new Dictionary<SlotType, List<RemnantItem>>();
        public static Dictionary<SlotType, Dictionary<RemnantItem, bool>> Disabled = new Dictionary<SlotType, Dictionary<RemnantItem, bool>>();
        public static Dictionary<SlotType, Dictionary<RemnantItem, bool>> TempDisabled;

        public static Dictionary<string, RemnantItem> reflist = new Dictionary<string, RemnantItem>();

        public static Dictionary<string, SlotType> Slots = new Dictionary<string, SlotType>() { 
            {"Chest",SlotType.CH },
            {"Head",SlotType.HE },
            {"Legs",SlotType.LE },
            {"BossHand",SlotType.HG },
            {"RegHand",SlotType.HG },
            {"BossLong",SlotType.LG },
            {"RegLong",SlotType.LG },
            {"Melee",SlotType.M },
            {"Amulets",SlotType.AM },
            {"Rings",SlotType.RI },
            {"RegularMods",SlotType.MO },
            {"LongMod",SlotType.MO },
            {"HandMod",SlotType.MO },
        };
        private static readonly XmlDocument doc = new XmlDocument();

        public static void ReadXML()
        {
            GearList.Clear();
            doc.Load("Resources/GearInfo.xml");
            parseItems("RegularMods");
            parseItems("LongMod");
            parseItems("HandMod");
            parseItems("Chest");
            parseItems("Head");
            parseItems("Legs");
            parseItems("BossHand");
            parseItems("RegHand");
            parseItems("BossLong");
            parseItems("RegLong");
            parseItems("Melee");
            parseItems("Amulets");
            parseItems("Rings");
            TempDisabled = new Dictionary<SlotType, Dictionary<RemnantItem, bool>>(Disabled);
        }
        public static void parseItems(string tag) {
            List<RemnantItem> list = new List<RemnantItem>();
            foreach (XmlElement xe in doc.GetElementsByTagName(tag))
            {             
                RemnantItem ri = new RemnantItem(XmlElementExtension.GetXPath(xe).Replace("/GearInfo", ""), xe.GetAttribute("desc"), Slots[tag]);
                ri.Mod = xe.GetAttribute("mod"); 
                ri.Dlc = xe.GetAttribute("DLC"); 
                list.Add(ri);
                if (ri.Slot == SlotType.HG || ri.Slot == SlotType.LG|| ri.Slot == SlotType.M) {
                    ri.Description = ri.Itemname;
                }
                reflist.Add(ri.Itemname,ri);
                if (Disabled.ContainsKey(ri.Slot)) { Disabled[ri.Slot].Add(ri, false); } else {
                    Disabled.Add(ri.Slot, new Dictionary<RemnantItem, bool>());
                    Disabled[ri.Slot].Add(ri,false);
                }
            }
            SlotType st = Slots[tag];
            if (GearList.ContainsKey(st)) { GearList[st].AddRange(list); } else { GearList[st]=list; }
        }
        
    }
}

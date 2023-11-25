﻿/*
 * Copyright © 2023-2023 EDDiscovery development team
 *
 * Licensed under the Apache License", Version 2.0 (the "License")"] = "you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing", software distributed under
 * the License is distributed on an "AS IS" BASIS", WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND", either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace EliteDangerousCore
{
    public class StationDefinitions
    {
        // names are matched to spansh names

        public static Dictionary<string, string> ServiceTypes = new Dictionary<string, string>()
        {
            ["apexinterstellar"] = "Apex Interstellar",
            ["autodock"] = "Autodock",
            ["bartender"] = "Bartender",
            ["blackmarket"] = "Black Market",
            ["contacts"] = "Contacts",
            ["crewlounge"] = "Crew Lounge",
            ["dock"] = "Dock",
            ["workshop"] = "Workshop",      // synonmyms
            ["engineer"] = "Workshop",
            ["modulepacks"] = "Fleet Carrier Administration",
            ["carrierfuel"] = "Fleet Carrier Fuel",
            ["carriermanagement"] = "Fleet Carrier Management",
            ["carriervendor"] = "Fleet Carrier Vendor",
            ["flightcontroller"] = "Flight Controller",
            ["frontlinesolutions"] = "Frontline Solutions",
            ["facilitator"] = "Interstellar Factors Contact",
            ["initiatives"] = "Initiatives",
            ["livery"] = "Livery",
            ["commodities"] = "Market",
            ["materialtrader"] = "Material Trader",
            ["missions"] = "Missions",
            ["missionsgenerated"] = "Missions Generated",
            ["ondockmission"] = "On Dock Mission",
            ["outfitting"] = "Outfitting",
            ["pioneersupplies"] = "Pioneer Supplies",
            ["powerplay"] = "Powerplay",
            ["voucherredemption"] = "Redemption Office",
            ["refuel"] = "Refuel",
            ["repair"] = "Repair",
            ["rearm"] = "Restock",
            ["searchandrescue"] = "Search And Rescue",
            ["searchrescue"] = "Search And Rescue",
            ["shipyard"] = "Shipyard",
            ["shop"] = "Shop",
            ["socialspace"] = "Social Space",
            ["stationmenu"] = "Station Menu",
            ["stationoperations"] = "Station Operations",
            ["techbroker"] = "Technology Broker",
            ["tuning"] = "Tuning",
            ["exploration"] = "Universal Cartographics",
            ["vistagenomics"] = "Vista Genomics",

        };

        public static string ReverseLookup(string englishname)
        {
            foreach(var kvp in ServiceTypes)
            {
                if (englishname.Equals(kvp.Value, System.StringComparison.InvariantCultureIgnoreCase))
                    return kvp.Key;
            }

            System.Diagnostics.Debug.WriteLine($"*** Reverse lookup services failed {englishname}");
            return null;
        }



//["Orbis"] = "Orbis",
//["Outpost"] = "Outpost",
//["Bernal"] = "Bernal",
//["Coriolis"] = "Coriolis",
//["MegaShip"] = "MegaShip",
//["SurfaceStation"] = "SurfaceStation",
//["AsteroidBase"] = "AsteroidBase",
//["CraterOutpost"] = "CraterOutpost",
//["CraterPort"] = "CraterPort",
//["Ocellus"] = "Ocellus",
//["FleetCarrier"] = "FleetCarrier",
//["OnFootSettlement"] = "OnFootSettlement",


        public static Dictionary<string, string> StarportTypes = new Dictionary<string, string>()
        {
            // in journal as of Nov 23

            ["asteroidbase"] = "Asteroid Base",
            ["coriolis"] = "Coriolis Starport", 

            ["fleetcarrier"] = "Drake-Class Carrier",   
            ["megaship"] = "Mega Ship",     

            ["ocellus"] = "Ocellus Starport",   
            ["bernal"] = "Ocellus Starport",    
         
            ["orbis"] = "Orbis Starport",       

            ["outpost"] = "Outpost",            
            ["onfootsettlement"] = "Settlement",    

            ["surfacestation"] = "Planetary Outpost",   
            ["crateroutpost"] = "Planetary Outpost",

            ["craterport"] = "Planetary Port",       

            // these are from Spansh but not seen in my journals

            ["coriolis starport"] = "Coriolis Starport",
            ["orbis starport"] = "Orbis Starport",
            ["ocellus starport"] = "Ocellus Starport",

            ["planetary outpost"] = "Planetary Outpost",
            ["planetary port"] = "Planetary Port",
                
            ["settlement"] = "Settlement",      // only seen in redeemvoucher
            ["carrier"] = "Drake-Class Carrier",

            ["civilian outpost"] = "Outpost",
            ["commercial outpost"] = "Outpost",
            ["industrial outpost"] = "Outpost",
            ["military outpost"] = "Outpost",
            ["mining outpost"] = "Outpost",    
            ["scientific outpost"] = "Outpost",
            ["outpostscientific"] = "Outpost",
            ["megashipcivilian"] = "Mega Ship",

        };

    }

    // Station adds onto to JournalDocked more fields

    [System.Diagnostics.DebuggerDisplay("Station {System.Name} {BodyName} {StationName}")]
    public class StationInfo : JournalEvents.JournalDocked
    {
        public StationInfo(System.DateTime utc) : base(utc)
        {
        }
        public double DistanceRefSystem { get; set; }
        public ISystem System { get; set; }
        public string BodyName { get; set; }
        public string BodyType { get; set; }
        public string BodySubType { get; set; }
        public double DistanceToArrival { get; set; }
        public bool IsPlanetary { get; set; }
        public bool IsFleetCarrier { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool HasMarket { get; set; }                 // may be true because its noted in services, but with market = null because of no data
        public List<CCommodities> Market { get; set; }      // may be null
        public DateTime MarketUpdateUTC { get; set; }
        public double MarketAgeInDays { get { return DateTime.UtcNow.Subtract(MarketUpdateUTC).TotalDays; } }
        public string MarketStateString { get { if (HasMarket && Market != null) return $"\u2713 {MarketAgeInDays:N1}"; else if (HasMarket) return "\u2713 ND"; else return ""; } }

        // sync with journalcarrier..
        public bool HasItem(string fdname) { return Market != null && Market.FindIndex(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase)) >= 0; }
        public bool HasItemToBuy(string fdname) { return Market != null && Market.FindIndex(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase) && x.CanBeBought) >= 0; }
        public bool HasItemToSell(string fdname) { return Market != null && Market.FindIndex(x => x.fdname.Equals(fdname, StringComparison.InvariantCultureIgnoreCase) && x.CanBeSold) >= 0; }

        // go thru the market array, and see if any of the fdnames given matches that market entry
        public bool HasAnyItem(string[] fdnames) { return Market != null && Market.FindIndex(x => fdnames.IndexOf(x.fdname, StringComparison.InvariantCultureIgnoreCase) >= 0) >= 0; }
        public bool HasAnyItemToBuy(string[] fdnames) { return Market != null && Market.FindIndex(x => fdnames.IndexOf(x.fdname, StringComparison.InvariantCultureIgnoreCase) >= 0 && x.CanBeBought) >= 0; }
        public bool HasAnyItemToSell(string[] fdnames) { return Market != null && Market.FindIndex(x => fdnames.IndexOf(x.fdname, StringComparison.InvariantCultureIgnoreCase) >= 0 && x.CanBeSold) >= 0; }

        public bool HasOutfitting { get; set; }// see market
        public List<Outfitting.OutfittingItem> Outfitting { get; set; }     // may be null
        public DateTime OutfittingUpdateUTC { get; set; }
        public bool HasAnyModuleTypes(string[] fdnames) { return Outfitting != null && Outfitting.FindIndex(x => fdnames.IndexOf(x.ModType, StringComparison.InvariantCultureIgnoreCase) >= 0) >= 0; }
        public double OutfittingAgeInDays { get { return DateTime.UtcNow.Subtract(OutfittingUpdateUTC).TotalDays; } }
        public string OutfittingStateString { get { if (HasOutfitting && Outfitting != null) return $"\u2713 {OutfittingAgeInDays:N1}"; else if (HasOutfitting) return "\u2713 ND"; else return ""; } }

        public bool HasShipyard { get; set; }   // see market
        public List<ShipYard.ShipyardItem> Shipyard { get; set; }     // may be null
        public DateTime ShipyardUpdateUTC { get; set; }
        public bool HasAnyShipTypes(string[] fdnames) { return Shipyard != null && Shipyard.FindIndex(x => fdnames.IndexOf(x.ShipType, StringComparison.InvariantCultureIgnoreCase) >= 0) >= 0; }
        public double ShipyardAgeInDays { get { return DateTime.UtcNow.Subtract(ShipyardUpdateUTC).TotalDays; } }
        public string ShipyardStateString { get { if (HasShipyard && Shipyard != null) return $"\u2713 {ShipyardAgeInDays:N1}"; else if (HasShipyard) return "\u2713 ND"; else return ""; } }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Station:", StationName, "System:", System.Name, "Body:", BodyName, "Lat:;;N4", Latitude, "Long:;;N4", Longitude, "Distance to Arrival:;ls;N1", DistanceToArrival);
            base.FillInformation(out string baseinfo, out string basedetailed, false);
            info = info.AppendPrePad(baseinfo, global::System.Environment.NewLine);
            detailed = basedetailed;
            if (HasMarket)
            {
                string l = "";
                foreach (CCommodities m in Market.EmptyIfNull())
                {
                    l = l.AppendPrePad(" " + m.ToStringShort(), global::System.Environment.NewLine);
                }

                detailed += global::System.Environment.NewLine + "Market: " + global::System.Environment.NewLine + l;
            }

            if (HasOutfitting)
            {
                string l = "";
                foreach (var o in Outfitting.EmptyIfNull())
                {
                    l = l.AppendPrePad(" " + o.ToStringShort(), global::System.Environment.NewLine);
                }

                detailed += global::System.Environment.NewLine + "Outfitting: " + global::System.Environment.NewLine + l;
            }
            if (HasShipyard)
            {
                string l = "";
                foreach (var s in Shipyard.EmptyIfNull())
                {
                    l = l.AppendPrePad(" " + s.ToStringShort(), global::System.Environment.NewLine);
                }

                detailed += global::System.Environment.NewLine + "Shipyard: " + global::System.Environment.NewLine + l;
            }
        }

    }

}



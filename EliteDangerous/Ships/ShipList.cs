﻿/*
 * Copyright © 2016-2023 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("{currentid} ships {Ships.Count}")]
    public class ShipList
    {
        public string CurrentShipID { get { return currentid; } }           // by shipid key
        public Dictionary<string, Ship> Ships { get; private set; }         // by shipid key

        public ShipModulesInStore StoredModules { get; private set; }       // stored modules

        public bool HaveCurrentShip { get { return currentid != null; } }

        [QuickJSON.JsonIgnore()]
        public Ship CurrentShip { get { return (HaveCurrentShip) ? Ships[currentid] : null; } }

        // IDs have been repeated, need more than just that
        private string Key(string fdname, ulong i) { return fdname.ToLowerInvariant() + ":" + i.ToStringInvariant(); }

        public Ship GetShipByShortName(string sn)
        {
            List<Ship> lst = Ships.Values.ToList();
            int index = lst.FindIndex(x => x.ShipShortName.Equals(sn));
            return (index >= 0) ? lst[index] : null;
        }

        public Ship GetShipByNameIdentType(string sn)
        {
            List<Ship> lst = Ships.Values.ToList();
            int index = lst.FindIndex(x => x.ShipNameIdentType.Equals(sn));
            return (index >= 0) ? lst[index] : null;
        }

        public Ship GetShipByFullInfoMatch(string sn)
        {
            List<Ship> lst = Ships.Values.ToList();
            int index = lst.FindIndex(x => x.ShipFullInfo().IndexOf(sn, StringComparison.InvariantCultureIgnoreCase) != -1);
            return (index >= 0) ? lst[index] : null;
        }

        private ulong newsoldid = ulong.MaxValue / 2;

        public ShipList()
        {
            Ships = new Dictionary<string, Ship>();
            StoredModules = new ShipModulesInStore();
            itemlocalisation = new Dictionary<string, string>();
            currentid = null;
        }

        public void Loadout(ulong id, string ship, string shipfd, string name, string ident, List<ShipModule> modulelist,
                        long HullValue, long ModulesValue, long Rebuy, double unladenmass, double reservefuelcap, double hullhealth, bool? Hot)
        {
            string sid = Key(shipfd, id);

            //System.Diagnostics.Debug.WriteLine("Loadout {0} {1} {2} {3}", id, ship, name, ident);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm = sm.SetShipDetails(ship, shipfd, name, ident, 0, 0, HullValue, ModulesValue, Rebuy, unladenmass, reservefuelcap, hullhealth, Hot);     // update ship key, make a fresh one if required.

            //System.Diagnostics.Debug.WriteLine("Loadout " + sid);

            Ship newsm = null;       // if we change anything, we need a new clone..

            Dictionary<ShipSlots.Slot, ShipModule> moduleSlots = sm.Modules.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (ShipModule m in modulelist)
            {
                if (!sm.Contains(m.SlotFD) || !sm.Same(m))  // no slot, or not the same data.. (ignore localised item)
                {
                    if (m.LocalisedItem == null && itemlocalisation.ContainsKey(m.Item))        // if we have a cached localisation, use it
                    {
                        m.LocalisedItem = itemlocalisation[m.Item];
                        //                        System.Diagnostics.Debug.WriteLine("Have localisation for " + m.Item + ": " + m.LocalisedItem);
                    }

                    if (newsm == null)              // if not cloned
                    {
                        newsm = sm.ShallowClone();  // we need a clone, pointing to the same modules, but with a new dictionary
                        Ships[sid] = newsm;              // update our record of last module list for this ship
                    }

                    newsm.SetModule(m);                   // update entry only.. rest will still point to same entries
                }

                moduleSlots.Remove(m.SlotFD);
            }

            // Remove modules not in loadout
            if (moduleSlots.Count != 0)
            {
                List<ShipModule> modulesToRemove = moduleSlots.Values.ToList();

                if (newsm == null)
                {
                    newsm = sm.ShallowClone();
                }

                foreach (ShipModule m in modulesToRemove)
                {
                    //System.Diagnostics.Trace.WriteLine($"Warning: Module {m.Item} in slot {m.Slot} is missing from loadout");
                    newsm = newsm.RemoveModule(m.SlotFD, m.Item);
                }

                Ships[sid] = newsm;
            }
            VerifyList();
        }

        public void LoadGame(ulong id, string ship, string shipfd, string name, string ident, double fuellevel, double fueltotal)        // LoadGame..
        {
            string sid = Key(shipfd, id);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.

            Ships[sid] = sm = sm.SetShipDetails(ship, shipfd, name, ident, fuellevel, fueltotal);   // this makes a shallow copy if any data has changed..

            //System.Diagnostics.Debug.WriteLine("Load Game " + sid);

            if (ItemData.IsShip(shipfd))
                currentid = sid;
            VerifyList();
        }

        public void LaunchSRV()
        {
            //System.Diagnostics.Debug.WriteLine("Launch SRV");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.SRV);
            VerifyList();
        }

        public void DockSRV()
        {
            //System.Diagnostics.Debug.WriteLine("Dock SRV");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.None);
            VerifyList();
        }

        public void DestroyedSRV()
        {
            //System.Diagnostics.Debug.WriteLine("Destroyed SRV");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.None);
            VerifyList();
        }

        public void LaunchFighter(bool pc)
        {
            //System.Diagnostics.Debug.WriteLine("Launch Fighter");
            if (HaveCurrentShip && pc == true)
                Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.Fighter);
            VerifyList();
        }

        public void DockFighter()
        {
            //System.Diagnostics.Debug.WriteLine("Dock Fighter");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.None);
            VerifyList();
        }

        public void FighterDestroyed()      // even if NPC controlled, no harm in setting back to none since we must be in ship
        {
            //System.Diagnostics.Debug.WriteLine("Fighter Destroyed");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.None);
            VerifyList();
        }

        public void Resurrect(bool abandonedship)
        {
            if (HaveCurrentShip)           // resurrect always in ship
            {
                if (abandonedship)
                    Ships[currentid] = Ships[currentid].Destroyed();
                else
                    Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.None);
            }
            VerifyList();
        }

        public void VehicleSwitch(string to)
        {
            if (HaveCurrentShip)
            {
                if (to == "Fighter")
                    Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.Fighter);
                else
                    Ships[currentid] = Ships[currentid].SetSubVehicle(Ship.SubVehicleType.None);
            }
            VerifyList();
        }

        public void ShipyardSwap(JournalShipyardSwap e, string station, string system)
        {
            if (e.StoreShipId.HasValue)    // if we have an old ship ID (old records do not)
            {
                string oldship = Key(e.StoreOldShipFD, e.StoreShipId.Value);

                if (Ships.ContainsKey(oldship))
                {
                    //System.Diagnostics.Debug.WriteLine(oldship + " Swap Store at " + system + ":" + station);
                    Ships[oldship] = Ships[oldship].Store(station, system);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(e.StoreOldShipFD + " Cant find to swap");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(e.StoreOldShipFD + " Cant find to swap");
            }

            string sid = Key(e.ShipFD, e.ShipId);           //swap to new ship

            //System.Diagnostics.Debug.WriteLine(sid + " Swap to at " + system);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            sm = sm.SetShipDetails(e.ShipType, e.ShipFD);   // shallow copy if changed
            sm = sm.SwapTo();                               // swap into
            Ships[sid] = sm;
            currentid = sid;
            VerifyList();
        }

        public void ShipyardNew(string ship, string shipFD, ulong id)
        {
            string sid = Key(shipFD, id);
            //System.Diagnostics.Debug.WriteLine(sid + " New");

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm.SetShipDetails(ship, shipFD); // shallow copy if changed
            currentid = sid;
            VerifyList();
        }

        public void Sell(string ShipFD, ulong id)
        {
            string sid = Key(ShipFD, id);
            if (Ships.ContainsKey(sid))       // if we don't have it, don't worry
            {
                //System.Diagnostics.Debug.WriteLine(sid + " Sold ");
                Ships[sid] = Ships[sid].SellShip();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(sid + " can't find to Sell");
            }
            VerifyList();
        }

        public void Transfer(string ship, string shipFD, ulong id, string fromsystem, string tosystem, string tostation, DateTime arrivaltime)
        {
            string sid = Key(shipFD, id);
            Ship sm = EnsureShip(sid);              // this either gets current ship or makes a new one.
            sm = sm.SetShipDetails(ship, shipFD);               // set up minimum stuff we know about it
            sm = sm.Transfer(tosystem, tostation, arrivaltime);    // transfer set up
            Ships[sid] = sm;
            //System.Diagnostics.Debug.WriteLine(shipFD + " Transfer from " + fromsystem + " to " + tosystem + ":" + tostation + " arrives " + arrivaltime.ToString());
            VerifyList();
        }

        public void Store(string ShipFD, ulong id, string station, string system)
        {
            string sid = Key(ShipFD, id);
            if (Ships.ContainsKey(sid))       // if we don't have it, don't worry
            {
                //System.Diagnostics.Debug.WriteLine(sid + " store on buy at " + system);
                Ships[sid] = Ships[sid].Store(station, system);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(sid + " cannot find ship to store on buy");
            }
            VerifyList();
        }

        public void StoredShips(StoredShip[] ships)
        {
            foreach (var i in ships)
            {
                string sid = Key(i.ShipTypeFD, i.ShipID);
                //System.Diagnostics.Debug.WriteLine(sid + " Stored info " + i.StarSystem + ":" + i.StationName + " transit" + i.InTransit);

                Ship sm = EnsureShip(sid);              // this either gets current ship or makes a new one.
                sm = sm.SetShipDetails(i.ShipType, i.ShipTypeFD, i.Name, hot: i.Hot);  // set up minimum stuff we know about it

                if (!i.InTransit)                                 // if in transit, we don't know where it is, ignore
                    sm = sm.Store(i.StationName, i.StarSystem);         // ship is not with us, its stored, so store it.

                Ships[sid] = sm;
            }
            VerifyList();
        }

        public void SetUserShipName(JournalSetUserShipName e)
        {
            string sid = Key(e.ShipFD, e.ShipID);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm.SetShipDetails(e.Ship, e.ShipFD, e.ShipName, e.ShipIdent); // will clone if data changed..
            currentid = sid;           // must be in it to do this
            VerifyList();
        }

        public void ModuleBuy(JournalModuleBuy e, ISystem sys)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            Ship sm = EnsureShip(sid);              // this either gets current ship or makes a new one.

            Ships[sid] = sm = sm.SetShipDetails(e.Ship, e.ShipFD);   // shallow copy if changed, store back into array (bug may 24!)

            if (e.StoredItem.Length > 0)                             // if we stored something
                StoredModules = StoredModules.StoreModule(e.StoredItemFD, e.StoredItem, e.StoredItemLocalised, sys);

            Ships[sid] = sm.AddModule(e.Slot, e.SlotFD, e.BuyItem, e.BuyItemFD, e.BuyItemLocalised);      // replace the slot with this

            itemlocalisation[e.BuyItem] = e.BuyItemLocalised;       // record any localisations
            if (e.SellItem.Length > 0)
                itemlocalisation[e.SellItem] = e.SellItemLocalised;
            if (e.StoredItem.Length > 0)
                itemlocalisation[e.StoredItem] = e.StoredItemLocalised;

            VerifyList();
        }

        public void ModuleBuyAndStore(JournalModuleBuyAndStore e, ISystem sys)
        {
            StoredModules = StoredModules.StoreModule(e.BuyItemFD, e.BuyItem, e.BuyItemLocalised, sys);
            VerifyList();
        }

        public void ModuleSell(JournalModuleSell e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.

            sm = sm.SetShipDetails(e.Ship, e.ShipFD);   // shallow copy if changed
            Ships[sid] = sm.RemoveModule(e.SlotFD, e.SellItem);

            if (e.SellItem.Length > 0)
                itemlocalisation[e.SellItem] = e.SellItemLocalised;

            VerifyList();
        }

        public void ModuleSwap(JournalModuleSwap e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            sm = sm.SetShipDetails(e.Ship, e.ShipFD);   // shallow copy if changed
            Ships[sid] = sm.SwapModule(e.FromSlot, e.FromSlotFD, e.FromItem, e.FromItemFD, e.FromItemLocalised,
                                            e.ToSlot, e.ToSlotFD, e.ToItem, e.ToItemFD, e.ToItemLocalised);
            VerifyList();
        }

        public void ModuleStore(JournalModuleStore e, ISystem sys)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.

            sm = sm.SetShipDetails(e.Ship, e.ShipFD);   // shallow copy if changed

            if (e.ReplacementItem.Length > 0)
                Ships[sid] = sm.AddModule(e.Slot, e.SlotFD, e.ReplacementItem, e.ReplacementItemFD, e.ReplacementItemLocalised);
            else
                Ships[sid] = sm.RemoveModule(e.SlotFD, e.StoredItem);

            StoredModules = StoredModules.StoreModule(e, sys);
            VerifyList();
        }

        public void ModuleRetrieve(JournalModuleRetrieve e, ISystem sys)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.

            sm = sm.SetShipDetails(e.Ship, e.ShipFD);   // shallow copy if changed
            if (e.SwapOutItem.Length > 0)
                StoredModules = StoredModules.StoreModule(e.SwapOutItemFD, e.SwapOutItem, e.SwapOutItemLocalised, sys);

            Ships[sid] = sm.AddModule(e.Slot, e.SlotFD, e.RetrievedItem, e.RetrievedItemFD, e.RetrievedItemLocalised);

            StoredModules = StoredModules.RemoveModuleUsingEnglishName(e.RetrievedItem);
            VerifyList();
        }

        public void ModuleSellRemote(JournalModuleSellRemote e)
        {
            StoredModules = StoredModules.RemoveModuleUsingEnglishName(e.SellItem);
        }

        public void MassModuleStore(JournalMassModuleStore e, ISystem sys)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            Ship sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            sm = sm.SetShipDetails(e.Ship, e.ShipFD);   // will clone if data changed..
            Ships[sid] = sm.RemoveModules(e.ModuleItems);
            StoredModules = StoredModules.StoreModule(e.ModuleItems, itemlocalisation, sys);
            VerifyList();
        }

        public void UpdateStoredModules(JournalStoredModules s)
        {
            StoredModules = StoredModules.UpdateStoredModules(s.ModuleItems);
            VerifyList();
        }

        public void SupercruiseEntry(JournalSupercruiseEntry e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetSubVehicle(Ship.SubVehicleType.None);
            }
            VerifyList();
        }

        public void FSDJump(JournalFSDJump e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(e.FuelLevel).SetSubVehicle(Ship.SubVehicleType.None);
            }
            VerifyList();
        }

        public void FuelScoop(JournalFuelScoop e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(e.Total);
            }
            VerifyList();
        }

        public void FuelReservoirReplenished(JournalReservoirReplenished e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(e.FuelMain, e.FuelReservoir);
            }
            VerifyList();
        }

        public void UIFuel(UIEvents.UIFuel e)       // called by controller if a new UI fuel is found
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(e.Fuel, e.FuelRes);
            }
            VerifyList();
        }

        public void RefuelAll(JournalRefuelAll e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(CurrentShip.FuelCapacity);
            }
            VerifyList();
        }

        public void RefuelPartial(JournalRefuelPartial e)
        {
            if (HaveCurrentShip)
            {
                // Amount includes reserve
                double level = CurrentShip.FuelLevel + e.Amount - 0.1;

                // If amount refuelled is less than 10%, then the tank is full
                if (e.Amount < CurrentShip.FuelCapacity / 10 || level > CurrentShip.FuelCapacity)
                    level = CurrentShip.FuelCapacity;

                Ships[currentid] = CurrentShip.SetFuelLevel(level);
            }
            VerifyList();
        }

        public void EngineerCraft(JournalEngineerCraftBase c)
        {
            if (HaveCurrentShip)
            {
                System.Diagnostics.Debug.Assert(c.Engineering != null);     // double check its being passed a good engineering

                Ships[currentid] = CurrentShip.Craft(c.SlotFD, c.Module, c.Engineering);
            }
            VerifyList();
        }

        #region Helpers

        private Ship EnsureShip(string id)      // ensure we have an ID of this type..
        {
            if (Ships.ContainsKey(id))
            {
                Ship sm = Ships[id];
                if (sm.State == Ship.ShipState.Owned)               // if owned, ok
                    return sm;
                else
                {
                    Ships[Key(sm.ShipFD, newsoldid++)] = sm;              // okay, we place this information on back ID list+  all Ids of this will now refer to new entry
                }
            }

            ulong i = id.Substring(id.IndexOf(":") + 1).InvariantParseULong(0);
            Ship smn = new Ship(i);
            Ships[id] = smn;
            return smn;
        }

        void VerifyList()       // included so when debugging we can turn this on and verify the list after every action. Journals are so random they sometimes throw up problems.
        {
            //foreach( KeyValuePair<string,ShipInformation> i in Ships)
            //{
            //System.Diagnostics.Debug.Assert(i.Value.ShipFD.HasChars());
            //}
        }

        #endregion

        #region process

        public Tuple<Ship, ShipModulesInStore> Process(JournalEntry je, string whereami, ISystem system ,bool multicrew)
        {
            if (je is IShipInformation)
            {
                if (multicrew)
                {
                    System.Diagnostics.Debug.WriteLine($"ShipList Ignore {je.EventTimeUTC} {je.EventTypeStr} due to multicrew");
                }
                else
                {
                    IShipInformation e = je as IShipInformation;
                    e.ShipInformation(this, whereami, system);                             // not cloned.. up to callers to see if they need to
                }
            }

            return new Tuple<Ship, ShipModulesInStore>(CurrentShip, StoredModules);
        }

        #endregion

        #region vars
        private Dictionary<string, string> itemlocalisation;
        private string currentid;
        #endregion
    }


}

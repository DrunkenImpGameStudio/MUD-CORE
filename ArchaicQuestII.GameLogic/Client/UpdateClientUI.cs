﻿using System;
using System.Linq;
using System.Text;
using ArchaicQuestII.GameLogic.Character;
using ArchaicQuestII.GameLogic.Core;
using ArchaicQuestII.GameLogic.Effect;
using ArchaicQuestII.GameLogic.Hubs;
using ArchaicQuestII.GameLogic.Item;
using ArchaicQuestII.GameLogic.Utilities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ArchaicQuestII.GameLogic.Client
{
    public class UpdateClientUI : IUpdateClientUI
    {
        private readonly IHubContext<GameHub> _hubContext;

        public UpdateClientUI(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async void UpdateScore(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("ScoreUpdate", JsonConvert.SerializeObject(new { player = player }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateCommunication(Player player, string message, string type)
        {
            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("CommUpdate", message, type);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateHP(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync(
                        "UpdatePlayerHP",
                        player.Attributes.Attribute[EffectLocation.Hitpoints],
                        player.MaxAttributes.Attribute[EffectLocation.Hitpoints]
                    );
                //   UpdateScore(player);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateMana(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync(
                        "UpdatePlayerMana",
                        player.Attributes.Attribute[EffectLocation.Mana],
                        player.MaxAttributes.Attribute[EffectLocation.Mana]
                    );
                // UpdateScore(player);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateMoves(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync(
                        "UpdatePlayerMoves",
                        player.Attributes.Attribute[EffectLocation.Moves],
                        player.MaxAttributes.Attribute[EffectLocation.Moves]
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateExp(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("UpdatePlayerExp", player.ExperienceToNextLevel, player.Experience);
                // UpdateScore(player);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateAffects(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("UpdatePlayerAffects", player.Affects);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateEquipment(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                // Copy Pasta from Equip.cs
                // Sorry
                var displayEquipment = new StringBuilder();
                displayEquipment
                    .Append("<p>You are using:</p>")
                    .Append("<table>")
                    .Append(
                        "<tr><td style='width:175px;' class=\"cell-title\" title='Worn as light'>"
                    )
                    .Append("&lt;used as light&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Light?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td class=\"cell-title\" title='Worn on finger'>")
                    .Append(" &lt;worn on finger&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Finger?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on finger'>")
                    .Append(" &lt;worn on finger&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Finger2?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn around neck'>")
                    .Append(" &lt;worn around neck&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Neck?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td class=\"cell-title\" title='Worn around neck'>")
                    .Append(" &lt;worn around neck&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Neck2?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on face'>")
                    .Append(" &lt;worn on face&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Face?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on head'>")
                    .Append(" &lt;worn on head&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Head?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on torso'>")
                    .Append(" &lt;worn on torso&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Torso?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on legs'>")
                    .Append(" &lt;worn on legs&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Legs?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on feet'>")
                    .Append(" &lt;worn on feet&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Feet?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on hands'>")
                    .Append(" &lt;worn on hands&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Hands?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on arms'>")
                    .Append(" &lt;worn on arms&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Arms?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn about body'>")
                    .Append(" &lt;worn about body&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.AboutBody?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on waist'>")
                    .Append(" &lt;worn about waist&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Waist?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on wrist'>")
                    .Append(" &lt;worn around wrist&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Wrist?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn on wrist'>")
                    .Append(" &lt;worn around wrist&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Wrist2?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='worn as weapon'>")
                    .Append(" &lt;wielded&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Wielded?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='worn as weapon'>")
                    .Append(" &lt;secondary&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Secondary?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Worn as shield'>")
                    .Append(" &lt;worn as shield&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Shield?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Held'>")
                    .Append(" &lt;Held&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Held?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("<tr><td  class=\"cell-title\" title='Floating Nearby'>")
                    .Append(" &lt;Floating nearby&gt;")
                    .Append("</td>")
                    .Append("<td>")
                    .Append(player.Equipped.Floating?.ReturnWithFlags() ?? "(nothing)")
                    .Append("</td></tr>")
                    .Append("</table");

                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("EquipmentUpdate", displayEquipment.ToString());
                // UpdateScore(player);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void GetMap(Player player, string map)
        {
            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("MapUpdate", map, player.RoomId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateQuest(Player player)
        {
            try
            {
                var quests = JsonConvert.SerializeObject(player.QuestLog);

                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("QuestUpdate", quests);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateInventory(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                // Copy Pasta from inventory.cs
                // Sorry
                var inventory = new StringBuilder();
                inventory.Append("<p>You are carrying:</p>");

                if (player.Inventory.Where(x => x.Equipped == false).ToList().Count > 0)
                {
                    inventory.Append("<ul>");
                    var inv = new ItemList();

                    foreach (var item in player.Inventory.Where(x => x.Equipped == false).ToList())
                    {
                        inv.Add(item);
                    }

                    foreach (var item in inv.List(false))
                    {
                        inventory.Append($"<li>{item.Name}</li>");
                    }

                    inventory.Append("</ul>");
                }
                else
                {
                    inventory.Append("<p>Nothing.</p>");
                }

                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("InventoryUpdate", inventory.ToString());
                //     UpdateScore(player);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateTime(Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("UpdateTime", Services.Instance.Time.ReturnTime());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void UpdateContentPopUp(Player player, WriteBook bookContent)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("UpdateContentPopUp", bookContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void PlaySound(string soundName, Player player)
        {
            if (string.IsNullOrEmpty(player.ConnectionId) && !player.IsTelnet)
            {
                return;
            }

            try
            {
                await _hubContext.Clients
                    .Client(player.ConnectionId)
                    .SendAsync("PlaySound", soundName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

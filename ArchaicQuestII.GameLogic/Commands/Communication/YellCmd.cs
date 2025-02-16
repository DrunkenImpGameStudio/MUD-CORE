using System.Linq;
using ArchaicQuestII.GameLogic.Account;
using ArchaicQuestII.GameLogic.Character;
using ArchaicQuestII.GameLogic.Character.Status;
using ArchaicQuestII.GameLogic.Core;
using ArchaicQuestII.GameLogic.World.Room;

namespace ArchaicQuestII.GameLogic.Commands.Communication;

public class YellCmd : ICommand
{
    public YellCmd()
    {
        Aliases = new[] { "yell" };
        Description = "Sends a message to everyone in the area";
        Usages = new[] { "Type: yell 'message'" };
        Title = "";
        DeniedStatus = new[]
        {
            CharacterStatus.Status.Busy,
            CharacterStatus.Status.Dead,
            CharacterStatus.Status.Fleeing,
            CharacterStatus.Status.Incapacitated,
            CharacterStatus.Status.Sleeping,
            CharacterStatus.Status.Stunned
        };
        UserRole = UserRole.Player;
    }

    public string[] Aliases { get; }
    public string Description { get; }
    public string[] Usages { get; }
    public string Title { get; }
    public CharacterStatus.Status[] DeniedStatus { get; }
    public UserRole UserRole { get; }

    public void Execute(Player player, Room room, string[] input)
    {
        if (string.IsNullOrEmpty(input.ElementAtOrDefault(1)))
        {
            Services.Instance.Writer.WriteLine("<p>Yell what?</p>", player);
            return;
        }

        var text = string.Join(" ", input.Skip(1));

        var rooms = Services.Instance.Cache.GetAllRoomsInArea(room.AreaId);

        Services.Instance.Writer.WriteLine(
            $"<p class='yell'>You yell, {text.ToUpper()}</p>",
            player
        );

        foreach (
            var pc in from rm in rooms
            from pc in rm.Players
            where pc.Name != player.Name
            select pc
        )
        {
            Services.Instance.Writer.WriteLine(
                $"<p class='yell'>{player.Name} yells, {text.ToUpper()}</p>",
                pc
            );
            Services.Instance.UpdateClient.UpdateCommunication(
                pc,
                $"<p class='yell'>{player.Name} yells, {text.ToUpper()}</p>",
                "room"
            );
        }

        Services.Instance.UpdateClient.UpdateCommunication(
            player,
            $"<p class='yell'>You yell, {text.ToUpper()}</p>",
            "room"
        );
    }
}

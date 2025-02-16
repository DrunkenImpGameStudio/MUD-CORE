using System.Linq;
using ArchaicQuestII.GameLogic.Account;
using ArchaicQuestII.GameLogic.Character;
using ArchaicQuestII.GameLogic.Character.Status;
using ArchaicQuestII.GameLogic.Core;
using ArchaicQuestII.GameLogic.Effect;
using ArchaicQuestII.GameLogic.Utilities;
using ArchaicQuestII.GameLogic.World.Room;

namespace ArchaicQuestII.GameLogic.Commands.Skills
{
    public class ElbowCmd : SkillCore, ICommand
    {
        public ElbowCmd()
            : base()
        {
            Aliases = new[] { "elbow", "elb" };
            Description =
                "A simple but effective move to elbow the opponent, but a miss makes you vulnerable to attack.";
            Usages = new[]
            {
                "Type: elbow bob - This will start combat with the target if not already in a fight., during a fight only elbow can be used to hit the target."
            };
            DeniedStatus = new[]
            {
                CharacterStatus.Status.Sleeping,
                CharacterStatus.Status.Resting,
                CharacterStatus.Status.Dead,
                CharacterStatus.Status.Mounted,
                CharacterStatus.Status.Stunned
            };
            Title = SkillName.Elbow.ToString();
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
            if (!player.HasSkill(SkillName.Elbow))
                return;

            var obj = input.ElementAtOrDefault(1)?.ToLower() ?? player.Target;
            if (string.IsNullOrEmpty(obj))
            {
                Services.Instance.Writer.WriteLine("Elbow What!?.", player);
                return;
            }

            var target = FindTargetInRoom(obj, room, player);
            if (target == null)
            {
                return;
            }

            var skillSuccess = player.RollSkill(SkillName.Elbow, true, "You miss and stumble.");
            if (!skillSuccess)
            {
                var textToTarget = $"{player.Name} tries to elbow you but stumbles.";
                var textToRoom = $"{player.Name} tries to elbow {target.Name} but stumbles.";
                EmoteAction(textToTarget, textToRoom, target.Name, room, player);
                player.FailedSkill(SkillName.Elbow, true);
                player.Lag += 1;
                return;
            }

            var str = player.Attributes.Attribute[EffectLocation.Strength];
            var damage = DiceBag.Roll(1, 1, 6) + str / 5;

            player.Lag += 1;

            DamagePlayer(SkillName.Elbow.ToString(), damage, player, target, room);
        }
    }
}

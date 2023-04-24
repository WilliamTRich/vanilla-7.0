using RotMG.Game.Entities;
using RotMG.Utils;
using System.Text.RegularExpressions;

namespace RotMG.Game.Logic.Transitions
{
    public class PlayerTextTransition : Transition
    {
        private readonly double? _distSqr;
        public bool _transition;
        private Player _player;
        private readonly Regex _rgx;

        public PlayerTextTransition(string regex, float dist, bool ignoreCase, params string[] targetStates) : base(targetStates)
        {
            if (dist > 0)
                _distSqr = dist * dist;

            _rgx = (ignoreCase)
                ? new Regex(regex, RegexOptions.IgnoreCase)
                : new Regex(regex);
        }
        public PlayerTextTransition(string regex, params string[] targetStates) : base(targetStates)
        {
            var dist = 10;
            if (dist > 0)
                _distSqr = dist * dist;

            _rgx = new Regex(regex, RegexOptions.IgnoreCase);
        }
        public override bool Tick(Entity host)
        {
            if (_transition == false ||
                host.Parent == null ||
                _player == null ||
                !host.Parent.Players.ContainsValue(_player))
            {
                return false;
            }

            if(_distSqr != null)
                return MathUtils.DistanceSquared(_player.Position, host.Position) <= _distSqr;

            return true;
            //return host.GetNearestPlayer(Radius, SeeInvis) != null;
        }
        public void OnChatReceived(Player plr, string txt)
        {
            var match = _rgx.Match(txt);
            if(!match.Success)
            {
                _transition = false;
                _player = null;
                return;
            }
            _transition = true;
            _player = plr;

            Manager.AddTimedAction(1000, () => {
                if(this != null)
                {
                    _transition = false;
                    _player = null;
                }
            });
        }


    }
}
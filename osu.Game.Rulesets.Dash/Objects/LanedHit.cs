// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Dash.Judgements;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Dash.Objects
{
    public class LanedHit : DashHitObject
    {
        public readonly Bindable<LanedHitLane> LaneBindable = new Bindable<LanedHitLane>();

        public virtual LanedHitLane Lane
        {
            get => LaneBindable.Value;
            set => LaneBindable.Value = value;
        }

        public override Judgement CreateJudgement() => new DashJudgement();
    }
}

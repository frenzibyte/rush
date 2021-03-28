// Copyright (c) Shane Woolcock. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Rush.Objects;
using osu.Game.Rulesets.Rush.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Rush.Tests.Visual
{
    public class TestSceneMinion : RushHitObjectTestScene
    {
        protected override DrawableRushHitObject CreateHitObject()
        {
            var note = new Minion();
            note.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

            return new DrawableMinion(note)
            {
                Scale = new Vector2(4f),
            };
        }
    }
}

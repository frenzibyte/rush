// Copyright (c) Shane Woolcock. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Rush.UI;

namespace osu.Game.Rulesets.Rush.Tests.Visual
{
    /// <summary>
    /// A type of container that readies up a basic set of components for testing rush! hit objects.
    /// </summary>
    public class RushTestContainer : Container
    {
        protected override Container<Drawable> Content => content;

        private readonly Container content;

        [Cached]
        private readonly RushPlayfield playfield;

        public RushTestContainer(bool showPlayfield = false)
        {
            InternalChildren = new Drawable[]
            {
                playfield = new RushPlayfield
                {
                    Alpha = showPlayfield ? 1 : 0
                },
                content = new RushInputManager(new RushRuleset().RulesetInfo)
                {
                    RelativeSizeAxes = Axes.Both
                },
            };
        }
    }
}

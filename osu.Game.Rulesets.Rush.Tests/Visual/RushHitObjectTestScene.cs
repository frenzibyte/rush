// Copyright (c) Shane Woolcock. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Rush.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Rush.Tests.Visual
{
    public abstract class RushHitObjectTestScene : OsuTestScene
    {
        protected const double START_TIME = 1000000000;

        [Cached(typeof(IScrollingInfo))]
        private readonly TestScrollingInfo scrollingInfo = new TestScrollingInfo();

        protected override Ruleset CreateRuleset() => new RushRuleset();

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Height = 0.7f,
                Children = new Drawable[]
                {
                    new RushTestContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Y,
                        Width = 80,
                        Child = new ScrollingHitObjectContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                        }.With(c =>
                        {
                            c.Add(CreateHitObject().With(h =>
                            {
                                h.HitObject.StartTime = START_TIME;
                                h.AccentColour.Value = Color4.Orange;
                            }));
                        })
                    },
                }
            };
        });

        protected abstract DrawableRushHitObject CreateHitObject();

        private class TestScrollingInfo : IScrollingInfo
        {
            public readonly Bindable<ScrollingDirection> Direction = new Bindable<ScrollingDirection>();

            IBindable<ScrollingDirection> IScrollingInfo.Direction => Direction;
            IBindable<double> IScrollingInfo.TimeRange { get; } = new Bindable<double>(1000);
            IScrollAlgorithm IScrollingInfo.Algorithm { get; } = new ZeroScrollAlgorithm();
        }

        private class ZeroScrollAlgorithm : IScrollAlgorithm
        {
            public double GetDisplayStartTime(double originTime, float offset, double timeRange, float scrollLength)
                => double.MinValue;

            public float GetLength(double startTime, double endTime, double timeRange, float scrollLength)
                => scrollLength;

            public float PositionAt(double time, double currentTime, double timeRange, float scrollLength)
                => (float)((time - START_TIME) / timeRange) * scrollLength;

            public double TimeAt(float position, double currentTime, double timeRange, float scrollLength)
                => 0;

            public void Reset()
            {
            }
        }
    }
}

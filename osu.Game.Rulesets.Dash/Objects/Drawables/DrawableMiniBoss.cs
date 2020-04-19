// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Dash.Objects.Drawables
{
    public class DrawableMiniBoss : DrawableDashHitObject<MiniBoss>, IKeyBindingHandler<DashAction>
    {
        private const float base_sprite_scale = 6f;
        private const float target_sprite_scale = 7f;

        private readonly Sprite sprite;
        private readonly Container<DrawableMiniBossTick> ticks;

        public DrawableMiniBoss(MiniBoss hitObject)
            : base(hitObject)
        {
            Size = new Vector2(40);
            Origin = Anchor.Centre;

            AddInternal(sprite = new Sprite
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                Scale = new Vector2(base_sprite_scale),
            });

            AddInternal(ticks = new Container<DrawableMiniBossTick> { RelativeSizeAxes = Axes.Both });
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            sprite.Texture = store.Get("cirno");
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableMiniBossTick tick:
                    ticks.Add(tick);
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();
            ticks.Clear();
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case MiniBossTick tick:
                    return new DrawableMiniBossTick(tick);
            }

            return base.CreateNestedHitObject(hitObject);
        }

        protected override void Update()
        {
            base.Update();

            float fraction = (float)(HitObject.StartTime - Clock.CurrentTime) / 500f;
            sprite.Y = (float)(Math.Sin(fraction * 2 * Math.PI) * 5f);

            X = Math.Max(0, X);

            if (Time.Current >= HitObject.StartTime)
                ProxyContent();
            else
                UnproxyContent();
        }

        public override bool OnPressed(DashAction action) => UpdateResult(true);
        // {
        //     if (Time.Current < HitObject.StartTime)
        //         return false;
        //
        //     UpdateResult(true);
        //     return true;
        // }

        public override void OnReleased(DashAction action)
        {
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (userTriggered)
            {
                DrawableMiniBossTick nextTick = null;

                foreach (var t in ticks)
                {
                    if (!t.IsHit)
                    {
                        nextTick = t;
                        break;
                    }
                }

                nextTick?.TriggerResult(HitResult.Great);

                var numHits = ticks.Count(r => r.IsHit);
                var completion = (float)numHits / HitObject.RequiredHits;

                sprite.ScaleTo(base_sprite_scale + Math.Min(target_sprite_scale - base_sprite_scale, (target_sprite_scale - base_sprite_scale) * completion), 260, Easing.OutQuint);

                // TODO: update bonus score somehow?

                // if (numHits == HitObject.RequiredHits)
                //     ApplyResult(r => r.Type = HitResult.Great);
            }
            else
            {
                if (timeOffset < 0)
                    return;

                int numHits = 0;

                foreach (var tick in ticks)
                {
                    if (tick.IsHit)
                    {
                        numHits++;
                        continue;
                    }

                    tick.TriggerResult(HitResult.Miss);
                }

                var hitResult = numHits == HitObject.RequiredHits
                    ? HitResult.Great
                    : numHits > HitObject.RequiredHits / 2
                        ? HitResult.Good
                        : HitResult.Miss;

                ApplyResult(r => r.Type = hitResult);
            }
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Idle:
                    UnproxyContent();
                    break;

                case ArmedState.Miss:
                    this.FadeOut(300);
                    break;

                case ArmedState.Hit:
                    ProxyContent();

                    const float gravity_time = 300;
                    const float gravity_travel_height = 500f;

                    this.RotateTo(-180, gravity_time);
                    this.MoveToY(-gravity_travel_height, gravity_time, Easing.Out)
                        .Then()
                        .MoveToY(gravity_travel_height * 2, gravity_time * 2, Easing.In);

                    this.FadeOut(300);
                    break;
            }
        }
    }
}

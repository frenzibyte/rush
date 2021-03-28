// Copyright (c) Shane Woolcock. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Rush.Objects.Drawables.Pieces
{
    public class MinionPiece : CompositeDrawable
    {
        private Sprite innerTriangle;
        private Container trianglesContainer;
        private Triangles triangles;

        private IBindable<Color4> accentColour;

        [BackgroundDependencyLoader]
        private void load(DrawableHitObject drawableHitObject, TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.8f),
                    Children = new Drawable[]
                    {
                        innerTriangle = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            FillMode = FillMode.Fit,
                            Texture = textures.Get("Minion/rounded_triangle"),
                        },
                        trianglesContainer = new Container
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.55f, 0.65f),
                            Margin = new MarginPadding { Left = 10 },
                            Masking = true,
                            Child = triangles = new Triangles
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                Scale = new Vector2(0.5f),
                            }
                        }
                    }
                },
                new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    Texture = textures.Get("Minion/rounded_triangle_ring"),
                },
            };

            accentColour = drawableHitObject.AccentColour.GetBoundCopy();
            accentColour.BindValueChanged(c =>
            {
                innerTriangle.Colour = c.NewValue;
                triangles.ColourLight = c.NewValue.Lighten(0.1f);
                triangles.ColourDark = c.NewValue.Darken(0.1f);
            }, true);

            trianglesContainer.FillAspectRatio = innerTriangle.FillAspectRatio;
        }

        private class TriangularContainer : Container
        {
            public override RectangleF BoundingBox => toTriangle(ToParentSpace(LayoutRectangle)).AABBFloat;

            private static Framework.Graphics.Primitives.Triangle toTriangle(Quad q) => new Framework.Graphics.Primitives.Triangle(
                (q.TopLeft + q.TopRight) / 2,
                q.BottomLeft,
                q.BottomRight);

            public override bool Contains(Vector2 screenSpacePos) => toTriangle(ScreenSpaceDrawQuad).Contains(screenSpacePos);
        }
    }
}

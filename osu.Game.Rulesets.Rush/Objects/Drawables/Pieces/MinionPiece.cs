// Copyright (c) Shane Woolcock. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
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
        private TriangularContainer trianglesContainer;
        private Triangles triangles;

        private IBindable<Color4> accentColour;

        [BackgroundDependencyLoader]
        private void load(DrawableHitObject drawableHitObject, TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(5f),
                    Colour = Color4.White,
                },
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
                        trianglesContainer = new TriangularContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Texture = Texture.WhitePixel,
                            Colour = Color4.Aqua,
                            //Child = triangles = new Triangles
                            //{
                            //    Anchor = Anchor.Centre,
                            //    Origin = Anchor.Centre,
                            //    RelativeSizeAxes = Axes.Both,
                            //    Scale = new Vector2(0.5f),
                            //}
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
                //triangles.ColourLight = c.NewValue.Lighten(0.1f);
                //triangles.ColourDark = c.NewValue.Darken(0.1f);
            }, true);

            trianglesContainer.FillAspectRatio = innerTriangle.FillAspectRatio;
        }

        private class TriangularContainer : Sprite
        {
            private IShader triangleShader;

            [BackgroundDependencyLoader]
            private void load(ShaderManager shaders)
            {
                TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "Triangle");
                RoundedTextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "Triangle");
            }
        }
    }
}

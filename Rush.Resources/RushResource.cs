// Copyright (c) Shane Woolcock. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Reflection;

namespace Rush.Resources
{
    public static class RushResource
    {
        public static Assembly ResourcesAssembly { get; } = typeof(RushResource).Assembly;
    }
}

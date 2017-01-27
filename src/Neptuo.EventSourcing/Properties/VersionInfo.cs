﻿using System;

namespace Neptuo.EventSourcing
{
    public static class VersionInfo
    {
        internal const string Version = "1.2.0";
        internal const string Preview = "beta1";

        public static Version GetVersion()
        {
            return new Version(Version);
        }
    }
}
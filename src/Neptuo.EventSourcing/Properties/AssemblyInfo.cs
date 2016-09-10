﻿using Neptuo.EventSourcing;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Neptuo.EventSourcing")]
[assembly: AssemblyDescription("Event sourcing support for Models and Events.")]

#if !DEBUG

Fix version attribute!

#endif

[assembly: AssemblyVersion(VersionInfo.Version)]
[assembly: AssemblyInformationalVersion(VersionInfo.Version + "-beta5")]
[assembly: AssemblyFileVersion(VersionInfo.Version)]

[assembly: InternalsVisibleTo("UnitTest.Services")]
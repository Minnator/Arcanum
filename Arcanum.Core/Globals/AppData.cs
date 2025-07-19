﻿using Arcanum.Core.CoreSystems.ProjectFileUtil.Arcanum;
using Arcanum.Core.CoreSystems.ProjectFileUtil.Mod;
using Arcanum.Core.Utils.Git;

namespace Arcanum.Core.Globals;

public static class AppData
{
   internal const string APP_NAME = "Arcanum";
   internal const string APP_VERSION = "1.0.0-beta";

   public static GitDataDescriptor GitDataDescriptor { get; } = new();
   public static ArcanumDataDescriptor DataDescriptor { get; set; } = null!;
   public static MainMenuScreenDescriptor MainMenuScreenDescriptor { get; set; } = null!;
   
   public static ProjectFileDescriptor CurrentProjectFile { get; set; } = null!;
   
}
using Arcanum.Core.Utils.Git;

namespace Arcanum.Core;

public static class AppData
{
   internal const string APP_NAME = "Arcanum";
   internal const string APP_VERSION = "1.0.0-beta";

   public static GitDataDescriptor GitDataDescriptor { get; } = new();

}
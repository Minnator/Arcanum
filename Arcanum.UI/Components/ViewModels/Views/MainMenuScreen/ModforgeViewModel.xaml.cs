using System.Windows;
using Arcanum.Core;
using Arcanum.Core.Utils.Git;

namespace Arcanum.UI.Components.ViewModels.Views.MainMenuScreen;

public partial class ModforgeViewModel
{
   private const string MODFORGE_URL = "https://github.com/Minnator/Minnators-Modforge";

   public ModforgeViewModel()
   {
      InitializeComponent();

      SetReleaseText();
   }

   private void SetReleaseText()
   {
      var latestRelease = AppData.GitDataDescriptor.LatestVersion;
      if (latestRelease is { Data: not null })
      {
         var name = latestRelease.RepositoryName.StartsWith("Minnators-")
                       ? latestRelease.RepositoryName[10..]
                       : latestRelease.RepositoryName;
         LatestReleaseNameTextBox.Text =
            $"{name} {latestRelease.Data.Name.Split(' ').FirstOrDefault(string.Empty)}";

         LatestReleaseVersionTextBox.Text = $"Version {latestRelease.Data.TagName[1..]}";
         
         LatestReleaseNameTextBox.ToolTip = $"Release notes:\n{latestRelease.Data.Body}";
      }
   }
}
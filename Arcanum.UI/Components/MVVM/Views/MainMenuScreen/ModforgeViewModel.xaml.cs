﻿using System.Windows.Input;
using Arcanum.Core.Globals;
using Arcanum.Core.Utils;
using Arcanum.Core.Utils.Git;

namespace Arcanum.UI.Components.MVVM.Views.MainMenuScreen;

public partial class ModforgeViewModel
{
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

   private void DiscordTextBlock_PreviewMouseUp(object sender, MouseButtonEventArgs e)
   {
      if (e.ChangedButton == MouseButton.Left)
      {
         e.Handled = true;
         ProcessHelper.OpenDiscordLinkIfDiscordRunning(GitDataService.MODFORGE_DISCORD_URL);
      }
   }

   private void GitHubTextBlock_PreviewMouseUp(object sender, MouseButtonEventArgs e)
   {
      if (e.ChangedButton == MouseButton.Left)
      {
         e.Handled = true;
         ProcessHelper.OpenLink(GitDataService.MODFORGE_REPOSITORY_URL);
      }
   }
}
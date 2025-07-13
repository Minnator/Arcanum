
using Octokit;
namespace Arcanum.Core.Utils.Git;

public static class ArcanumGit
{
   public static async Task<(string, string)> GetLatestReleaseNameAndVersion()
   {
      Console.WriteLine("Fetching latest release information from GitHub...");
      var client = new GitHubClient(new ProductHeaderValue("Minnator"));
      var latestRelease = await client.Repository.Release.GetLatest("Minnator", "Minnators-Modforge");

      Console.WriteLine($"Found {latestRelease} releases in the repository.");
      
      var latestReleaseName = latestRelease.Name.Split(' ').FirstOrDefault("Unknown Release");
      var tag = latestRelease.TagName.TrimStart('v');
      
      return (latestReleaseName, tag);
   }
}

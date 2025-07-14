using System.Net.Http;
using Octokit;

namespace Arcanum.Core.Utils.Git;

public static class GitDataService
{
   public const string MOD_FORGE_LATEST_VERSION_KEY = "ModForgeLatestVersion";
   private const string GIT_OWNER = "Minnator";
   private const string MOD_FORGE_GIT_REPOSITORY = "Minnators-Modforge";
   private const string ARCANUM_REPOSITORY_URL = "https://github.com/Minnator/Minnators-Modforge";

   public static string GetFileFromRepositoryUrl(string owner, string repository, string branch, string filePath)
   {
      var client = new HttpClient();
      var url = $"https://raw.githubusercontent.com/{owner}/{repository}/{branch}/{filePath}";
      return client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
   }

   public static GitReleaseObject GetLatestVersion()
   {
      var gdo = AppData.GitDataDescriptor.LatestVersion ??
                new()
                {
                   RepositoryName = MOD_FORGE_GIT_REPOSITORY,
                   RepositoryOwner = GIT_OWNER,
                   DataKey = MOD_FORGE_LATEST_VERSION_KEY,
                };

      // We still have data and it is not outdated, return it
      if (gdo.IsDataAvailable() && !gdo.IsDataOutdated)
         return gdo;

      var client = CreateClient();
      var latestRelease = client.Repository.Release.GetLatest(GIT_OWNER, MOD_FORGE_GIT_REPOSITORY).Result;

      Console.WriteLine("GitDataService: Fetched latest release from GitHub");

      gdo.Data = new()
      {
         Name = latestRelease.Name,
         TagName = latestRelease.TagName,
         Body = latestRelease.Body,
      };

      return gdo;
   }

   private static GitHubClient CreateClient() => new(new ProductHeaderValue(GIT_OWNER));
}
namespace Arcanum.UI.Components.MVVM.Models;

public class LoadingScreenModel
{
   public event Action<string>? ProgressChanged;

   public async Task RunLoadingAsync()
   {
      for (var i = 0; i <= 100; i += 10)
      {
         await Task.Delay(200); // simulate work
         ProgressChanged?.Invoke($"Loading step {i / 10 + 1}/10");
      }
   }
}
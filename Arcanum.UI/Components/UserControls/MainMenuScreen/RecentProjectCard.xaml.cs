using System.Windows.Input;
using Arcanum.Core.CoreSystems.ProjectFileUtil.Mod;
using Arcanum.UI.Components.MVVM.Views.MainMenuScreen;

namespace Arcanum.UI.Components.UserControls.MainMenuScreen;

public partial class RecentProjectCard
{
   private readonly MainViewModel _mainViewModel;
   private readonly ProjectFileDescriptor _projectFileDescriptor;
   
   public RecentProjectCard(ProjectFileDescriptor projectFileDescriptor, MainViewModel mainViewModel)
   {
      _projectFileDescriptor = projectFileDescriptor;
      _mainViewModel = mainViewModel;
      InitializeComponent();
      
      ModNameTextBlock.Text = projectFileDescriptor.ModName;
      ModThumbnailImage.Source = projectFileDescriptor.ModThumbnailOrDefault();
   }

   private void CardBorder_MouseUp(object sender, MouseButtonEventArgs e)
   {
      if (e.ChangedButton != MouseButton.Left)
         return;
      
      _ = _mainViewModel.LaunchArcanum(_projectFileDescriptor);
   }
}
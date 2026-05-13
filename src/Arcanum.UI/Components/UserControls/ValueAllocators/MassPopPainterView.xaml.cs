#region

using System.Windows;
using System.Windows.Controls;
using Arcanum.Core.GameObjects.InGame.Cultural;
using Arcanum.Core.GameObjects.InGame.Pops;
using Arcanum.Core.GameObjects.InGame.Religious;

#endregion

namespace Arcanum.UI.Components.UserControls.ValueAllocators;

public sealed partial class MassPopPainterView
{
   public MassPopPainterView()
   {
      InitializeComponent();

      Loaded += (s, e) =>
      {
         ClearCultureTarget_OnClick(s, e);
         ClearReligionTarget_OnClick(s, e);
         ClearPopTypeTarget_OnClick(s, e);
         ClearCultureFilter_OnClick(s, e);
         ClearReligionFilter_OnClick(s, e);
         ClearPopTypeFilter_OnClick(s, e);
      };
   }

   private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      if (DataContext is not MassPopPainterViewModel vm)
         return;

      vm.OnRequestRefreshPreview();
   }

   private void ClearCultureTarget_OnClick(object sender, RoutedEventArgs e)
   {
      if (DataContext is not MassPopPainterViewModel vm)
         return;

      vm.TargetCulture = Culture.Empty;

      CultureTargetBox.SetSelectedItem(Culture.Empty, string.Empty);
   }

   private void ClearReligionTarget_OnClick(object sender, RoutedEventArgs e)
   {
      if (DataContext is not MassPopPainterViewModel vm)
         return;

      vm.TargetReligion = Religion.Empty;
      ReligionTargetBox.SetSelectedItem(Religion.Empty, string.Empty);
   }

   private void ClearPopTypeTarget_OnClick(object sender, RoutedEventArgs e)
   {
      if (DataContext is not MassPopPainterViewModel vm)
         return;

      vm.TargetPopType = PopType.Empty;
      PopTypeTargetBox.SetSelectedItem(PopType.Empty, string.Empty);
   }

   public void ClearCultureFilter_OnClick(object sender, RoutedEventArgs e)
   {
      if (DataContext is not MassPopPainterViewModel vm)
         return;

      vm.SourceCulture = Culture.Empty;
      CultureFilterBox.SetSelectedItem(Culture.Empty, string.Empty);
   }

   public void ClearReligionFilter_OnClick(object sender, RoutedEventArgs e)
   {
      if (DataContext is not MassPopPainterViewModel vm)
         return;

      vm.SourceReligion = Religion.Empty;
      ReligionFilterBox.SetSelectedItem(Religion.Empty, string.Empty);
   }

   public void ClearPopTypeFilter_OnClick(object sender, RoutedEventArgs e)
   {
      if (DataContext is not MassPopPainterViewModel vm)
         return;

      vm.SourcePopType = PopType.Empty;
      PopTypeFilterBox.SetSelectedItem(PopType.Empty, string.Empty);
   }
}
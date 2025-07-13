using Arcanum.Core.CoreSystems.SavingSystem.Util;

namespace Arcanum.Core.CoreSystems.SavingSystem;

public static class SaveMaster
{
   private static List<FileObj> NeedsToBeSaved { get; } = [];
   private static Dictionary<SaveableType, List<ISaveable>> NewSaveables { get; } = [];
   private static readonly Dictionary<SaveableType, int> ModificationCache;

   static SaveMaster()
   {
      var saveableTypes = Enum.GetValues<SaveableType>();
      ModificationCache = new(saveableTypes.Length);
      foreach (var t in saveableTypes)
         ModificationCache.Add(t, 0);
   }

   public static int GetModifiedCount => ModificationCache.Values.Sum();

   public static List<(SaveableType type, int amount)> GetModifiedCounts()
   {
      return ModificationCache
            .Where(kvp => kvp.Value > 0)
            .Select(kvp => (kvp.Key, kvp.Value))
            .ToList();
   }

   public static void HandleNewSaveables()
   {
      // Should check for any new saveables that have been added since the last save
      // check new saveables dictionary

      // Check if the path of the saveable is fixed or can be changed
      // check the file information in the saveable

      // Show all saveables with a selection of current file objects which are valid for the given saveableType
      // TODO how do we find the file objects that are valid for the given saveable type?

      // We need a list of fileObjs that are valid for the given saveableType, so we need a Dictionary of some sorts.
      // Might make sense to have a separate window for this, where valid files are shown and where the user can add another one.
      
      // If the saveable path is fixed, make it grayed out and only displayed with an additional checkbox
   }

   public static void SaveAll(bool onlyModified = true)
   {
      Save([..Enum.GetValues<SaveableType>()], onlyModified);
   }

   public static void Save(List<SaveableType> saveableTypes, bool onlyModified = true)
   {

      // All the files are hashed when read in (maybe directly in the char by char parsing method).
      // Before finally saving the file, the hash is checked against the current file
      // If the hash is different, the file is not saved and a MergeDialog is shown to the user

      // The user should be shown a dialog with all the different saveableTypes
      // All SaveableTypes should be possible to be selected
      // Per default all saveable in the same file should be highlighted, and if clicked all are selected
      // One can override this with an additional button press (alt or ctrl) to select a single saveable
      // The file is read in again and only one file is changed (The base file therefore needs to be same as it was when it was read in (see hashes))

      // But do we really want to separate the new saveables and the old ones?
      // In my opinion, it is better to have a general overview of everything that has been changed.
      // One can select if only added/ only modified, or all saveables should be shown
      // Then the user can select which saveables should be saved
      // This also allows users to save stuff to different files


      // WHAT DO WE NEED TO DO?:
      // 1. Hash files on loading and check them before the saving popup
      // 2. Create the Merging Dialog or at least an interface for it
      // 3. Get all saveables that are modified or added
      // 4. Determine files which are valid for the saving of a saveableType
      // 5. Create and Show the dialog
      // 5.1. One should be able to select modified or added saveables
      // 5.2. Changes to the categories should only affect currently shown saveables
      // 5.3. Checkboxes will affect all the saveables in the same file per default
      // 6. For fast saving, skip the user input except if new saveables are added

      // We need a list of fileObjs that are valid for the given saveableType, so we need a Dictionary of some sorts.
      // Might make sense to have a separate window for this, where valid files are shown and where the user can add another one.

      // When a new fileObj is registered, the dictionary can be constructed based on the saveable type.
      // Would need a dictionary of a list of all the files
      // Needs to update when new files are added

      // Merging issue:
      // We have a tri state: start file, end file, internal state
      // If the start and the end file are identical, we do not really care.
      // However, if we merge, we have to think about the dependencies of the files
      // If we, for example, change the province definitions, then the provinces have to be reloaded
      // But how do we handle a province which was changed? -> also merging ? and continue? But then the user might generate a not valid mod state, which we do not want.
      // This would be okay if we gave instant feedback, which is difficult.
      // At the moment just implement a system which notifies the user that the file he has overridden will be overriden by us and the user should backup the file before saving.


      // Saving:
      // Unless a git system is present, we should not directly override the data of the files. Instead, make a backup
      /*
       How to handle Dependencies

      FileObj is the instance of a single File
      FileInformationProvider more or less defines the behavior of it

         Where to put the Information of the dependency?
         We have to load a folder of files anyway -> But the files should share a FileInformationProvider

      So the FileInformationProvider will have to have the dependency information

      Why not directly make a class for all the saveabletypes
         -> then the dependency can be made as static?
         -> Then we need some sort of static method, which can be inherited and generate a FileObj?
         -> Does that make sense if we have like 50 different file types, or does it not really change since we would need 50 different IFileInformationProvider implementations anyway?
      */
   }

   public static void RegisterFile(FileObj fileObj)
   {

   }
}
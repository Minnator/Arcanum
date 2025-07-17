﻿using Arcanum.Core.CoreSystems.ProjectFileUtil;
using Arcanum.Core.CoreSystems.ProjectFileUtil.Mod;

namespace UnitTests.CoreSystems.ProjectFile;

[TestFixture]
public class ProjectFileDescriptorTests
{
   [Test]
   public void Constructor_InitializesProperties()
   {
      var desc = new ProjectFileDescriptor("TestMod", "TestPath");

      Assert.That(desc.ModName, Is.EqualTo("TestMod"));
      Assert.That(desc.ModPath, Is.EqualTo("TestPath"));
      Assert.That(desc.IsSubMod, Is.False);
      Assert.That(desc.RequiredMods, Is.Empty);
      Assert.That(desc.LastModified, Is.Not.EqualTo(default(DateTime)));
   }

   [Test]
   public void Constructor_WithRequiredMods_InitializesCorrectly()
   {
      var mods = new List<string> { "ReqMod1", "ReqMod2" };
      var desc = new ProjectFileDescriptor("MainMod", string.Empty, mods);

      Assert.That(desc.ModName, Is.EqualTo("MainMod"));
      Assert.That(desc.IsSubMod, Is.True);
      Assert.That(desc.RequiredMods, Is.EqualTo(mods));
   }

   [Test]
   public void UpdateLastModified_ChangesTimestamp()
   {
      var desc = new ProjectFileDescriptor("Test", string.Empty);
      var old = desc.LastModified;

      Thread.Sleep(10); // ensure time difference
      desc.UpdateLastModified();

      Assert.That(desc.LastModified, Is.GreaterThan(old));
   }

   [Test]
   public void Equals_ReturnsTrueForSameData()
   {
      var a = new ProjectFileDescriptor("X", string.Empty, ["A", "B"]);
      var b = new ProjectFileDescriptor("X", string.Empty, ["A", "B"]);

      Assert.That(a, Is.EqualTo(b));
   }

   [Test]
   public void Equals_ReturnsFalseForDifferentData()
   {
      var a = new ProjectFileDescriptor("X", string.Empty, ["A"]);
      var b = new ProjectFileDescriptor("X", string.Empty, []);

      Assert.That(a, Is.Not.EqualTo(b));
   }

   [Test]
   public void GetHashCode_ConsistentWithEquals()
   {
      var a = new ProjectFileDescriptor("X", string.Empty, ["A", "B"]);
      var b = new ProjectFileDescriptor("X", string.Empty, ["A", "B"]);

      Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
   }
}
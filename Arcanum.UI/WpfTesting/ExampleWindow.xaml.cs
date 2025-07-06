namespace Arcanum.UI.WpfTesting;

public partial class ExampleWindow 
{
   public UserSettings CurrentSettings { get; } = new();
   
   public ExampleWindow()
   {
      InitializeComponent();
      DataContext = this;
   }
}

public enum SomeValues
{
   Value1,
   Value2,
   Value3,
   Value4,
   Value5
}

public enum SomeOtherValues
{
   OptionA,
   OptionB,
   OptionC,
   OptionD,
   OptionE
}

public class UserSettings
{
   public SomeValues SelectedValue { get; set; } = SomeValues.Value2;
   public SomeOtherValues SelectedOption { get; set; } = SomeOtherValues.OptionB;
   public string Username { get; set; } = "User123";
   public bool IsAdmin { get; set; } = true;
   public int Age { get; set; } = 30;
   public string FavoriteColor { get; set; } = "Blue";
   public string ProfilePicturePath { get; set; } = "/Assets/profile_picture.png";
   public string Bio { get; set; } = "This is a sample bio for the user.";
   public string Email { get; set; } = "this.example@web.de";
   public string PhoneNumber { get; set; } = "+1234567890";
   public double AccountBalance { get; set; } = 1000.50;
   public float Height { get; set; } = 1.75f; // in meters
   public DateTime DateOfBirth { get; set; } = new(1993, 5, 15);
   public List<string> Hobbies { get; set; } = new()
   {
      "Reading",
      "Gaming",
      "Traveling",
      "Cooking"
   };
}
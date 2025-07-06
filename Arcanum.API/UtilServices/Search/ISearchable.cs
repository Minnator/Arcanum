namespace Arcanum.API.UtilServices.Search;

public interface ISearchable
{
   public string GetNamespace { get; }
   public List<string> SearchTerms { get; set; }
   public void OnSearchSelected();
   public float GetRelevanceScore(string query) => 1f;

}

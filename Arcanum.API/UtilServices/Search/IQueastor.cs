namespace Arcanum.API.UtilServices.Search;

public interface IQueastor
{
   public void AddToIndex(ISearchable item);
   public void RemoveFromIndex(ISearchable item);
   public void ModifyInIndex(ISearchable item, IReadOnlyList<string> oldTerms);

   public List<ISearchable> Search(string query, int maxDistance = 2);
   public List<ISearchable> SearchExact(string query);
}
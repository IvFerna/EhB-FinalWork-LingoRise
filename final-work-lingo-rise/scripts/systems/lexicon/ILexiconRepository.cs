using System.Collections.Generic;

public interface ILexiconRepository
{
    ILexiconEntry GetById(string id);

    IEnumerable<ILexiconEntry> GetAll();

    IEnumerable<ILexiconEntry> GetByCategory(WordCategory category);
}
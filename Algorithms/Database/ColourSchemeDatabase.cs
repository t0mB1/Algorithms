using SQLite;

namespace Algorithms.Database
{
    public class ColourSchemeDatabase
    {
        readonly SQLiteConnection _database;

        public ColourSchemeDatabase(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<ColourSchemeEntity>();
        }

        public int SaveColourSchemeDb(ColourSchemeEntity colourScheme)
        {
            // only 1 entity will exist
            _database.DeleteAll<ColourSchemeEntity>();
            return _database.Insert(colourScheme);
        }

        public int UpdateColourScheme(ColourSchemeEntity entity)
        {
            return _database.Update(entity);
        }

        public ColourSchemeEntity GetColourSchemeDb()
        {
            ColourSchemeEntity[] entities = _database.Table<ColourSchemeEntity>()
                                                     .ToArray();
            if(entities.Length > 0)
            {
                return entities[0];
            }
            return null;
        }
    }
}

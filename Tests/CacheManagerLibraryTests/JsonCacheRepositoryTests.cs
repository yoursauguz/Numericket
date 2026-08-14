using CacheManagerLibrary;

namespace CacheManagerLibraryTests;

[TestFixture]
public class Tests
{

    private class Dummy
    {
        public int Age { get; set; }
        public string? Name { get; set;  }

    }


    private class TestablePersistableStore : AbstractJsonCacheRepository<Dummy>
    {
        private readonly string _mockFilePath;

        public TestablePersistableStore(string mockFilePath)
        {
            _mockFilePath = mockFilePath;
        }

        protected override string GetFilePath()
        {
            return _mockFilePath;
        }
    }


    [TestCase(null, TestName = "LoadAsync -  file path is null. Outcome: ArgumentNullException must be thrown")]
    [TestCase("", TestName = "LoadAsync -  file path is empty. Outcome: ArgumentNullException must be thrown")]
    [TestCase(" ", TestName = "LoadAsync -  file path is whitespace. Outcome: ArgumentNullException must be thrown")]
    public void LoadData_FromEmptyFilePath_MustFail(string? filePath)
    {
        var repository = new TestablePersistableStore(filePath);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.LoadAsync());
    }


    [TestCase(null, TestName = "SaveAsync -  file path is null. Outcome: ArgumentNullException must be thrown")]
    [TestCase("", TestName = "SaveAsync -  file path is empty. Outcome: ArgumentNullException must be thrown")]
    [TestCase(" ", TestName = "SaveAsync -  file path is whitespace. Outcome: ArgumentNullException must be thrown")]
    public void SaveData_ToEmptyFilePath_MustFail(string? filePath)
    {
        var repository = new TestablePersistableStore(filePath);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.SaveAsync());
    }

    [TestCase("/cache/test1.json", TestName = "LoadAsync -  no file in path. Outcome: default object")]
    public async Task LoadData_FromEmptyFile_MustReturnDefaultObject(string? filePath)
    {
        var repository = new TestablePersistableStore(filePath);

        await repository.LoadAsync();

        Assert.Multiple(() =>
        {
            Assert.That(repository.Data, Is.Not.Null);
            Assert.That(repository.Data.Name, Is.EqualTo(null));
            Assert.That(repository.Data.Age, Is.EqualTo(0));
        });
    }

    [TestCase("/cache/test.json", TestName = "Save Async -  no file in path. Outcome: create file with object")]
    public async Task SaveData_ToEmptyFile_MustCreateFile(string? filePath)
    {
        var repository = new TestablePersistableStore(filePath);

        repository.Data = new Dummy { Name = "testName", Age = 20 };

        await repository.SaveAsync();

        await repository.LoadAsync();

        Assert.Multiple(() =>
        {
            Assert.That(repository.Data, Is.Not.Null);
            Assert.That(repository.Data.Name, Is.EqualTo("testName"));
            Assert.That(repository.Data.Age, Is.EqualTo(20));
        });
    }

    [TestCase("/cache/test.json", TestName = "Load Async - valid file available in path. Outcome: load the content inside the file")]
    public async Task LoadData_FromValidFile_MustReturnReadObject(string? filePath)
    {
        var repository = new TestablePersistableStore(filePath);

        await repository.LoadAsync();

        Assert.Multiple(() =>
        {
            Assert.That(repository.Data, Is.Not.Null);
            Assert.That(repository.Data.Name, Is.Not.EqualTo(null));
            Assert.That(repository.Data.Age, Is.Not.EqualTo(0));
        });
    }

    [TestCase("/cache/test.json", TestName = "Clear Data - valid file available in path. Outcome: clears the in memory data")]
    public async Task ClearData_ClearFromAppMemory_MustReturnDefaultObject(string? filePath)
    {
        var repository = new TestablePersistableStore(filePath);

        await repository.LoadAsync();

        repository.ClearCache();

        Assert.Multiple(() =>
        {
            Assert.That(repository.Data, Is.Not.Null);
            Assert.That(repository.Data.Name, Is.EqualTo(null));
            Assert.That(repository.Data.Age, Is.EqualTo(0));
        });
    }

    [TestCase("/cache/test.json", TestName = "Delete cache - valid file available in path. Outcome: deletes the content in the file and replace it with default value")]
    public async Task DeleteCache_ClearFromAppMemoryAndSavedFile_MustReturnDefaultObject(string? filePath)
    {
        var repository = new TestablePersistableStore(filePath);

        await repository.LoadAsync();

        await repository.DeleteCacheAsync();

        await repository.LoadAsync();

        Assert.Multiple(() =>
        {
            Assert.That(repository.Data, Is.Not.Null);
            Assert.That(repository.Data.Name, Is.EqualTo(null));
            Assert.That(repository.Data.Age, Is.EqualTo(0));
        });
    }
}

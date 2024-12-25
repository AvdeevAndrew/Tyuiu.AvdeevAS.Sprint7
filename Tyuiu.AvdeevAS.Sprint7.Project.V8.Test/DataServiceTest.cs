using Tyuiu.AvdeevAS.Sprint7.Project.V8.Lib;
namespace Tyuiu.AvdeevAS.Sprint7.Project.V8.Test
{
    [TestClass]
    public class DataServiceTest
    {
        private DataService _dataService;
        private string _testFilePath;

        [TestInitialize]
        public void Setup()
        {
            _dataService = new DataService();
            _testFilePath = Path.Combine(Path.GetTempPath(), "test_data.csv");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }


        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var expectedData = new List<string[]> {
                new[] { "�164��72", "Toyota", "��������" },
                new[] { "�832��72", "Honda", "�������" }
            };

            File.WriteAllLines(_testFilePath, new[] {
                "�164��72;Toyota;��������",
                "�832��72;Honda;�������"
            });

            // Act
            var actualData = _dataService.LoadData(_testFilePath);

            // Assert
            Assert.AreEqual(expectedData.Count, actualData.Count);
            for (int i = 0; i < expectedData.Count; i++)
            {
                CollectionAssert.AreEqual(expectedData[i], actualData[i]);
            }
        }
    }
}
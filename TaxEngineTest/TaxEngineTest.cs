using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaxEngineAPI.Entity;
using TaxEngineAPI.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MappingCommon;
using TaxMappingRepository.Models;
using TaxMappingRepository;
using TaxEngineMongoDbRepository;
using TaxEngineMongoDbRepository.Model;
using TaxEngineMongoDbCommon;

namespace TestTaxEngineAPI
{
    public class TaxEngineTest : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private static IServiceProvider _serviceProvider;
        private static IServiceCollection _serviceCollection = new ServiceCollection();
        private static IConfiguration _configuration;

        private static readonly string TaxEngineConnectionString = @"TaxEngineDatabase";
        private static readonly string TaxMappingConnectionString = @"MappingDb";

        public TaxEngineTest(ITestOutputHelper output)
        {
            _output = output;
            ConfigureFromJson();
            ConfigureServices();
        }

        public void Dispose()
        {
            _output.WriteLine(@"");
            _output.WriteLine(@"++++++++++++++++++++++++++++++++++++++++");
            _output.WriteLine(@"Disposing AvTaxEngineTest");

            ((IDisposable)_serviceProvider)?.Dispose();
        }

        private void ConfigureFromJson()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile(@"appsettings.json", false);

            _configuration = builder.Build();
        }

        private void ConfigureServices()
        {
            // appsettings.json configuration
            _serviceCollection.AddSingleton<IConfiguration>(_configuration);

            // logger
            _serviceCollection
                .AddLogging(cfg => cfg.AddConsole())
                .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Debug);

            // TaxEngine DB context
            _serviceCollection
                .AddEntityFrameworkSqlServer()
                .AddDbContext<TaxEngineDbContext>(
                    opt =>
                        opt.UseSqlServer(
                            _configuration.GetConnectionString(TaxEngineConnectionString)
                        ),
                    ServiceLifetime.Transient
                );

            // AvTaxEngine data repository
            _serviceCollection.AddTransient<ITaxEngineRepository, TaxEngineRepository>();

            // ShapeTaxRegionDb
            _serviceCollection
                .AddEntityFrameworkSqlServer()
                .AddDbContext<MappingDataContext>(
                    opt =>
                        opt.UseSqlServer(
                            _configuration.GetConnectionString(TaxMappingConnectionString)
                        ),
                    ServiceLifetime.Transient
                );

            // ShapeTaxRegionRepository
            _serviceCollection.AddTransient<IMappingRepository, MappingRepository>();

            // AvTaxEngine mongoDB data repository
            _serviceCollection.AddTransient<ITtrRepository, TtrRepository>();

   

            // build DI service provider
            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void GetTaxRegion()
        {
            // Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();

            // Act
            var taxRegion = repo.GetTaxRegion(@"CAN-ON");
            _output.WriteLine(
                $"Tax region {taxRegion.TaxRegionCode} [{taxRegion.Id}], parent region is {taxRegion.ParentRegion.TaxRegionCode}"
            );

            // Assert
            Assert.Equal("56F73913-9994-4A0F-8EA8-8DF01EB757CC", taxRegion.Id);
        }

        [Fact]
        public void GetTaxRegionChain()
        {
            // Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();

            // Act
            IList<TaxRegion> taxRegions = repo.GetTaxRegionChain(@"CAN-ON");
            foreach (var taxRegion in taxRegions)
            {
                _output.WriteLine(
                    $"Tax region {taxRegion.TaxRegionCode} [{taxRegion.Id}], parent region is {taxRegion.ParentRegion?.TaxRegionCode}"
                );
            }

            // Assert
            Assert.Equal(3, taxRegions.Count);
        }

        [Fact]
        public void GetTaxRegionChainWithDate()
        {
            // Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();
            int taxCodeCount = 0;

            // Act
            IList<TaxRegion> taxRegions = repo.GetTaxRegionChain(
                @"CAN-ON",
                new DateTime(2019, 01, 20)
            );
            foreach (var taxRegion in taxRegions)
            {
                _output.WriteLine(
                    $"Tax region {taxRegion.TaxRegionCode} [{taxRegion.Id}], parent region is {taxRegion.ParentRegion?.TaxRegionCode}"
                );
                foreach (var taxCodeRegion in taxRegion.TaxCodeRegions)
                {
                    _output.WriteLine(
                        $"  Tax code {taxCodeRegion.TaxCode.Code} [{taxCodeRegion.TaxCode.Description}]"
                    );
                }

                taxCodeCount = taxCodeCount + taxRegion.TaxCodes.Count;
            }

            // Assert
            Assert.Equal(10, taxCodeCount);
        }

        [Fact]
        public void GetItemTaxGroupTaxCode()
        {
            // Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();

            // Act
            var taxCodeSet = repo.GetItemTaxGroupTaxCode("JET", new DateTime(2020, 01, 20));
            foreach (var item in taxCodeSet)
            {
                _output.WriteLine($"Tax code {item.Code}");
            }
            _output.WriteLine($"Total {taxCodeSet.Count} tax codes.");

            // Assert
            Assert.Equal(28, taxCodeSet.Count);
        }

        [Fact]
        public void GetItemTaxGroupIntersectTaxRegionCode()
        {
            // Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();
            var txDate = new DateTime(2020, 01, 20);

            // Act
            var taxRegionChain = repo.GetTaxRegionChain("CAN-ON", txDate);

            taxRegionChain = repo.ApplyItemTaxGroup(taxRegionChain, "JET", txDate);
            taxRegionChain = repo.ApplyTaxCertificate(taxRegionChain, "WORCA", txDate);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void GetAttributeTagById()
        {
            // Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();
            var a1 = repo.FindAttributeTag("#tagB");
            var a2 = repo.FindAttributeTag("NA");

            // Act
            bool result = a1.Id == "44FF112B-AE5E-46DC-9B49-F72AAC6BF264" && a2 == null;

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void GetTaxCertificateById()
        {
            // Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();
            var c1 = repo.FindTaxCertificate("TST-CAN-A");

            var c2 = repo.FindTaxCertificate("NOT FOUND");

            // Act
            bool result = c1.Id == "81019D55-DF64-4D1A-871A-0F756A64C5C9" && c2 == null;

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void GetTaxRegionChildren_ReturnsCorrectType()
        {
            //Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();
            string taxRegionCode = null;

            //Act
            var taxRegionChildren = repo.GetTaxRegionChildren(taxRegionCode);

            _output.WriteLine($" Returned type is {taxRegionChildren.GetType()}");
            //Assert
            Assert.IsType<List<TaxRegion>>(taxRegionChildren);
        }

        [Fact]
        public void GetTaxRegionChildren_ReturnsCorrectValue_ForNoTaxRegionCodeEntry()
        {
            // Arrange
            string taxRegionCode = null;
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();

            //Act
            IList<TaxRegion> taxRegions = repo.GetTaxRegionChildren(taxRegionCode);
            foreach (var taxRegion in taxRegions)
            {
                _output.WriteLine(
                    $"Tax region {taxRegion.TaxRegionCode} [{taxRegion.Id}], parent region is {taxRegion.ParentRegion?.TaxRegionCode}"
                );
            }

            //Assert
            Assert.Equal(1, taxRegions.Count);
        }

        [Fact]
        public void GetTaxRegionChildren_ReturnsCorrectValue_ForNonExistentTaxRegion()
        {
            // Arrange
            string taxRegionCode = "nontExitRegion";
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();

            //Act
            IList<TaxRegion> taxRegions = repo.GetTaxRegionChildren(taxRegionCode);

            //Assert
            Assert.Equal(1, taxRegions.Count);
        }

        [Fact]
        public void GetGeoJsonByShapeId_ReturnsCorrectType_ForExistentShape()
        {
            //Arrange
            string id = "557";
            var repo = _serviceProvider.GetService<ITtrRepository>();

            //Act
            var polygon = repo.TtrGetGeoJsonByShapeId(id);
            _output.WriteLine("Json Response:");
            _output.WriteLine(polygon);

            //Assert
            Assert.IsType<string>(polygon);
        }

        [Fact]
        public void GetGeoJsonByShapeId_ReturnsCorrectType_ForNonExistentShape()
        {
            //Arrange
            string id = "nonExistingId";
            var repo = _serviceProvider.GetService<ITtrRepository>();

            //Act
            var json = repo.TtrGetGeoJsonByShapeId(id);
            var result = json == null ? null : json;
            _output.WriteLine($"Json response is null: {result}");

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetRateById_ReturnCorrectType_ForExistentRateId()
        {
            //Arrange
            string lat = "44.74140167";
            string lng = "-85.5821991";

            var repo = _serviceProvider.GetService<ITtrRepository>();

            //Act
            var json = repo.TtrGetRateByCoordinate(lat, lng);
            _output.WriteLine("Json Response:");
            _output.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented));

            //Assert
            Assert.IsType<TtrRate>(json);
        }

        [Fact]
        public void GetRateById_ReturnCorrectType_ForNonExistentRateId()
        {
            //Arrange
            string lat = "invalidLat";
            string lng = "invalidLng";
            var repo = _serviceProvider.GetService<ITtrRepository>();

            //Act
            var json = repo.TtrGetRateByCoordinate(lat, lng);

            if (json == null)
            {
                _output.WriteLine($"Json response is null");
            }

            //Assert
            Assert.Null(json);
        }

        [Fact]
        public void GetAssociation_ReturnCorrectType_ForExistentShapeId()
        {
            //Arrange
            string shapeId = "814981";
            string source = "TTR";

            var repo = _serviceProvider.GetService<IMappingRepository>();

            //Act

            var association = repo.GetMapping(shapeId, source);
            var json = JsonConvert.SerializeObject(association, Formatting.Indented);
            _output.WriteLine(json);

            // Assert
            Assert.IsType<Mapping>(association);
        }

        [Fact]
        public void GetAssociation_ReturnNull_ForNonExistingShapeId()
        {
            //Arrange
            string shapeId = "nonExistingId";
            string source = "TTR";

            var repo = _serviceProvider.GetService<IMappingRepository>();

            //Act
            var association = repo.GetMapping(shapeId, source);

            // Assert
            Assert.Null(association);
        }

        [Fact]
        public void CreateAssociation()
        {
            // Arrange

            var repo = _serviceProvider.GetService<IMappingRepository>();

            var elementsCountPrior = _serviceCollection
                .AddEntityFrameworkSqlServer()
                .AddDbContext<MappingDataContext>(
                    opt =>
                        opt.UseSqlServer(
                            _configuration.GetConnectionString(TaxMappingConnectionString)
                        ),
                    ServiceLifetime.Transient
                )
                .Count();

            var randomNum = new Random();
            var shapeId = randomNum.Next(1, 1000000).ToString();

            var association = new MappingInput() { ShapeId = shapeId };
            var source = "TTR";

            //Act
            repo.AddMapping(association, source);

            var elementsCountAfter = _serviceCollection
                .AddEntityFrameworkSqlServer()
                .AddDbContext<MappingDataContext>(
                    opt =>
                        opt.UseSqlServer(
                            _configuration.GetConnectionString(TaxMappingConnectionString)
                        ),
                    ServiceLifetime.Transient
                )
                .Count();

            bool result = elementsCountAfter > elementsCountPrior;
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TtrGetSearchOptions()
        {
            //Arrange
            var repo = _serviceProvider.GetService<ITtrRepository>();

            //Act
            var listOptions = repo.TtrGetCordinatesByStateName("Florida");

            _output.WriteLine($"Count search options : {listOptions.Count()}");

            //Assert
            Assert.IsType<List<TtrSearchOption>>(listOptions);
        }


        [Fact]
        public void TtrGetStates()
        {
            //Arrange
            var repo = _serviceProvider.GetService<ITtrRepository>();

            //Act
            var listOptions = repo.TtrGetStates();

            _output.WriteLine($"Count states : {listOptions.Count()}");

            //Assert
            Assert.IsType<List<string>>(listOptions);
        }

        [Fact]
        public void GetCoordinateByIcao_ReturnDicitonary_ForExistingIcao()
        {
            //Arrange
            var repo = _serviceProvider.GetService<ITaxEngineRepository>();
            var codeName = "KPNS";

            //Act
            var coordinates = repo.GetCoordinateByIcao(codeName);

            //Assert
            Assert.IsType<Dictionary<string, string>>(coordinates);
        }
    }
}

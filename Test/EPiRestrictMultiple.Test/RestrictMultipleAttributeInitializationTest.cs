namespace EPiRestrictMultiple.Test
{
    using System.Globalization;
    using EPiServer;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.Framework.Localization;
    using EPiServer.ServiceLocation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RestrictMultipleAttributeInitializationTest
    {
        /// <summary>
        /// Test initialize.
        /// </summary>
        [TestInitialize()]
        public void TestInitialize()
        {
            var contentEventsMock = new Mock<IContentEvents>();

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            var contentRepositoryMock = new Mock<IContentRepository>();

            var contentModelUsageMock = new Mock<IContentModelUsage>();

            var localizationService = new MemoryLocalizationService();
            localizationService.AddString(new CultureInfo("en"), "/RestrictMultiple/max", " Max Error");
            localizationService.AddString(new CultureInfo("en"), "/RestrictMultiple/culturemax", "CultureMax Error");

            var serviceLocatorMock = new Mock<IServiceLocator>();

            serviceLocatorMock.Setup(x => x.GetInstance<IContentEvents>()).Returns(contentEventsMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<IContentTypeRepository>()).Returns(contentTypeRepositoryMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<IContentRepository>()).Returns(contentRepositoryMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<IContentModelUsage>()).Returns(contentModelUsageMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<MemoryLocalizationService>()).Returns(localizationService);

            ServiceLocator.SetLocator(serviceLocatorMock.Object);
        }

        /// <summary>
        /// Test cleanup.
        /// </summary>
        [TestCleanup()]
        public void TestCleanup()
        {
        }

    }
}

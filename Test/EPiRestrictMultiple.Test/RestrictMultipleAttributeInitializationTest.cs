namespace EPiRestrictMultiple.Test
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using EPiServer;
    using EPiServer.Core;
    using EPiServer.DataAbstraction;
    using EPiServer.Framework.Initialization;
    using EPiServer.Framework.Localization;
    using EPiServer.ServiceLocation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class RestrictMultipleAttributeInitializationTest
    {
        /// <summary>
        /// The _content events mock
        /// </summary>
        private Mock<IContentEvents> _contentEventsMock;

        /// <summary>
        /// Test initialize.
        /// </summary>
        [TestInitialize()]
        public void TestInitialize()
        {
            // IContentEvents
            _contentEventsMock = new Mock<IContentEvents>();

            // IContentTypeRepository
            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(x => x.Load(1)).Returns(new ContentType());

            // IContentModelUsage
            var contentModelUsageMock = new Mock<IContentModelUsage>();

            var usages = new List<ContentUsage>();
            var contentUsage = new ContentUsage();
            var contentReference = new ContentReference();
            contentReference.ID = 1;
            contentUsage.ContentLink = contentReference;
            contentModelUsageMock.Setup(x => x.ListContentOfContentType(It.IsAny<ContentType>())).Returns(new List<ContentUsage>() { contentUsage });

            // IContentRepository
            var contentRepositoryMock = new Mock<IContentRepository>();

            contentRepositoryMock.Setup(x => x.GetItems(It.IsAny<IEnumerable<ContentReference>>(), LanguageSelector.MasterLanguage())).Returns(new List<IContent>());

            // LocalizationService
            var localizationService = new MemoryLocalizationService();
            localizationService.AddString(new CultureInfo("en"), "/RestrictMultiple/max", " Max Error");
            localizationService.AddString(new CultureInfo("en"), "/RestrictMultiple/culturemax", "CultureMax Error");

            // IServiceLocator
            var serviceLocatorMock = new Mock<IServiceLocator>();

            serviceLocatorMock.Setup(x => x.GetInstance<IContentEvents>()).Returns(_contentEventsMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<IContentTypeRepository>()).Returns(contentTypeRepositoryMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<IContentRepository>()).Returns(contentRepositoryMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<IContentModelUsage>()).Returns(contentModelUsageMock.Object);
            serviceLocatorMock.Setup(x => x.GetInstance<LocalizationService>()).Returns(localizationService);

            ServiceLocator.SetLocator(serviceLocatorMock.Object);
        }

        /// <summary>
        /// Test cleanup.
        /// </summary>
        [TestCleanup()]
        public void TestCleanup()
        {
        }

        /// <summary>
        /// Determines whether this instance [can not create more than one page].
        /// </summary>
        [TestMethod]
        public void CanNotCreateMoreThanOnePage()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Initialize(initializationEngine);

            var content = new Mock<TestPage>();
            content.Setup(x => x.ContentTypeID).Returns(1);

            var contentLink = new ContentReference(1);
            var contentEventArgs = new ContentEventArgs(contentLink, content.Object);

            _contentEventsMock.Raise(x => x.CreatingContent += null, contentEventArgs);

            Assert.IsTrue(contentEventArgs.CancelAction);
            Assert.IsInstanceOfType(contentEventArgs.CancelReason, typeof(string));

            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
        }

        /// <summary>
        /// Determines whether this instance can initialize.
        /// </summary>
        [TestMethod]
        public void CanInitialize()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
        }

        /// <summary>
        /// Determines whether this instance can uninitialize.
        /// </summary>
        [TestMethod]
        public void CanUninitialize()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
        }

        /// <summary>
        /// Determines whether this instance [can initialize and uninitialize].
        /// </summary>
        [TestMethod]
        public void CanInitializeAndUninitialize()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
        }

        /// <summary>
        /// Determines whether this instance [can initialize two times].
        /// </summary>
        [TestMethod]
        public void CanInitializeTwoTimes()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
        }

        /// <summary>
        /// Determines whether this instance [can uninitialize two times].
        /// </summary>
        [TestMethod]
        public void CanUninitializeTwoTimes()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
        }

        /// <summary>
        /// Determines whether this instance [can initialize and uninitialize two times].
        /// </summary>
        [TestMethod]
        public void CanInitializeAndUninitializeTwoTimes()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
        }

        /// <summary>
        /// Determines whether this instance [can initialize and uninitialize multiple times].
        /// </summary>
        [TestMethod]
        public void CanInitializeAndUninitializeMultipleTimes()
        {
            var initializationEngine = new InitializationEngine();
            var restrictMultipleAttributeInitialization = new RestrictMultipleAttributeInitialization();

            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
            restrictMultipleAttributeInitialization.Initialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
            restrictMultipleAttributeInitialization.Uninitialize(initializationEngine);
        }

        [RestrictMultiple]
        public class TestPage : PageData
        {

        }
    }

    
}

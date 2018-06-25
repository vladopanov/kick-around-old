using KickAround.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KickAround.Web.Controllers;
using TestStack.FluentMVCTesting;
using KickAround.Services.Contracts;

namespace KickAround.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [TestInitialize]
        public void Init()
        {
            IHomeService service = new HomeService();
            this._controller = new HomeController(service);
        }

        [TestMethod]
        public void Welcome_ShouldPass()
        {
            _controller.WithCallTo(c => c.Welcome()).ShouldRenderDefaultView();
        }

        [TestMethod]
        public void Dashboard_ShouldPass()
        {
            _controller.WithCallTo(c => c.Feedback()).ShouldRenderDefaultView();
        }

        [TestMethod]
        public void Feedback_ShouldPass()
        {
            _controller.WithCallTo(c => c.Feedback()).ShouldRenderDefaultView();
        }
    }
}

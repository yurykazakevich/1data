using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Borigran.OneData.WebApi.Pipeline.ExceptionHandling;
using Borigran.OneData.WebApi.Pipeline.ExceptionHandling.Handlers;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Borigran.OneData.WebApi.Tests.Pipeline.ExceptionHandling
{
    public class ExceptionHandlerFactoryTests
    {
        [AttributeUsage(AttributeTargets.Method)]
        private class CustomAutoDataAttribute : AutoDataAttribute
        {
            public CustomAutoDataAttribute() 
                : base(CreateFixture) { }

            private static IFixture CreateFixture()
            {
                var fixture = new Fixture();

                fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true, GenerateDelegates = true });

                var serviceProviderMoq = fixture.Freeze<Mock<IServiceProvider>>();
                var defaultExceptionHandler = fixture.Freeze<DefaultExceptionHandler>();
                var validationExceptionHandler = fixture.Freeze<ValidationExceptionHandler>();

                serviceProviderMoq.Setup(x => x.GetService(It.IsAny<Type>()))
                    .Returns(defaultExceptionHandler);

                serviceProviderMoq.Setup(x => x.GetService(typeof(IExceptionHandler<Exception>)))
                .Returns(defaultExceptionHandler);

                serviceProviderMoq.Setup(x => x.GetService(typeof(IExceptionHandler<ValidationException>)))
                    .Returns(validationExceptionHandler);

                return fixture;
            }
        }

        private class TestUnknownException : Exception
        {

        }

        [Test, CustomAutoData]
        public void GetHandlerForKnownException_Test(
            ExceptionHandlerFactory sut)
        {
            var handler = sut.GetHandler(new ValidationException("Test"));

            Assert.IsInstanceOf<ValidationExceptionHandler>(handler);
        }

        [Test, CustomAutoData]
        public void GetHandlerForUnknownException_Test(
            ExceptionHandlerFactory sut)
        {
            var handler = sut.GetHandler(new TestUnknownException());

            Assert.IsInstanceOf<DefaultExceptionHandler>(handler);
        }

        [Test, CustomAutoData]
        public void GetHandlerForException_Test(
            ExceptionHandlerFactory sut)
        {
            var handler = sut.GetHandler(new Exception("Test"));

            Assert.IsInstanceOf<DefaultExceptionHandler>(handler);
        }
    }
}

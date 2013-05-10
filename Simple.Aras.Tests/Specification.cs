using System;
using NUnit.Framework;

namespace Simple.Aras.Tests
{
    public abstract class Specification<TSut>
    {
        protected Exception ThrownException;
        protected TSut Sut;

        [TestFixtureSetUp]
        public void InitSpecification()
        {
            ThrownException = new NoExceptionThrownException();
            
            Setup();

            try
            {
                RunSpecification();
            }
            catch (Exception ex)
            {
                ThrownException = ex;
            }
            
        }

        virtual protected void Setup()
        {
            
        }

        protected abstract void RunSpecification();
    }
}
using System;

namespace Simple.Aras.Tests
{
    public abstract class QuerySpecification<TSut, TResult> : Specification<TSut>
    {
        protected TResult Result;
        protected override void RunSpecification()
        {
            Sut = Given();
            Result = When()(Sut);
        }

        protected abstract TSut Given();
        protected abstract Func<TSut, TResult> When();
    }
}
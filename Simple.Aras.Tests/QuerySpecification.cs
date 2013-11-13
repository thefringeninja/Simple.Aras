using System;
using System.Threading.Tasks;

namespace Simple.Aras.Tests
{
    public abstract class QuerySpecification<TSut, TResult> : Specification<TSut>
    {
        protected TResult Result;
        protected override async Task RunSpecification()
        {
            Sut = Given();
            Result = await When()(Sut);
        }

        protected abstract TSut Given();
        protected abstract Func<TSut, Task<TResult>> When();
    }
}
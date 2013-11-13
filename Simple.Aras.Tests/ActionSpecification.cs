using System;
using System.Threading.Tasks;

namespace Simple.Aras.Tests
{
    public class Unit{}
    public abstract class ActionSpecification<TSut> : ActionSpecification<TSut, Unit>
    {
        
    }
    public abstract class ActionSpecification<TSut, TRet> : Specification<TSut>
    {
        protected TRet Result;
        protected override async Task RunSpecification()
        {
            Sut = Given();

            Result = await When()(Sut);
        }

        protected abstract TSut Given();

        protected virtual Func<TSut, Task<TRet>> When()
        {
            return sut => Task.FromResult(default(TRet));
        }
    }
}
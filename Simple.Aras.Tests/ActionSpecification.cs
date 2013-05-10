using System;

namespace Simple.Aras.Tests
{
    public class Unit{}
    public abstract class ActionSpecification<TSut> : ActionSpecification<TSut, Unit>
    {
        
    }
    public abstract class ActionSpecification<TSut, TRet> : Specification<TSut>
    {
        protected TRet Result;
        protected override void RunSpecification()
        {
            Sut = Given();

            Result = When()(Sut);
        }

        protected abstract TSut Given();

        protected virtual Func<TSut, TRet> When()
        {
            return sut => default(TRet);
        }
    }
}
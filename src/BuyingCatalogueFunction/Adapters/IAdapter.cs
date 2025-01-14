﻿namespace BuyingCatalogueFunction.Adapters
{
    public interface IAdapter<in TSource, out TTarget>
    {
        TTarget Process(TSource input);
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.AppCenter.Storage
{
    public interface IStorageAdapter
    {
        Task InitializeStorageAsync();
        Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> pred, int limit) where T : new();
        Task CreateTableAsync<T>() where T : new();
        Task<int> CountAsync<T>(Expression<Func<T, bool>> pred) where T : new();
        Task<int> InsertAsync<T>(T val) where T : new();
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> pred) where T : new();
    }
}

using Raven.Client.Documents;
using Raven.Client.Documents.Changes;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Caches.Internal
{
  public abstract class InternalEntityCache<T> : InternalEntityCache<T, T>, IInternalEntityCache<T> where T : ZeroIdEntity
  {
    public InternalEntityCache(IZeroStore store, bool isCore = true) : base(store, isCore) { }
  }

  public abstract class InternalEntityCache<TProjection, T> : IInternalEntityCache<TProjection, T>, IDisposable where T : ZeroIdEntity where TProjection : ZeroIdEntity
  {
    protected IZeroStore Store { get; private set; }
    protected ConcurrentBag<TProjection> _cache = new();
    protected bool _isDirty = true;
    protected bool _isCore = true;
    IDatabaseChanges _subscription = null;


    public InternalEntityCache(IZeroStore store, bool isCore = true)
    {
      Store = store;

      _isCore = isCore;
      _subscription = store.Changes(isCore ? store.CoreDatabase : store.ResolvedDatabase);
      _subscription
        .ForDocumentsInCollection<T>()
        .Subscribe(change => Refresh(change));
    }

    /// <inheritdoc />
    public bool IsDirty => _isDirty;


    /// <inheritdoc />
    public ConcurrentBag<TProjection> All
    {
      get
      {
        CheckIntegrity();
        return _cache;
      }
    }


    /// <inheritdoc />
    public TProjection ById(string id)
    {
      if (id.IsNullOrEmpty())
      {
        return default;
      }
      CheckIntegrity();
      return _cache.FirstOrDefault(x => x.Id == id);
    }


    /// <inheritdoc />
    public void Dispose()
    {
      _subscription?.Dispose();
    }


    void CheckIntegrity()
    {
      if (_isDirty || _cache.IsEmpty)
      {
        Refresh();
      }
    }


    /// <summary>
    /// Refreshes the application cache
    /// </summary>
    protected virtual void Refresh(DocumentChange change = null)
    {
      _isDirty = true;

      using IDocumentSession session = Store.OpenSession(_isCore ? Store.CoreDatabase : Store.ResolvedDatabase);
      List<TProjection> items = session.Query<T>().ProjectInto<TProjection>().ToList();

      _cache.Clear();

      foreach (TProjection item in items)
      {
        _cache.Add(item);
      }

      _isDirty = false;
    }
  }


  public interface IInternalEntityCache<T> : IInternalEntityCache<T, T> where T : ZeroIdEntity { }


  public interface IInternalEntityCache<TProjection, T> : IDisposable where T : ZeroIdEntity where TProjection : ZeroIdEntity
  {
    /// <summary>
    /// Get all registered items
    /// </summary>
    ConcurrentBag<TProjection> All { get; }

    /// <summary>
    /// Get a registered items by ID
    /// </summary>
    TProjection ById(string id);
  }
}

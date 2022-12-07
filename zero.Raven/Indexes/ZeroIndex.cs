using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Spatial;
using Raven.Client.Documents.Operations.Attachments;
using System.Linq.Expressions;

namespace zero.Raven;


public abstract class ZeroJavascriptIndex : AbstractJavaScriptIndexCreationTask, IZeroIndexDefinition
{
  public ZeroJavascriptIndex() { Create(); }
  protected virtual void Create() { }

  public virtual void Setup(IZeroOptions options, IDocumentStore store) { }

  public new string Reduce { get => base.Reduce; set => base.Reduce = value; }
  public new string OutputReduceToCollection { get => base.OutputReduceToCollection; set => base.OutputReduceToCollection = value; }
  public new string PatternReferencesCollectionName { get => base.PatternReferencesCollectionName; set => base.PatternReferencesCollectionName = value; }
  public new string PatternForOutputReduceToCollectionReferences { get => base.PatternForOutputReduceToCollectionReferences; set => base.PatternForOutputReduceToCollectionReferences = value; }

  // AbstractIndexCreationTask
  public new IJsonObject AsJson(object doc) => base.AsJson(doc);
  public new IEnumerable<AttachmentName> AttachmentsFor(object doc) => base.AttachmentsFor(doc);
  public new IEnumerable<string> CounterNamesFor(object doc) => base.CounterNamesFor(doc);
  public new IJsonObject.IMetadata MetadataFor(object doc) => base.MetadataFor(doc);
  public new IEnumerable<string> TimeSeriesNamesFor(object doc) => base.TimeSeriesNamesFor(doc);

  // AbstractIndexCreationTaskBase<TIndexDefinition>
  public new object CreateField(string name, object value, CreateFieldOptions options) => base.CreateField(name, value, options);
  public new object CreateField(string name, object value, bool stored, bool analyzed) => base.CreateField(name, value, stored, analyzed);
  public new object CreateField(string name, object value) => base.CreateField(name, value);

  // AbstractCommonApiForIndexes
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, SortedSet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ISet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IList<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult[]> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, List<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ICollection<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IEnumerable<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, HashSet<TResult>> func) => base.Recurse(source, func);
  public new T? TryConvert<T>(object value) where T : struct => base.TryConvert<T>(value);
}

public abstract class ZeroMultiMapIndex : AbstractMultiMapIndexCreationTask<object>, IZeroIndexDefinition
{
  public ZeroMultiMapIndex() { Create(); }
  protected virtual void Create() { }

  public virtual void Setup(IZeroOptions options, IDocumentStore store) { }

  // AbstractMultiMapIndexCreationTask<TReduceResult>
  public new void AddMap<TSource>(Expression<Func<IEnumerable<TSource>, IEnumerable>> map) => base.AddMap(map);
  public new void AddMapForAll<TBase>(Expression<Func<IEnumerable<TBase>, IEnumerable>> map) => base.AddMapForAll(map);

  // AbstractGenericIndexCreationTask<TReduceResult>
  public new Expression<Func<IEnumerable<object>, IEnumerable>> Reduce { get => base.Reduce; set => base.Reduce = value; }
  public new string OutputReduceToCollection { get => base.OutputReduceToCollection; set => base.OutputReduceToCollection = value; }
  public new IDictionary<string, FieldIndexing> IndexesStrings { get => base.IndexesStrings; set => base.IndexesStrings = value; }
  public new IDictionary<Expression<Func<object, object>>, FieldIndexing> Indexes { get => base.Indexes; set => base.Indexes = value; }
  public new IDictionary<string, SpatialOptions> SpatialIndexesStrings { get => base.SpatialIndexesStrings; set => base.SpatialIndexesStrings = value; }
  public new IDictionary<Expression<Func<object, object>>, SpatialOptions> SpatialIndexes { get => base.SpatialIndexes; set => base.SpatialIndexes = value; }
  public new IDictionary<string, FieldTermVector> TermVectorsStrings { get => base.TermVectorsStrings; set => base.TermVectorsStrings = value; }
  public new IDictionary<Expression<Func<object, object>>, FieldTermVector> TermVectors { get => base.TermVectors; set => base.TermVectors = value; }
  public new IDictionary<string, string> AnalyzersStrings { get => base.AnalyzersStrings; set => base.AnalyzersStrings = value; }
  public new IDictionary<Expression<Func<object, object>>, string> Analyzers { get => base.Analyzers; set => base.Analyzers = value; }
  public new ISet<Expression<Func<object, object>>> IndexSuggestions { get => base.IndexSuggestions; set => base.IndexSuggestions = value; }
  public new IDictionary<string, FieldStorage> StoresStrings { get => base.StoresStrings; set => base.StoresStrings = value; }
  public new IDictionary<Expression<Func<object, object>>, FieldStorage> Stores { get => base.Stores; set => base.Stores = value; }
  public new string PatternReferencesCollectionName { get => base.PatternReferencesCollectionName; set => base.PatternReferencesCollectionName = value; }
  public new Expression<Func<object, string>> PatternForOutputReduceToCollectionReferences { get => base.PatternForOutputReduceToCollectionReferences; set => base.PatternForOutputReduceToCollectionReferences = value; }
  public new void AddAssembly(AdditionalAssembly assembly) => base.AddAssembly(assembly);
  public new void Analyze(string field, string analyzer) => base.Analyze(field, analyzer);
  public new void Analyze(Expression<Func<object, object>> field, string analyzer) => base.Analyze(field, analyzer);
  public new void Index(Expression<Func<object, object>> field, FieldIndexing indexing) => base.Index(field, indexing);
  public new void Index(string field, FieldIndexing indexing) => base.Index(field, indexing);
  public new void Spatial(string field, Func<SpatialOptionsFactory, SpatialOptions> indexing) => base.Spatial(field, indexing);
  public new void Spatial(Expression<Func<object, object>> field, Func<SpatialOptionsFactory, SpatialOptions> indexing) => base.Spatial(field, indexing);
  public new void Store(string field, FieldStorage storage) => base.Store(field, storage);
  public new void Store(Expression<Func<object, object>> field, FieldStorage storage) => base.Store(field, storage);
  public new void StoreAllFields(FieldStorage storage) => base.StoreAllFields(storage);
  public new void Suggestion(Expression<Func<object, object>> field) => base.Suggestion(field);
  public new void TermVector(string field, FieldTermVector termVector) => base.TermVector(field, termVector);
  public new void TermVector(Expression<Func<object, object>> field, FieldTermVector termVector) => base.TermVector(field, termVector);

  // AbstractIndexCreationTask
  public new IJsonObject AsJson(object doc) => base.AsJson(doc);
  public new IEnumerable<AttachmentName> AttachmentsFor(object doc) => base.AttachmentsFor(doc);
  public new IEnumerable<string> CounterNamesFor(object doc) => base.CounterNamesFor(doc);
  public new IJsonObject.IMetadata MetadataFor(object doc) => base.MetadataFor(doc);
  public new IEnumerable<string> TimeSeriesNamesFor(object doc) => base.TimeSeriesNamesFor(doc);

  // AbstractIndexCreationTaskBase<TIndexDefinition>
  public new object CreateField(string name, object value, CreateFieldOptions options) => base.CreateField(name, value, options);
  public new object CreateField(string name, object value, bool stored, bool analyzed) => base.CreateField(name, value, stored, analyzed);
  public new object CreateField(string name, object value) => base.CreateField(name, value);

  // AbstractCommonApiForIndexes
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, SortedSet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ISet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IList<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult[]> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, List<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ICollection<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IEnumerable<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, HashSet<TResult>> func) => base.Recurse(source, func);
  public new T? TryConvert<T>(object value) where T : struct => base.TryConvert<T>(value);
}


public abstract class ZeroMultiMapIndex<TReduceResult> : AbstractMultiMapIndexCreationTask<TReduceResult>, IZeroIndexDefinition
{
  public ZeroMultiMapIndex() { Create(); }
  protected virtual void Create() { }

  public virtual void Setup(IZeroOptions options, IDocumentStore store) { }

  // AbstractMultiMapIndexCreationTask<TReduceResult>
  public new void AddMap<TSource>(Expression<Func<IEnumerable<TSource>, IEnumerable>> map) => base.AddMap(map);
  public new void AddMapForAll<TBase>(Expression<Func<IEnumerable<TBase>, IEnumerable>> map) => base.AddMapForAll(map);

  // AbstractGenericIndexCreationTask<TReduceResult>
  public new Expression<Func<IEnumerable<TReduceResult>, IEnumerable>> Reduce { get => base.Reduce; set => base.Reduce = value; }
  public new string OutputReduceToCollection { get => base.OutputReduceToCollection; set => base.OutputReduceToCollection = value; }
  public new IDictionary<string, FieldIndexing> IndexesStrings { get => base.IndexesStrings; set => base.IndexesStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, FieldIndexing> Indexes { get => base.Indexes; set => base.Indexes = value; }
  public new IDictionary<string, SpatialOptions> SpatialIndexesStrings { get => base.SpatialIndexesStrings; set => base.SpatialIndexesStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, SpatialOptions> SpatialIndexes { get => base.SpatialIndexes; set => base.SpatialIndexes = value; }
  public new IDictionary<string, FieldTermVector> TermVectorsStrings { get => base.TermVectorsStrings; set => base.TermVectorsStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, FieldTermVector> TermVectors { get => base.TermVectors; set => base.TermVectors = value; }
  public new IDictionary<string, string> AnalyzersStrings { get => base.AnalyzersStrings; set => base.AnalyzersStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, string> Analyzers { get => base.Analyzers; set => base.Analyzers = value; }
  public new ISet<Expression<Func<TReduceResult, object>>> IndexSuggestions { get => base.IndexSuggestions; set => base.IndexSuggestions = value; }
  public new IDictionary<string, FieldStorage> StoresStrings { get => base.StoresStrings; set => base.StoresStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, FieldStorage> Stores { get => base.Stores; set => base.Stores = value; }
  public new string PatternReferencesCollectionName { get => base.PatternReferencesCollectionName; set => base.PatternReferencesCollectionName = value; }
  public new Expression<Func<TReduceResult, string>> PatternForOutputReduceToCollectionReferences { get => base.PatternForOutputReduceToCollectionReferences; set => base.PatternForOutputReduceToCollectionReferences = value; }
  public new void AddAssembly(AdditionalAssembly assembly) => base.AddAssembly(assembly);
  public new void Analyze(string field, string analyzer) => base.Analyze(field, analyzer);
  public new void Analyze(Expression<Func<TReduceResult, object>> field, string analyzer) => base.Analyze(field, analyzer);
  public new void Index(Expression<Func<TReduceResult, object>> field, FieldIndexing indexing) => base.Index(field, indexing);
  public new void Index(string field, FieldIndexing indexing) => base.Index(field, indexing);
  public new void Spatial(string field, Func<SpatialOptionsFactory, SpatialOptions> indexing) => base.Spatial(field, indexing);
  public new void Spatial(Expression<Func<TReduceResult, object>> field, Func<SpatialOptionsFactory, SpatialOptions> indexing) => base.Spatial(field, indexing);
  public new void Store(string field, FieldStorage storage) => base.Store(field, storage);
  public new void Store(Expression<Func<TReduceResult, object>> field, FieldStorage storage) => base.Store(field, storage);
  public new void StoreAllFields(FieldStorage storage) => base.StoreAllFields(storage);
  public new void Suggestion(Expression<Func<TReduceResult, object>> field) => base.Suggestion(field);
  public new void TermVector(string field, FieldTermVector termVector) => base.TermVector(field, termVector);
  public new void TermVector(Expression<Func<TReduceResult, object>> field, FieldTermVector termVector) => base.TermVector(field, termVector);

  // AbstractIndexCreationTask
  public new IJsonObject AsJson(object doc) => base.AsJson(doc);
  public new IEnumerable<AttachmentName> AttachmentsFor(object doc) => base.AttachmentsFor(doc);
  public new IEnumerable<string> CounterNamesFor(object doc) => base.CounterNamesFor(doc);
  public new IJsonObject.IMetadata MetadataFor(object doc) => base.MetadataFor(doc);
  public new IEnumerable<string> TimeSeriesNamesFor(object doc) => base.TimeSeriesNamesFor(doc);

  // AbstractIndexCreationTaskBase<TIndexDefinition>
  public new object CreateField(string name, object value, CreateFieldOptions options) => base.CreateField(name, value, options);
  public new object CreateField(string name, object value, bool stored, bool analyzed) => base.CreateField(name, value, stored, analyzed);
  public new object CreateField(string name, object value) => base.CreateField(name, value);

  // AbstractCommonApiForIndexes
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, SortedSet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ISet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IList<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult[]> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, List<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ICollection<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IEnumerable<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, HashSet<TResult>> func) => base.Recurse(source, func);
  public new T? TryConvert<T>(object value) where T : struct => base.TryConvert<T>(value);
}


public abstract class ZeroIndex<TDocument, TReduceResult> : AbstractIndexCreationTask<TDocument, TReduceResult>, IZeroIndexDefinition
{
  public ZeroIndex() { Create(); }
  protected virtual void Create() { }

  public virtual void Setup(IZeroOptions options, IDocumentStore store) { }

  // AbstractIndexCreationTask<TDocument, TReduceResult>
  public new Expression<Func<IEnumerable<TDocument>, IEnumerable>> Map { get => base.Map; set => base.Map = value; }

  // AbstractGenericIndexCreationTask<TReduceResult>
  public new Expression<Func<IEnumerable<TReduceResult>, IEnumerable>> Reduce { get => base.Reduce; set => base.Reduce = value; }
  public new string OutputReduceToCollection { get => base.OutputReduceToCollection; set => base.OutputReduceToCollection = value; }
  public new IDictionary<string, FieldIndexing> IndexesStrings { get => base.IndexesStrings; set => base.IndexesStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, FieldIndexing> Indexes { get => base.Indexes; set => base.Indexes = value; }
  public new IDictionary<string, SpatialOptions> SpatialIndexesStrings { get => base.SpatialIndexesStrings; set => base.SpatialIndexesStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, SpatialOptions> SpatialIndexes { get => base.SpatialIndexes; set => base.SpatialIndexes = value; }
  public new IDictionary<string, FieldTermVector> TermVectorsStrings { get => base.TermVectorsStrings; set => base.TermVectorsStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, FieldTermVector> TermVectors { get => base.TermVectors; set => base.TermVectors = value; }
  public new IDictionary<string, string> AnalyzersStrings { get => base.AnalyzersStrings; set => base.AnalyzersStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, string> Analyzers { get => base.Analyzers; set => base.Analyzers = value; }
  public new ISet<Expression<Func<TReduceResult, object>>> IndexSuggestions { get => base.IndexSuggestions; set => base.IndexSuggestions = value; }
  public new IDictionary<string, FieldStorage> StoresStrings { get => base.StoresStrings; set => base.StoresStrings = value; }
  public new IDictionary<Expression<Func<TReduceResult, object>>, FieldStorage> Stores { get => base.Stores; set => base.Stores = value; }
  public new string PatternReferencesCollectionName { get => base.PatternReferencesCollectionName; set => base.PatternReferencesCollectionName = value; }
  public new Expression<Func<TReduceResult, string>> PatternForOutputReduceToCollectionReferences { get => base.PatternForOutputReduceToCollectionReferences; set => base.PatternForOutputReduceToCollectionReferences = value; }
  public new void AddAssembly(AdditionalAssembly assembly) => base.AddAssembly(assembly);
  public new void Analyze(string field, string analyzer) => base.Analyze(field, analyzer);
  public new void Analyze(Expression<Func<TReduceResult, object>> field, string analyzer) => base.Analyze(field, analyzer);
  public new void Index(Expression<Func<TReduceResult, object>> field, FieldIndexing indexing) => base.Index(field, indexing);
  public new void Index(string field, FieldIndexing indexing) => base.Index(field, indexing);
  public new void Spatial(string field, Func<SpatialOptionsFactory, SpatialOptions> indexing) => base.Spatial(field, indexing);
  public new void Spatial(Expression<Func<TReduceResult, object>> field, Func<SpatialOptionsFactory, SpatialOptions> indexing) => base.Spatial(field, indexing);
  public new void Store(string field, FieldStorage storage) => base.Store(field, storage);
  public new void Store(Expression<Func<TReduceResult, object>> field, FieldStorage storage) => base.Store(field, storage);
  public new void StoreAllFields(FieldStorage storage) => base.StoreAllFields(storage);
  public new void Suggestion(Expression<Func<TReduceResult, object>> field) => base.Suggestion(field);
  public new void TermVector(string field, FieldTermVector termVector) => base.TermVector(field, termVector);
  public new void TermVector(Expression<Func<TReduceResult, object>> field, FieldTermVector termVector) => base.TermVector(field, termVector);

  // AbstractIndexCreationTask
  public new IJsonObject AsJson(object doc) => base.AsJson(doc);
  public new IEnumerable<AttachmentName> AttachmentsFor(object doc) => base.AttachmentsFor(doc);
  public new IEnumerable<string> CounterNamesFor(object doc) => base.CounterNamesFor(doc);
  public new IJsonObject.IMetadata MetadataFor(object doc) => base.MetadataFor(doc);
  public new IEnumerable<string> TimeSeriesNamesFor(object doc) => base.TimeSeriesNamesFor(doc);

  // AbstractIndexCreationTaskBase<TIndexDefinition>
  public new object CreateField(string name, object value, CreateFieldOptions options) => base.CreateField(name, value, options);
  public new object CreateField(string name, object value, bool stored, bool analyzed) => base.CreateField(name, value, stored, analyzed);
  public new object CreateField(string name, object value) => base.CreateField(name, value);

  // AbstractCommonApiForIndexes
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, SortedSet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ISet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IList<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult[]> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, List<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ICollection<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IEnumerable<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, HashSet<TResult>> func) => base.Recurse(source, func);
  public new T? TryConvert<T>(object value) where T : struct => base.TryConvert<T>(value);
}


public abstract class ZeroIndex<TDocument> : ZeroIndex<TDocument, TDocument>
{
}


public abstract class ZeroIndex : AbstractIndexCreationTask, IZeroIndexDefinition
{
  public ZeroIndex() { Create(); }
  protected virtual void Create() { }

  public virtual void Setup(IZeroOptions options, IDocumentStore store) { }

  // AbstractIndexCreationTask
  public new IJsonObject AsJson(object doc) => base.AsJson(doc);
  public new IEnumerable<AttachmentName> AttachmentsFor(object doc) => base.AttachmentsFor(doc);
  public new IEnumerable<string> CounterNamesFor(object doc) => base.CounterNamesFor(doc);
  public new IJsonObject.IMetadata MetadataFor(object doc) => base.MetadataFor(doc);
  public new IEnumerable<string> TimeSeriesNamesFor(object doc) => base.TimeSeriesNamesFor(doc);

  // AbstractIndexCreationTaskBase<TIndexDefinition>
  public new object CreateField(string name, object value, CreateFieldOptions options) => base.CreateField(name, value, options);
  public new object CreateField(string name, object value, bool stored, bool analyzed) => base.CreateField(name, value, stored, analyzed);
  public new object CreateField(string name, object value) => base.CreateField(name, value);

  // AbstractCommonApiForIndexes
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, SortedSet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ISet<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IList<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult[]> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, List<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, ICollection<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, IEnumerable<TResult>> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, TResult> func) => base.Recurse(source, func);
  public new IEnumerable<TResult> Recurse<TSource, TResult>(TSource source, Func<TSource, HashSet<TResult>> func) => base.Recurse(source, func);
  public new T? TryConvert<T>(object value) where T : struct => base.TryConvert<T>(value);
}


public interface IZeroIndexDefinition : IAbstractIndexCreationTask
{
  void Setup(IZeroOptions options, IDocumentStore store);
}

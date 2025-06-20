namespace Application.Interfaces;

using Nest;

public interface IElasticIndexDefinition<T> where T : class
{
    TypeMappingDescriptor<T> BuildMapping(TypeMappingDescriptor<T> map);
}

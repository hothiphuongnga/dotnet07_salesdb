using AutoMapper;
using SalesDB.Dtos.Base;
using SalesDB.Repositories;

namespace SalesDB.Services;

public interface IServiceBase<TEntity, TDto> where TEntity : class
{
    Task<ResponseEntity> GetAllAsync();
    Task<ResponseEntity> GetByIdAsync(int id);
    Task<ResponseEntity> GetByIdsAsync(IEnumerable<int> ids);
    Task<ResponseEntity> SearchAsync(string fieldName, string keyword);
    Task<ResponseEntity> FindAsync(string fieldName, string keyword);

}

public class ServiceBase<TEntity, TDto>
(IRepositoryBase<TEntity, int> _repository, IMapper _mapper)
 : IServiceBase<TEntity, TDto> where TEntity : class
{


    public async Task<ResponseEntity> GetAllAsync()
    {
        var res = await _repository.GetAllAsync();
        var resDTO = _mapper.Map<List<TDto>>(res);
        return new ResponseEntity(200, resDTO, "Success");
    }

    public async Task<ResponseEntity> GetByIdAsync(int id)
    {
        var entitie = await _repository.GetByIdAsync(id);
        return new ResponseEntity(200, (object?)_mapper.Map<TDto>(entitie) ?? new { }, "Success");

    }

    public async Task<ResponseEntity> GetByIdsAsync(IEnumerable<int> ids)
    {
        var entities = await _repository.GetByIdsAsync(ids);
        return new ResponseEntity(200, _mapper.Map<List<TDto>>(entities));
    }

    public async Task<ResponseEntity> SearchAsync(string fieldName, string keyword)
    {
        var res = await _repository.SearchAsync(fieldName, keyword);
        var resDto = _mapper.Map<List<TDto>>(res);
        return new ResponseEntity(200, resDto, "Success");
    }
    public async Task<ResponseEntity> FindAsync(string fieldName, string keyword)
    {
        var res = await _repository.FindByFieldAsync(fieldName, keyword);
        var resDto = _mapper.Map<List<TDto>>(res);
        return new ResponseEntity(200, resDto, "Success");
    }
}
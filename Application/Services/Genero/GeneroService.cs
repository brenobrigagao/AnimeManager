using Application.DTO.Genero;
using Infra.Entities;
using Infra.Repositories;

namespace Application.Services.Genero
{
    public class GeneroService : IGeneroService
    {
        private readonly UnityOfWork _unityOfWork;

        public GeneroService(UnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        public async Task<Response<IEnumerable<GeneroDTO>>> GetAllAsync()
        {
            var resposta = new Response<IEnumerable<GeneroDTO>>();
            try
            {
                var generos = await _unityOfWork.Generos.GetAll();
                resposta.Status = true;
                resposta.Mensagem = "Lista de gêneros carregada com sucesso";
                resposta.Dados = generos.Select(g => new GeneroDTO
                {
                    Id = g.Id,
                    Nome = g.Nome
                });
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao listar gêneros: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<GeneroDTO>> GetByIdAsync(int id)
        {
            var resposta = new Response<GeneroDTO>();
            try
            {
                var genero = await _unityOfWork.Generos.GetById(id);
                if (genero == null)
                    throw new Exception("Gênero não encontrado");

                resposta.Status = true;
                resposta.Mensagem = "Gênero encontrado";
                resposta.Dados = new GeneroDTO
                {
                    Id = genero.Id,
                    Nome = genero.Nome
                };
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao buscar gênero: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<GeneroDTO>> CreateAsync(GeneroCreateDTO dto)
        {
            var resposta = new Response<GeneroDTO>();
            try
            {
                var genero = new Infra.Entities.Genero
                {
                    Nome = dto.Nome
                };
                await _unityOfWork.Generos.Add(genero);
                await _unityOfWork.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Gênero criado com sucesso";
                resposta.Dados = new GeneroDTO
                {
                    Id = genero.Id,
                    Nome = genero.Nome
                };
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao criar gênero: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<string>> UpdateAsync(int id, GeneroUpdateDTO dto)
        {
            var resposta = new Response<string>();
            try
            {
                var genero = await _unityOfWork.Generos.GetById(id);
                if (genero == null)
                    throw new Exception("Gênero não encontrado");

                genero.Nome = dto.Nome;
                _unityOfWork.Generos.Update(genero);
                await _unityOfWork.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Gênero atualizado com sucesso";
                resposta.Dados = "Atualização concluída";
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao atualizar gênero: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<string>> DeleteAsync(int id)
        {
            var resposta = new Response<string>();
            try
            {
                var genero = await _unityOfWork.Generos.GetById(id);
                if (genero == null)
                    throw new Exception("Gênero não encontrado");

                _unityOfWork.Generos.Delete(genero);
                await _unityOfWork.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Gênero deletado com sucesso";
                resposta.Dados = "Deleção concluída";
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao deletar gênero: {ex.Message}";
            }
            return resposta;
        }
    }
}

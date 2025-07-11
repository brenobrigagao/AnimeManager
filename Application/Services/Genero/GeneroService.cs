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

            var generos = await _unityOfWork.Generos.GetAll();

            resposta.Status = true;
            resposta.Mensagem = "Lista de gêneros carregada com sucesso";
            resposta.Dados = generos.Select(g => new GeneroDTO
            {
                Id = g.Id,
                Nome = g.Nome
            });

            return resposta;
        }

        public async Task<Response<GeneroDTO>> GetByIdAsync(int id)
        {
            var resposta = new Response<GeneroDTO>();

            var genero = await _unityOfWork.Generos.GetById(id);
            if (genero == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Gênero não encontrado";
                return resposta;
            }

            resposta.Status = true;
            resposta.Mensagem = "Gênero encontrado";
            resposta.Dados = new GeneroDTO
            {
                Id = genero.Id,
                Nome = genero.Nome
            };

            return resposta;
        }

        public async Task<Response<GeneroDTO>> CreateAsync(GeneroCreateDTO dto)
        {
            var resposta = new Response<GeneroDTO>();

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

            return resposta;
        }

        public async Task<Response<string>> UpdateAsync(int id, GeneroUpdateDTO dto)
        {
            var resposta = new Response<string>();

            var genero = await _unityOfWork.Generos.GetById(id);
            if (genero == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Gênero não encontrado";
                return resposta;
            }

            genero.Nome = dto.Nome;
            _unityOfWork.Generos.Update(genero);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Gênero atualizado com sucesso";
            resposta.Dados = "Atualização concluída";

            return resposta;
        }

        public async Task<Response<string>> DeleteAsync(int id)
        {
            var resposta = new Response<string>();

            var genero = await _unityOfWork.Generos.GetById(id);
            if (genero == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Gênero não encontrado";
                return resposta;
            }

            _unityOfWork.Generos.Delete(genero);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Gênero deletado com sucesso";
            resposta.Dados = "Deleção concluída";

            return resposta;
        }
    }
}

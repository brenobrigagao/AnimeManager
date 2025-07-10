using Application.DTO.Estudio;
using Infra.Entities;
using Infra.Repositories;

namespace Application.Services.Estudio
{
    public class EstudioService : IEstudioService
    {
        private readonly UnityOfWork _unityOfWork;

        public EstudioService(UnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        public async Task<Response<IEnumerable<EstudioDTO>>> GetAllAsync()
        {
            var resposta = new Response<IEnumerable<EstudioDTO>>();
            try
            {
                var estudios = await _unityOfWork.Estudios.GetAll();
                resposta.Status = true;
                resposta.Mensagem = "Lista de estúdios carregada com sucesso";
                resposta.Dados = estudios.Select(e => new EstudioDTO
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Descricao = e.Descricao
                });
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao listar estúdios: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<EstudioDTO>> GetByIdAsync(int id)
        {
            var resposta = new Response<EstudioDTO>();
            try
            {
                var estudio = await _unityOfWork.Estudios.GetById(id);
                if (estudio == null)
                    throw new Exception("Estúdio não encontrado");

                resposta.Status = true;
                resposta.Mensagem = "Estúdio encontrado";
                resposta.Dados = new EstudioDTO
                {
                    Id = estudio.Id,
                    Nome = estudio.Nome,
                    Descricao = estudio.Descricao
                };
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao buscar estúdio: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<EstudioDTO>> CreateAsync(EstudioCreateDTO dto)
        {
            var resposta = new Response<EstudioDTO>();
            try
            {
                var estudio = new Infra.Entities.Estudio
                {
                    Nome = dto.Nome,
                    Descricao = dto.Descricao
                };
                await _unityOfWork.Estudios.Add(estudio);
                await _unityOfWork.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Estúdio criado com sucesso";
                resposta.Dados = new EstudioDTO
                {
                    Id = estudio.Id,
                    Nome = estudio.Nome,
                    Descricao = estudio.Descricao
                };
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao criar estúdio: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<string>> UpdateAsync(int id, EstudioUpdateDTO dto)
        {
            var resposta = new Response<string>();
            try
            {
                var estudio = await _unityOfWork.Estudios.GetById(id);
                if (estudio == null)
                    throw new Exception("Estúdio não encontrado");

                estudio.Nome = dto.Nome;
                estudio.Descricao = dto.Descricao;

                _unityOfWork.Estudios.Update(estudio);
                await _unityOfWork.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Estúdio atualizado com sucesso";
                resposta.Dados = "Atualização concluída";
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao atualizar estúdio: {ex.Message}";
            }
            return resposta;
        }

        public async Task<Response<string>> DeleteAsync(int id)
        {
            var resposta = new Response<string>();
            try
            {
                var estudio = await _unityOfWork.Estudios.GetById(id);
                if (estudio == null)
                    throw new Exception("Estúdio não encontrado");

                _unityOfWork.Estudios.Delete(estudio);
                await _unityOfWork.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Estúdio deletado com sucesso";
                resposta.Dados = "Deleção concluída";
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao deletar estúdio: {ex.Message}";
            }
            return resposta;
        }
    }
}

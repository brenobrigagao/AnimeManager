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
            
            var estudios = await _unityOfWork.Estudios.GetAll();

            resposta.Status = true;
            resposta.Mensagem = "Lista de estúdios carregada com sucesso";
            resposta.Dados = estudios.Select(e => new EstudioDTO
            {
                Id = e.Id,
                Nome = e.Nome,
                Descricao = e.Descricao
            });

            return resposta;
        }

        public async Task<Response<EstudioDTO>> GetByIdAsync(int id)
        {
            var resposta = new Response<EstudioDTO>();

            var estudio = await _unityOfWork.Estudios.GetById(id);
            if (estudio == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Estúdio não encontrado";
                return resposta;
            }

            resposta.Status = true;
            resposta.Mensagem = "Estúdio encontrado";
            resposta.Dados = new EstudioDTO
            {
                Id = estudio.Id,
                Nome = estudio.Nome,
                Descricao = estudio.Descricao
            };

            return resposta;
        }

        public async Task<Response<EstudioDTO>> CreateAsync(EstudioCreateDTO dto)
        {
            var resposta = new Response<EstudioDTO>();

            var existe = await _unityOfWork.Estudios.AnyAsync(e => e.Nome == dto.Nome);
            if (existe)
            {
                resposta.Status = false;
                resposta.Mensagem = "Já existe um estúdio com esse nome";
                return resposta;
            }

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

            return resposta;
        }

        public async Task<Response<string>> UpdateAsync(int id, EstudioUpdateDTO dto)
        {
            var resposta = new Response<string>();

            var existe = await _unityOfWork.Estudios.AnyAsync(e => e.Nome == dto.Nome && e.Id != id);
            if (existe)
            {
                resposta.Status = false;
                resposta.Mensagem = "Já existe um estúdio com esse nome";
                return resposta;
            }

            var estudio = await _unityOfWork.Estudios.GetById(id);
            if (estudio == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Estúdio não encontrado";
                return resposta;
            }

            estudio.Nome = dto.Nome;
            estudio.Descricao = dto.Descricao;

            _unityOfWork.Estudios.Update(estudio);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Estúdio atualizado com sucesso";
            resposta.Dados = "Atualização concluída";

            return resposta;
        }

        public async Task<Response<string>> DeleteAsync(int id)
        {
            var resposta = new Response<string>();

            var estudio = await _unityOfWork.Estudios.GetById(id);
            if (estudio == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Estúdio não encontrado";
                return resposta;
            }

            _unityOfWork.Estudios.Delete(estudio);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Estúdio deletado com sucesso";
            resposta.Dados = "Deleção concluída";

            return resposta;
        }
    }
}

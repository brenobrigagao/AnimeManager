using Application.DTO.Usuario;
using Infra.Repositories.Interfaces;
using Application.Services.Senha;
using Infra.Entities;

namespace Application.Services.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly ISenhaService _senhaService;

        public UsuarioService(IUnityOfWork unityOfWork, ISenhaService senhaService)
        {
            _unityOfWork = unityOfWork;
            _senhaService = senhaService;
        }

        public async Task<Response<UsuarioDTO>> GetByIdAsync(int id)
        {
            var resposta = new Response<UsuarioDTO>();

            var usuario = await _unityOfWork.Usuarios.GetById(id);
            if (usuario == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Usuário não encontrado";
                return resposta;
            }

            resposta.Status = true;
            resposta.Mensagem = "Usuário encontrado";
            resposta.Dados = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            return resposta;
        }

        public async Task<Response<IEnumerable<UsuarioDTO>>> GetAllAsync()
        {
            var resposta = new Response<IEnumerable<UsuarioDTO>>();

            var usuarios = await _unityOfWork.Usuarios.GetAll();

            resposta.Status = true;
            resposta.Mensagem = "Lista de usuários carregada com sucesso";
            resposta.Dados = usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
            });

            return resposta;
        }

        public async Task<Response<UsuarioDTO>> CreateAsync(UsuarioCreateDTO dto)
        {
            var resposta = new Response<UsuarioDTO>();

            var existente = await _unityOfWork.Usuarios.GetByEmail(dto.Email);
            if (existente != null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Esse email já está cadastrado";
                return resposta;
            }

            _senhaService.CriarHashSenha(dto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            var usuario = new Infra.Entities.Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = senhaHash,
                SenhaSalt = senhaSalt,
                TokenDataCriacao = DateTime.UtcNow,
                IsAdmin = false
            };

            await _unityOfWork.Usuarios.Add(usuario);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Usuário criado com sucesso";
            resposta.Dados = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            return resposta;
        }

        public async Task<Response<string>> UpdateAsync(int id, UsuarioUpdateDTO dto)
        {
            var resposta = new Response<string>();

            var usuario = await _unityOfWork.Usuarios.GetById(id);
            if (usuario == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Usuário não encontrado";
                return resposta;
            }

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.NovaSenha))
            {
                _senhaService.CriarHashSenha(dto.NovaSenha, out byte[] novaHash, out byte[] novoSalt);
                usuario.SenhaHash = novaHash;
                usuario.SenhaSalt = novoSalt;
            }

            _unityOfWork.Usuarios.Update(usuario);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Usuário atualizado com sucesso";
            resposta.Dados = "Atualização concluída";

            return resposta;
        }

        public async Task<Response<string>> DeleteAsync(int id)
        {
            var resposta = new Response<string>();

            var usuario = await _unityOfWork.Usuarios.GetById(id);
            if (usuario == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "O usuário não foi encontrado";
                return resposta;
            }

            _unityOfWork.Usuarios.Delete(usuario);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Usuário deletado com sucesso";
            resposta.Dados = "Deleção concluída";

            return resposta;
        }

        public async Task<Response<UsuarioDTO>> CreateAdminAsync(UsuarioCreateDTO dto)
        {
            var resposta = new Response<UsuarioDTO>();

            var existente = await _unityOfWork.Usuarios.GetByEmail(dto.Email);
            if (existente != null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Esse email já está cadastrado";
                return resposta;
            }

            _senhaService.CriarHashSenha(dto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            var usuario = new Infra.Entities.Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = senhaHash,
                SenhaSalt = senhaSalt,
                TokenDataCriacao = DateTime.UtcNow,
                IsAdmin = true
            };

            await _unityOfWork.Usuarios.Add(usuario);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Admin criado com sucesso";
            resposta.Dados = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            return resposta;
        }

        public async Task<Response<string>> UpdateAdminAsync(int id)
        {
            var resposta = new Response<string>();

            var usuario = await _unityOfWork.Usuarios.GetById(id);
            if (usuario == null)
            {
                resposta.Status = false;
                resposta.Mensagem = "Usuário não encontrado";
                return resposta;
            }

            usuario.IsAdmin = true;
            _unityOfWork.Usuarios.Update(usuario);
            await _unityOfWork.SaveChangesAsync();

            resposta.Status = true;
            resposta.Mensagem = "Usuário promovido a admin";
            resposta.Dados = "Atualização concluída";

            return resposta;
        }
    }
}

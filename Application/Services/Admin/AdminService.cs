using Application.DTO.Usuario;
using Application.Services.Usuario;
using Infra.Data.Context;
using Infra.Entities;

namespace Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioService _usuarioService;

        public AdminService(AppDbContext context, IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
        }

        public async Task<Response<UsuarioDTO>> RegistrarAdmin(UsuarioCreateDTO dto)
        {
            return await _usuarioService.CreateAdminAsync(dto);
        }

        public async Task<Response<string>> PromoverAdmin(int id)
        {
            return await _usuarioService.UpdateAdminAsync(id);
        }

        public async Task<Response<IEnumerable<UsuarioDTO>>> ListarUsuarios()
        {
            return await _usuarioService.GetAllAsync();
        }

        public async Task<Response<string>> DeletarUsuario(int id)
        {
            return await _usuarioService.DeleteAsync(id);
        }
    }
}
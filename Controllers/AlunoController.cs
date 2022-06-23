using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Escola.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Escola.Controllers 
{
    public class AlunoController : Controller
    {
        private readonly ILogger<AlunoController> _logger;  
        private readonly DadosContext _context;
        const int itensPorPagina = 5;
  
        public AlunoController(ILogger<AlunoController> logger, DadosContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? pagina)
        {
            var idEscola = 1;      
            int numeroPagina = (pagina ?? 1);

            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@idEscola", idEscola)
            };
            List<Models.Aluno> alunos = _context.RetornarLista<Models.Aluno>("sp_consultarAluno", parametros);

            ViewBagEscolas();
            return View(alunos.ToPagedList(numeroPagina, itensPorPagina));
        }

        public IActionResult Detalhe(int id, int idEscola)
        {
            Models.Aluno aluno = new Models.Aluno();
            if (id > 0)  {
                SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@identificacao", id)
            };
                aluno = _context.ListarObjeto<Models.Aluno>("sp_buscarPessoaPorId", parametros); 
            } else {
                aluno.IdEscola = idEscola;
            }

            ViewBagTurmas(id > 0 ? aluno.IdEscola : idEscola);
            return View(aluno);
        }

        [HttpPost]
        public IActionResult Detalhe(Models.Aluno aluno){

            string mensagem = "";

            if(string.IsNullOrEmpty(aluno.Nome)){
                ModelState.AddModelError("", "O nome não pode ser vazio");
            }    
            if(string.IsNullOrEmpty(aluno.Endereco)){
                ModelState.AddModelError("", "O endereco deve ser informado");
            }
            if(string.IsNullOrEmpty(aluno.Bairro)){
                ModelState.AddModelError("", "O bairro deve ser informado");
            }
            if(string.IsNullOrEmpty(aluno.Cidade)){
                ModelState.AddModelError("", "A cidade deve ser informado");
            }
            if(string.IsNullOrEmpty(aluno.Estado)){
                ModelState.AddModelError("", "O estado deve ser informado");
            }
            if(string.IsNullOrEmpty(aluno.Pais)){
                ModelState.AddModelError("", "O país deve ser informado"); 
            }
            if(string.IsNullOrEmpty(aluno.CEP)){
                ModelState.AddModelError("", "O CEP deve ser informado");
            }
            if(string.IsNullOrEmpty(aluno.Telefone)){
                ModelState.AddModelError("", "O telefone deve ser informado");
            }

            if(ModelState.IsValid){
           
                List<SqlParameter> parametros = new List<SqlParameter>(){
                    new SqlParameter("@Nome", aluno.Nome),
                    new SqlParameter("@CPF", aluno.CPF),
                    new SqlParameter("@RG", aluno.RG),
                    new SqlParameter("@Telefone", aluno.Telefone),
                    new SqlParameter("@Endereco", aluno.Endereco),
                    new SqlParameter("@Numero", aluno.Numero),
                    new SqlParameter("@Complemento", aluno.Complemento),
                    new SqlParameter("@Bairro", aluno.Bairro),
                    new SqlParameter("@Cidade", aluno.Cidade),
                    new SqlParameter("@Estado", aluno.Estado),
                    new SqlParameter("@Pais", aluno.Pais),
                    new SqlParameter("@CEP", aluno.CEP),
                    new SqlParameter("@DataNascimento", aluno.DataNascimento),
                    new SqlParameter("@NomeMae", aluno.NomeMae),
                    new SqlParameter("@NomePai", aluno.NomePai),
                    new SqlParameter("@IdTurma", aluno.IdTurma),
                    new SqlParameter("@IdEscola", aluno.IdEscola)

                };
                if (aluno.Id > 0){
                    parametros.Add(new SqlParameter("@Id", aluno.Id));
                    parametros.Add(new SqlParameter("@Acao", 2));
                    parametros.Add(new SqlParameter("@Opcao", "aluno"));
                } else {
                    parametros.Add(new SqlParameter("@Acao", 1));
                    parametros.Add(new SqlParameter("@Opcao", "aluno"));
                }

                var retorno = _context.ListarObjeto<RetornoProcedure>("sp_salvarPessoa", parametros.ToArray());
            
                if (retorno.Mensagem == "Ok"){
                    return new JsonResult(new {Sucesso = retorno.Mensagem == "Ok"});
                } else {
                    mensagem = retorno.Mensagem;                
                }
            }

            return new JsonResult(new {Sucesso = false, Mensagem = mensagem});
        }

        public JsonResult Excluir(int id){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@Id", id),
                new SqlParameter("@Acao", 0),
                new SqlParameter("@Opcao", "aluno")
            };
            var retorno = _context.ListarObjeto<RetornoProcedure>("sp_salvarPessoa", parametros);
            return new JsonResult(new {Sucesso = retorno.Mensagem == "Excluído", Mensagem = retorno.Mensagem });
        }

        public PartialViewResult ListaPartialView(int idEscola){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@idEscola", idEscola)
            };
            List<Models.Aluno> alunos = _context.RetornarLista<Models.Aluno>("sp_consultarAluno", parametros);
            
            HttpContext.Session.SetInt32("IdEscola", idEscola);

            return PartialView(alunos.ToPagedList(1, itensPorPagina));
        }

        private void ViewBagEscolas(){
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@nome", "")
            };
            List<Models.Escola> escolas = new List<Models.Escola>(); 
            escolas = _context.RetornarLista<Models.Escola>("sp_consultarEscola", param);
            
            ViewBag.Escolas = escolas.Select(c => new SelectListItem(){
                Text= c.Nome, Value= c.Id.ToString()
            }).ToList();
        }

        private void ViewBagTurmas(int idescola){
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@idEscola", idescola)
            };
            List<Models.Turma> turmas = new List<Models.Turma>(); 
            turmas = _context.RetornarLista<Models.Turma>("sp_consultarTurma", param);
            
            ViewBag.Turmas = turmas.Select(c => new SelectListItem(){
                Text= c.Id +" - "+ c.Tipo +" - "+ c.Serie +" - "+ c.Descricao, Value= c.Id.ToString()
            }).ToList();
        }
      

    }
}
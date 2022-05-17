using System.Collections.Generic;
using System.Data.SqlClient;
using Escola.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Escola.Controllers 
{
    public class ProfessorController : Controller
    {
        private readonly ILogger<ProfessorController> _logger;  
        private readonly DadosContext _context;
        const int itensPorPagina = 5;
  
        public ProfessorController(ILogger<ProfessorController> logger, DadosContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? pagina)
        {
            var nome = "";          
            int numeroPagina = (pagina ?? 1);

            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@nome", nome)
            };
            List<Models.Professor> professores = _context.RetornarLista<Models.Professor>("sp_consultarProfessor", parametros);
            
            return View(professores.ToPagedList(numeroPagina, itensPorPagina));
        }

        public IActionResult Detalhe(int id)
        {
            Models.Professor professor = new Models.Professor();
            if (id > 0)  {
                SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@identificacao", id)
            };
                professor = _context.ListarObjeto<Models.Professor>("sp_buscarPessoaPorId", parametros); 
            }

            return View(professor);
        }

        [HttpPost]
        public IActionResult Detalhe(Models.Professor professor){
            if(string.IsNullOrEmpty(professor.Nome)){
                ModelState.AddModelError("", "O nome não pode ser vazio");
            }    
            if(string.IsNullOrEmpty(professor.Endereco)){
                ModelState.AddModelError("", "O endereco deve ser informado");
            }
            if(string.IsNullOrEmpty(professor.Bairro)){
                ModelState.AddModelError("", "O bairro deve ser informado");
            }
            if(string.IsNullOrEmpty(professor.Cidade)){
                ModelState.AddModelError("", "A cidade deve ser informado");
            }
            if(string.IsNullOrEmpty(professor.Estado)){
                ModelState.AddModelError("", "O estado deve ser informado");
            }
            if(string.IsNullOrEmpty(professor.Pais)){
                ModelState.AddModelError("", "O país deve ser informado"); 
            }
            if(string.IsNullOrEmpty(professor.CEP)){
                ModelState.AddModelError("", "O CEP deve ser informado");
            }
            if(string.IsNullOrEmpty(professor.Telefone)){
                ModelState.AddModelError("", "O telefone deve ser informado");
            }

            if(ModelState.IsValid){
           
                List<SqlParameter> parametros = new List<SqlParameter>(){
                    new SqlParameter("@Nome", professor.Nome),
                    new SqlParameter("@CPF", professor.CPF),
                    new SqlParameter("@RG", professor.RG),
                    new SqlParameter("@Telefone", professor.Telefone),
                    new SqlParameter("@Endereco", professor.Endereco),
                    new SqlParameter("@Numero", professor.Numero),
                    new SqlParameter("@Complemento", professor.Complemento),
                    new SqlParameter("@Bairro", professor.Bairro),
                    new SqlParameter("@Cidade", professor.Cidade),
                    new SqlParameter("@Estado", professor.Estado),
                    new SqlParameter("@Pais", professor.Pais),
                    new SqlParameter("@CEP", professor.CEP),
                    new SqlParameter("@DataNascimento", professor.DataNascimento),
                    new SqlParameter("@NomeMae", professor.NomeMae),
                    new SqlParameter("@NomePai", professor.NomePai),
                    new SqlParameter("@IdTurma", professor.IdTurma)

                };
                if (professor.Id > 0){
                    parametros.Add(new SqlParameter("@Id", professor.Id));
                    parametros.Add(new SqlParameter("@Acao", 2));
                    parametros.Add(new SqlParameter("@Opcao", "professor"));
                } else {
                    parametros.Add(new SqlParameter("@Acao", 1));
                    parametros.Add(new SqlParameter("@Opcao", "professor"));
                }

                var retorno = _context.ListarObjeto<RetornoProcedure>("sp_salvarPessoa", parametros.ToArray());
            
                if (retorno.Mensagem == "Ok"){
                    return RedirectToAction("Index");
                } else {
                    ModelState.AddModelError("", retorno.Mensagem);                    
                }
            }
            return View(professor);
        }

        public JsonResult Excluir(int id){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@Id", id),
                new SqlParameter("@Acao", 0),
                new SqlParameter("@Opcao", "professor")
            };
            var retorno = _context.ListarObjeto<RetornoProcedure>("sp_salvarPessoa", parametros);
            return new JsonResult(new {Sucesso = retorno.Mensagem == "Excluído", Mensagem = retorno.Mensagem });
        }

        public PartialViewResult ListaPartialView(string nome){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@nome", nome)
            };
            List<Models.Professor> professores = _context.RetornarLista<Models.Professor>("sp_consultarProfessor", parametros);
            if (string.IsNullOrEmpty(nome)){
                HttpContext.Session.Remove("TextoPesquisa");
            } else {
            HttpContext.Session.SetString("TextoPesquisa", nome);
            }

            return PartialView(professores.ToPagedList(1, itensPorPagina));
        }
      

    }
}
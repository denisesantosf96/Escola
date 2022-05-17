using System.Collections.Generic;
using Escola.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;
using System.Data.SqlClient;

namespace Escola.Controllers 
{
    public class EscolaController : Controller
    {
        private readonly ILogger<EscolaController> _logger;  
        private readonly DadosContext _context;
        const int itensPorPagina = 5;
  
        public EscolaController(ILogger<EscolaController> logger, DadosContext context)
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
            List<Models.Escola> escolas = _context.RetornarLista<Models.Escola>("sp_consultarEscola", parametros);
            
            return View(escolas.ToPagedList(numeroPagina, itensPorPagina));
        }

        public IActionResult Detalhe(int id)
        {
            Models.Escola escola = new Models.Escola();
            if (id > 0)  {
                SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@identificacao", id)
            };
                escola = _context.ListarObjeto<Models.Escola>("sp_buscarEscolaPorId", parametros); 
            }

            return View(escola);
        }

        [HttpPost]
        public IActionResult Detalhe(Models.Escola escola){
            if(string.IsNullOrEmpty(escola.Nome)){
                ModelState.AddModelError("", "O nome não pode ser vazio");
            }    
            if(string.IsNullOrEmpty(escola.Endereco)){
                ModelState.AddModelError("", "O endereco deve ser informado");
            }
            if(string.IsNullOrEmpty(escola.Bairro)){
                ModelState.AddModelError("", "O bairro deve ser informado");
            }
            if(string.IsNullOrEmpty(escola.Cidade)){
                ModelState.AddModelError("", "A cidade deve ser informado");
            }
            if(string.IsNullOrEmpty(escola.Estado)){
                ModelState.AddModelError("", "O estado deve ser informado");
            }
            if(string.IsNullOrEmpty(escola.Pais)){
                ModelState.AddModelError("", "O país deve ser informado"); 
            }
            if(string.IsNullOrEmpty(escola.CEP)){
                ModelState.AddModelError("", "O CEP deve ser informado");
            }
            if(string.IsNullOrEmpty(escola.Telefone)){
                ModelState.AddModelError("", "O telefone deve ser informado");
            }

            if(ModelState.IsValid){
           
                List<SqlParameter> parametros = new List<SqlParameter>(){
                    new SqlParameter("Nome", escola.Nome),
                    new SqlParameter("Endereco", escola.Endereco),
                    new SqlParameter("Numero", escola.Numero),
                    new SqlParameter("Complemento", escola.Complemento),
                    new SqlParameter("Bairro", escola.Bairro),
                    new SqlParameter("Cidade", escola.Cidade),
                    new SqlParameter("Estado", escola.Estado),
                    new SqlParameter("Pais", escola.Pais),
                    new SqlParameter("CEP", escola.CEP),
                    new SqlParameter("Telefone", escola.Telefone)

                };
                if (escola.Id > 0){
                    parametros.Add(new SqlParameter("@Identificacao", escola.Id));
                }
                var retorno = _context.ListarObjeto<RetornoProcedure>(escola.Id > 0? "sp_atualizarEscola" : "sp_inserirEscola", parametros.ToArray());
            
                if (retorno.Mensagem == "Ok"){
                    return RedirectToAction("Index");
                } else {
                    ModelState.AddModelError("", retorno.Mensagem);
                    
                }
            }
            return View(escola);
        }

        public JsonResult Excluir(int id){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@Identificacao", id)
            };
            var retorno = _context.ListarObjeto<RetornoProcedure>("sp_excluirEscola", parametros);
            return new JsonResult(new {Sucesso = retorno.Mensagem == "Excluído", Mensagem = retorno.Mensagem });
        }

        public PartialViewResult ListaPartialView(string nome){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@nome", nome)
            };
            List<Models.Escola> escolas = _context.RetornarLista<Models.Escola>("sp_consultarEscola", parametros);
            if (string.IsNullOrEmpty(nome)){
                HttpContext.Session.Remove("TextoPesquisa");
            } else {
            HttpContext.Session.SetString("TextoPesquisa", nome);
            }

            return PartialView(escolas.ToPagedList(1, itensPorPagina));
        }
      

    }
}
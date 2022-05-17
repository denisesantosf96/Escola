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
    public class TurmaController : Controller
    {
        private readonly ILogger<TurmaController> _logger;  
        private readonly DadosContext _context ;
        const int itensPorPagina = 5; 

        public TurmaController(ILogger<TurmaController> logger, DadosContext context)
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
            List<Turma> turmas = _context.RetornarLista<Turma>("sp_consultarTurma", parametros);
            
            ViewBagEscolas();
            return View(turmas.ToPagedList(numeroPagina, itensPorPagina));
        }

        public IActionResult Detalhe(int id)
        {
            Models.Turma turma = new Models.Turma();
            if (id > 0)  {
                SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@identificacao", id)
            };
                turma = _context.ListarObjeto<Turma>("sp_buscarTurmaPorId", parametros); 
            }

            ViewBagEscolas();
            return View(turma);
        }



        [HttpPost]
        public IActionResult Detalhe(Models.Turma turma){
            
            if(string.IsNullOrEmpty(turma.Tipo)){
                ModelState.AddModelError("", "O tipo deve ser preenchido");
            }

            if(ModelState.IsValid){
           
                List<SqlParameter> parametros = new List<SqlParameter>(){
                    
                    new SqlParameter("@IdEscola", turma.IdEscola),
                    new SqlParameter("@Tipo", turma.Tipo),
                    new SqlParameter("@Descricao", turma.Descricao),
                    new SqlParameter("@Serie", turma.Serie),
                    new SqlParameter("@Ano", turma.Ano)                 
                };
                if (turma.Id > 0){
                    parametros.Add(new SqlParameter("@Identificacao", turma.Id));
                }
                var retorno = _context.ListarObjeto<RetornoProcedure>(turma.Id > 0? "sp_atualizarTurma" : "sp_inserirTurma", parametros.ToArray());
            
                if (retorno.Mensagem == "Ok"){
                    return RedirectToAction("Index");
                } else {
                    ModelState.AddModelError("", retorno.Mensagem);
                    
                }
            }

            ViewBagEscolas();
            return View(turma);
        }

        public JsonResult Excluir(int id){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@Identificacao", id)
            };
            var retorno = _context.ListarObjeto<RetornoProcedure>("sp_excluirTurma", parametros);
            return new JsonResult(new {Sucesso = retorno.Mensagem == "Excluído", Mensagem = retorno.Mensagem });
        }

        public PartialViewResult ListaPartialView(int idEscola){
            
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@idEscola", idEscola)                
            };
            List<Turma> turmas = _context.RetornarLista<Turma>("sp_consultarTurma", parametros);
            
            
            HttpContext.Session.SetInt32("IdEscola", idEscola);
            
            return PartialView(turmas.ToPagedList(1, itensPorPagina));
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
    }
}
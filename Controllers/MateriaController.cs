using System.Collections.Generic;
using System.Data.SqlClient;
using Escola.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Escola.Controllers 
{
    public class MateriaController : Controller
    {
        private readonly ILogger<MateriaController> _logger;  
        private readonly DadosContext _context;
        const int itensPorPagina = 5;
  
        public MateriaController(ILogger<MateriaController> logger, DadosContext context)
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
            List<Models.Materia> materias = _context.RetornarLista<Models.Materia>("sp_consultarMateria", parametros);
            
            return View(materias.ToPagedList(numeroPagina, itensPorPagina));
        }

        public IActionResult Detalhe(int id)
        {
            Models.Materia materia = new Models.Materia();
            if (id > 0)  {
                SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@identificacao", id)
            };
                materia = _context.ListarObjeto<Models.Materia>("sp_buscarMateriaPorId", parametros); 
            }

            return View(materia);
        }

        [HttpPost]
        public IActionResult Detalhe(Models.Materia materia){

            if(string.IsNullOrEmpty(materia.Nome)){
                ModelState.AddModelError("", "O nome não pode ser vazio");
            }    

            if(ModelState.IsValid){
           
                List<SqlParameter> parametros = new List<SqlParameter>(){
                    new SqlParameter("@Nome", materia.Nome)
                };

                if (materia.Id > 0){
                    parametros.Add(new SqlParameter("@Identificacao", materia.Id));
                }
                var retorno = _context.ListarObjeto<RetornoProcedure>(materia.Id > 0? "sp_atualizarMateria" : "sp_inserirMateria", parametros.ToArray());
            
                if (retorno.Mensagem == "Ok"){
                    return RedirectToAction("Index");
                } else {
                    ModelState.AddModelError("", retorno.Mensagem);   
                }
            }

            return View(materia);
        }

        public JsonResult Excluir(int id){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@Identificacao", id)
            };
            var retorno = _context.ListarObjeto<RetornoProcedure>("sp_excluirMateria", parametros);
            return new JsonResult(new {Sucesso = retorno.Mensagem == "Excluído", Mensagem = retorno.Mensagem });
        }

        public PartialViewResult ListaPartialView(string nome){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@nome", nome)
            };
            List<Models.Materia> materias = _context.RetornarLista<Models.Materia>("sp_consultarMateria", parametros);
            if (string.IsNullOrEmpty(nome)){
                HttpContext.Session.Remove("TextoPesquisa");
            } else {
            HttpContext.Session.SetString("TextoPesquisa", nome);
            }

            return PartialView(materias.ToPagedList(1, itensPorPagina));
        }
      

    }
}
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
    public class GradeAulaController : Controller
    {
        private readonly ILogger<GradeAulaController> _logger;  
        private readonly DadosContext _context;
        const int itensPorPagina = 5;
  
        public GradeAulaController(ILogger<GradeAulaController> logger, DadosContext context)
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
            List<Models.GradeAula> gradeaulas = _context.RetornarLista<Models.GradeAula>("sp_consultarGradeAula", parametros);
            
            ViewBagEscolas();
            return View(gradeaulas.ToPagedList(numeroPagina, itensPorPagina));
        }

        public IActionResult Detalhe(int id)
        {
            Models.GradeAula gradeaula = new Models.GradeAula();
            if (id > 0)  {
                SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@identificacao", id)
            };
                gradeaula = _context.ListarObjeto<Models.GradeAula>("sp_buscarGradeAulaPorId", parametros); 
            } 

            return new JsonResult(new {Sucesso = gradeaula.Id > 0, GradeAula = gradeaula});
        }

        [HttpPost]
        public IActionResult Detalhe(Models.GradeAula gradeaula){

            string mensagem = "";
              

            if(ModelState.IsValid){
           
                List<SqlParameter> parametros = new List<SqlParameter>(){
                    new SqlParameter("@IdTurma", gradeaula.IdTurma),
                    new SqlParameter("@IdProfessor", gradeaula.IdProfessor),
                    new SqlParameter("@IdMateria", gradeaula.IdMateria)

                };
                if (gradeaula.Id > 0){
                    parametros.Add(new SqlParameter("@Identificacao", gradeaula.Id));
                } 
               
                var retorno = _context.ListarObjeto<RetornoProcedure>(gradeaula.Id > 0? "sp_atualizarGradeAula" : "sp_inserirGradeAula", parametros.ToArray());
            
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
                new SqlParameter("@Identificacao", id)
            };
            var retorno = _context.ListarObjeto<RetornoProcedure>("sp_excluirGradeAula", parametros);
            return new JsonResult(new {Sucesso = retorno.Mensagem == "Excluído", Mensagem = retorno.Mensagem });
        }

        public PartialViewResult ListaPartialView(int idescola){
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@idEscola", idescola)
            };
            List<Models.GradeAula> gradeaulas = _context.RetornarLista<Models.GradeAula>("sp_consultarGradeAula", parametros);

            HttpContext.Session.SetInt32("IdEscola", idescola);

            return PartialView(gradeaulas.ToPagedList(1, itensPorPagina));
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

        private void ViewBagProfessores(){
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@nome", "")
            };
            List<Models.Professor> professores = new List<Models.Professor>(); 
            professores = _context.RetornarLista<Models.Professor>("sp_consultarProfessor", param);
            
            ViewBag.Professores = professores.Select(c => new SelectListItem(){
                Text= c.Nome, Value= c.Id.ToString()
            }).ToList();
        }

        private void ViewBagMaterias(){
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@nome", "")
            };
            List<Models.Materia> materias = new List<Models.Materia>(); 
            materias = _context.RetornarLista<Models.Materia>("sp_consultarMateria", param);
            
            ViewBag.Materias = materias.Select(c => new SelectListItem(){
                Text= c.Nome, Value= c.Id.ToString()
            }).ToList();
        }
           

      

    }
}
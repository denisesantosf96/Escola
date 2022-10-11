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
    public class BoletimController : Controller
    {
        private readonly ILogger<BoletimController> _logger;
        private readonly DadosContext _context;
        const int itensPorPagina = 5;

        public BoletimController(ILogger<BoletimController> logger, DadosContext context)
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
            List<Models.Boletim> boletins = _context.RetornarLista<Models.Boletim>("sp_consultarBoletim", parametros);

            ViewBagEscolas();
            return View(boletins.ToPagedList(numeroPagina, itensPorPagina));
        }

        public IActionResult Detalhe(int? idTurma, int idEscola)
        {
            ViewBag.IdTurma = idTurma;
            ViewBagTurmas(idEscola);
            ViewBagAlunos();
            return View();

        }

        [HttpPost]
        public IActionResult Detalhe(Models.Boletim boletim)
        {

            string mensagem = "";

            List<SqlParameter> parametros = new List<SqlParameter>(){
                    new SqlParameter("@IdAluno", boletim.IdAluno),
                    new SqlParameter("@IdGradeAula", boletim.IdGradeAula),
                    new SqlParameter("@DescricaoAvaliacao", boletim.DescricaoAvaliacao),
                    new SqlParameter("@Nota", boletim.Nota)
            };

            if (boletim.Id > 0)
            {
                parametros.Add(new SqlParameter("@Identificacao", boletim.Id));
            }
            var retorno = _context.ListarObjeto<RetornoProcedure>(boletim.Id > 0 ? "sp_atualizarBoletim" : "sp_inserirBoletim", parametros.ToArray());

            if (retorno.Mensagem == "Ok")
            {
                return new JsonResult(new { Sucesso = retorno.Mensagem == "Ok" });
            }
            else
            {
                mensagem = retorno.Mensagem;
            }

            return new JsonResult(new { Sucesso = false, Mensagem = mensagem });
        }

        public JsonResult Excluir(int id)
        {
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@Identificacao", id)
            };
            var retorno = _context.ListarObjeto<RetornoProcedure>("sp_excluirBoletim", parametros);
            return new JsonResult(new { Sucesso = retorno.Mensagem == "Excluído", Mensagem = retorno.Mensagem });
        }

        public PartialViewResult ListaPartialView(int idEscola)
        {
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@idEscola", idEscola)
            };
            List<Models.Boletim> boletins = _context.RetornarLista<Models.Boletim>("sp_consultarBoletim", parametros);

            HttpContext.Session.SetInt32("IdEscola", idEscola);

            return PartialView(boletins.ToPagedList(1, itensPorPagina));
        }

        public PartialViewResult ListaPartialViewDetalhe(int idTurma)
        {
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@identificacao", idTurma)
            };
            List<Models.Boletim> boletins = _context.RetornarLista<Models.Boletim>("sp_buscarBoletimPorId", parametros);

            HttpContext.Session.SetInt32("IdTurma", idTurma);

            return PartialView(boletins.ToPagedList(1, itensPorPagina));
        }

        public JsonResult TrazerIdGradeAula(int id, int idTurma)
        {
            SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@id", id),
                new SqlParameter("@idTurma", idTurma)
            };

            Models.GradeAula gradeaula = _context.ListarObjeto<Models.GradeAula>("sp_consultarGradeAulaPorTurma", parametros);

            return new JsonResult(new { gradeaula });
        }

        private void ViewBagEscolas()
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@nome", "")
            };
            List<Models.Escola> escolas = new List<Models.Escola>();
            escolas = _context.RetornarLista<Models.Escola>("sp_consultarEscola", param);

            ViewBag.Escolas = escolas.Select(c => new SelectListItem()
            {
                Text = c.Nome,
                Value = c.Id.ToString()
            }).ToList();
        }

        private void ViewBagAlunos()
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@nome", "")
            };
            List<Models.Aluno> alunos = new List<Models.Aluno>();
            alunos = _context.RetornarLista<Models.Aluno>("sp_consultarAluno", param);

            ViewBag.Alunos = alunos.Select(c => new SelectListItem()
            {
                Text = c.Id + " - " + c.Nome,
                Value = c.Id.ToString()
            }).ToList();
        }

        private void ViewBagTurmas(int idEscola)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@idEscola", idEscola)
            };
            List<Models.Turma> turmas = new List<Models.Turma>();
            turmas = _context.RetornarLista<Models.Turma>("sp_consultarTurma", param);

            ViewBag.Turmas = turmas.Select(c => new SelectListItem()
            {
                Text = c.Id + " - " + c.Tipo + " - " + c.Serie + " - " + c.Descricao,
                Value = c.Id.ToString()
            }).ToList();
        }

    }

}

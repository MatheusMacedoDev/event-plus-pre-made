using System.ComponentModel.Design;
using webapi.event_.Contexts;
using webapi.event_.Domains;
using webapi.event_.Interfaces;

namespace webapi.event_.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly Event_Context _context;

        public EventoRepository()
        {
            _context = new Event_Context();
        }

        public void Atualizar(Guid id, Evento evento)
        {
            try
            {
                Evento eventoBuscado = _context.Evento.Find(id)!;

                if (eventoBuscado != null)
                {
                    eventoBuscado.DataEvento = evento.DataEvento;
                    eventoBuscado.NomeEvento = evento.NomeEvento;
                    eventoBuscado.Descricao = evento.Descricao;
                    eventoBuscado.IdTipoEvento = evento.IdTipoEvento;
                }

                _context.Evento.Update(eventoBuscado!);

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Evento BuscarPorId(Guid id)
        {
            try
            {
                return _context.Evento.Select(evento => new Evento
                {
                    IdEvento = evento.IdEvento,
                    NomeEvento = evento.NomeEvento,
                    Descricao = evento.Descricao,
                    DataEvento = evento.DataEvento,
                    IdTipoEvento = evento.IdTipoEvento,
                    TiposEvento = new TiposEvento
                    {
                        IdTipoEvento = evento.IdTipoEvento,
                        Titulo = evento.TiposEvento.Titulo
                    },
                    IdInstituicao = evento.IdInstituicao,
                    Instituicao = new Instituicao
                    {
                        IdInstituicao = evento.IdInstituicao,
                        NomeFantasia = evento.Instituicao.NomeFantasia
                    }
                }).FirstOrDefault(evento => evento.IdEvento == id)!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Cadastrar(Evento evento)
        {
            try
            {
                _context.Evento.Add(evento);

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public void Deletar(Guid id)
        {
            try
            {
                Evento eventoBuscado = _context.Evento.Find(id)!;

                if (eventoBuscado != null)
                {
                    _context.Evento.Remove(eventoBuscado);
                }

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public List<Evento> Listar()
        {
            try
            {
                return _context.Evento.Select(evento => new Evento
                {
                    IdEvento = evento.IdEvento,
                    NomeEvento = evento.NomeEvento,
                    Descricao = evento.Descricao,
                    DataEvento = evento.DataEvento,
                    IdTipoEvento = evento.IdTipoEvento,
                    TiposEvento = new TiposEvento
                    {
                        IdTipoEvento = evento.IdTipoEvento,
                        Titulo = evento.TiposEvento.Titulo
                    },
                    IdInstituicao = evento.IdInstituicao,
                    Instituicao = new Instituicao
                    {
                        IdInstituicao = evento.IdInstituicao,
                        NomeFantasia = evento.Instituicao.NomeFantasia
                    }
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Evento> ListarProximos()
        {
            try
            {
                return _context.Evento
                    .Where(e => e.DataEvento >= DateTime.Now).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Evento> ListarAnteriores()
        {
            try
            {
                return _context.Evento
                    .Where(e => e.DataEvento < DateTime.Now).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

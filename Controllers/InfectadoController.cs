using Api.Data.Collections;
using infected.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Linq;

namespace infected.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InfectadoController : ControllerBase
    {

        Data.MongoDB _mongoDB;

        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        [Route("adicionar")]
        public IActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.NomeVirus, dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adcicionado com sucesso");
        }


        [HttpGet]
        [Route("buscar")]
        public IActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        [HttpPut]
        [Route("alterar")]
        public IActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
        {

            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(u => u.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo",dto.Sexo));
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(u => u.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("NomeVirus", dto.NomeVirus));
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(u => u.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("Latitude", dto.Latitude));
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(u => u.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("Longitude", dto.Longitude));


            return Ok("Atualizado com sucesso");
        }


        [HttpDelete]
        [Route("deletar/{dataNasc}")]
        public IActionResult DeletarInfectado(DateTime dataNasc)
        {

            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(u => u.DataNascimento == dataNasc));
      

            return Ok("Deletado com sucesso");
        }

    }
}

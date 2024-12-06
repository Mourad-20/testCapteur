using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Model;
using Model.DataModel;
using Model.ViewModel;
using Repositories;
using Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestioCapteur.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapteurController : ControllerBase
    {
        private readonly ContextProject _context;
        private readonly ICapteurRepository _capteurRepo;
        private readonly IDistributedCache _distributedCache;

        public CapteurController(ContextProject context, ICapteurRepository capteurRepo, IDistributedCache distributedCache)
        {
            _context = context;
            _capteurRepo=capteurRepo ;
            _distributedCache = distributedCache;
        }
        // GET: api/<CapteurController>
        // Version accepte 'Api-Version' in header
        [HttpGet]
        [ApiVersion("2.0")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<CapteurVM>>> GetAll()
        {
            try
            {
                ICollection<Capteur> capteurs = await _capteurRepo.GetAll();
                if (capteurs == null || !capteurs.Any())
                {
                    return NoContent(); // 204 No Content
                }
                List<CapteurVM> capteurVMs = capteurs.Select(capteur => new CapteurVM()
                {
                    Id = capteur.Id,
                    Capteur_Name = capteur.Name,
                    Capteur_Type = capteur.Type,
                    Capteur_Value = capteur.Value
                }).ToList();
                return Ok(capteurVMs);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Erreur serveur: {ex.Message}");
            }
          
        }

        // GET api/<CapteurController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CapteurVM>> Get(int id)
        {
            try{
                string cacheKey = $"Capteur_{id}";
                string cachedCapteur = await _distributedCache.GetStringAsync(cacheKey);
                if (cachedCapteur == null)
                {
                    Capteur capteur = await _capteurRepo.GetById(id);
                    CapteurVM capteurVM = new CapteurVM()
                    {
                        Id = capteur.Id,
                        Capteur_Name = capteur.Name,
                        Capteur_Type = capteur.Type,
                        Capteur_Value = capteur.Value
                    };
                    cachedCapteur = System.Text.Json.JsonSerializer.Serialize(capteurVM);
                    await _distributedCache.SetStringAsync(cacheKey, cachedCapteur, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                }
            return Ok(System.Text.Json.JsonSerializer.Deserialize<CapteurVM>(cachedCapteur));
            }
            catch(Exception ex){
                return StatusCode(500, $"Erreur serveur: {ex.Message}");
            }
            
        }

        // POST api/<CapteurController>
        [HttpPost]
        public async Task<ActionResult<CapteurVM>> Post([FromBody] CapteurVM value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Retourne les erreurs de validation
            }
            try
            {
                Capteur capteur = new Capteur()
                {
                    Name = value.Capteur_Name,
                    Type = value.Capteur_Type,
                    Value = value.Capteur_Value,
                    Dt_Modif = DateTime.Now
                };
                var result = await _capteurRepo.Add(capteur) == 1;
                if (result)
                {
                    value.Id = capteur.Id;
                    return Ok(value);
                }
                return StatusCode(500, "Erreur lors de l'insertion du capteur."); // Erreur serveur

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur: {ex.Message}");  
            }

        }

        // PUT api/<CapteurController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CapteurVM>> Put(int id, [FromBody] CapteurVM value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Retourne les erreurs de validation
            }

            try
            {
                // Récupère le capteur existant
                var existingCapteur = await _capteurRepo.GetById(id);
                if (existingCapteur == null)
                {
                    return NotFound("Not Found");
                }

                // Met à jour les propriétés
                existingCapteur.Name = value.Capteur_Name;
                existingCapteur.Type = value.Capteur_Type;
                existingCapteur.Value = value.Capteur_Value ?? existingCapteur.Value;
                existingCapteur.Dt_Modif = DateTime.Now;

                // Sauvegarde les modifications
                var result = _capteurRepo.Update(existingCapteur);
                if (result)
                {
                    value.Id = existingCapteur.Id;
                    return Ok(value); 
                }

                return StatusCode(500, "Erreur lors de la mise à jour du capteur.");  // Erreur serveur
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur: {ex.Message}");  
            }
        }
        // DELETE api/<CapteurController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Object>> Delete(int id)
        {
            try
            {
                var existingCapteur = await _capteurRepo.GetById(id);
                if (existingCapteur == null)
                {
                    return NotFound("Not Found");
                }
                bool result =  _capteurRepo.Delete(existingCapteur);
                if (result)
                {
                    string cacheKey = $"Capteur_{id}";
                    await _distributedCache.RemoveAsync(cacheKey);
                }
                var res= new ResponsDeletCapteur
                {
                    Id=existingCapteur.Id,
                    Message=(result)? "supprimé avec succès" : "Erreur serveur"
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur: {ex.Message}");
            }
        }
    }
}

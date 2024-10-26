using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OP.Repository.Interfaces;

namespace OP.Controllers.OperatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorsController : ControllerBase
    {
        private readonly IOperatorRepo _repository;
        public OperatorsController()
        {
            
        }
    }
}
